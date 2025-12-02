// - Deserialization of sum types works by reading the "command" field, or just by their snake case string name
//   (I could implement more but there's no point in doing that)
// - Deserialization of enums works by using their snake case name
// - Serialization of sum types depends on whether the `TagName` attr is present
//    - if it is, it's used as the tag and the rest of the case structure is flattened
//    - if it isn't, it's serialized as a single-attr record
// - Before converting to snake case, all _s are replaced with /s
// - All field names are converted to camel case
// - Serialization of sum types works by either serializing their name as string or an object with a single key
// - Bools, strings, int types, lists, arrays, options are supported, but not much else
// - For json schema purposes, options are considered to be optional rather than nullable values - this is easier to read for LLMs
// - Incidentally, there's no other way to mark a field as optional for deserialization

namespace NeuroFSharp

open System
open System.Collections
open System.IO
open System.Linq
open System.Net.WebSockets
open System.Reflection
open System.Runtime.CompilerServices
open System.Text
open System.Threading
open System.Threading.Tasks
open System.Text.RegularExpressions
open FSharp.Reflection

[<assembly: InternalsVisibleTo("NeuroShogunTests")>]
do ()

type UnionAttr() =
    inherit Attribute()

    abstract member TagName: string option
    default _.TagName = None

type TagName(name: string) =
    inherit UnionAttr()

    override _.TagName = Some name

type CaseAttr() =
    inherit Attribute()

    abstract member MapFieldNames: (string * string) list
    default _.MapFieldNames = []

// a hack for union case fields not supporting attributes in F#
// this is very limited, only to be used for action deserialization purposes
type RenameCaseField(from: string, ``to``: string) =
    inherit CaseAttr()

    override _.MapFieldNames = [ from, ``to`` ]

type FieldAttr() =
    inherit Attribute()

    abstract member Serialized: name: string -> value: obj -> serialized: Lazy<JsonValue> -> (string * JsonValue) list
    abstract member ExtraDeserializationNames: string list

    default _.Serialized name _ serialized = [ (name, serialized.Value) ]
    default _.ExtraDeserializationNames = []

type SkipSerializingIfEquals(x: obj) =
    inherit FieldAttr()

    override _.Serialized name value serialized =
        if x = value then [] else [ (name, serialized.Value) ]

type SkipSerializingIfNone() =
    inherit FieldAttr()

    override _.Serialized name value serialized =
        if
            value = null
            || FSharpValue.GetUnionFields(value, value.GetType()) |> snd |> Array.length = 0
        then
            []
        else
            [ (name, serialized.Value) ]

type RenameField(name: string) =
    inherit FieldAttr()

    override _.Serialized _ _ serialized = [ (name, serialized.Value) ]
    override _.ExtraDeserializationNames = [ name ]

type internal TypeInfo' =
    | Record of Property array * Attribute array
    | List of TypeInfo
    | Array of TypeInfo
    | Union of Case array * Attribute array
    | Enum of (string * obj) array * Attribute array
    | Option of TypeInfo
    | PrimitiveString
    | PrimitiveBool
    | PrimitiveInt
    | PrimitiveFloat
    | Serializable

and internal TypeInfo = Type * TypeInfo'

and internal Property =
    { propName: string
      info: PropertyInfo
      ty: TypeInfo
      attrs: Attribute array }

and internal Case =
    { caseName: string
      info: UnionCaseInfo
      attrs: obj array
      props: Property array }

type PathElem =
    | Index of int
    | Prop of string

type JsonPath = PathElem list

[<StructuredFormatDisplay("{DisplayText}")>]
type DeserError =
    { path: JsonPath
      error: string }

    member this.DisplayText = this.ToString()

    override this.ToString() =
        let path =
            String.Join(
                "",
                this.path
                |> List.rev
                |> List.map (fun x ->
                    match x with
                    | Prop x -> $".\"{x}\""
                    | Index x -> $"[{x}]")
            )

        $"Error while deserializing field <root>{path}: {this.error}"

module DeserError =
    let internal unexpected (unexpected: JsonValue) (expected: string) (path: JsonPath) : DeserError =
        let unexpected' =
            match unexpected with
            | JsonValue.String _ -> "string"
            | JsonValue.Record _ -> "object"
            | JsonValue.Array _ -> "array"
            | JsonValue.Null -> "null"
            | JsonValue.Float _ -> "number"
            | JsonValue.Number _ -> "number"
            | JsonValue.Boolean _ -> "boolean"

        { path = path
          error = $"unexpected {unexpected'}, expected a {expected}" }

    let internal case (unexpected: string) (expected: string array) (path: JsonPath) : DeserError =
        let sep = "\", \""
        let expected' = $"\"{String.concat sep expected}\""

        { path = path
          error = $"unexpected value \"{unexpected}\", expected one of {expected'}" }

    let internal caseData (path: JsonPath) : DeserError =
        { path = path
          error = $"unexpected string, expected an object" }

    let internal missingField (path: JsonPath) : DeserError =
        let field =
            match List.head path with
            | Prop x -> $"field \"{x}\""
            | Index x -> $"item {x}"

        { path = List.tail path
          error = $"missing {field}" }

    let internal exc (exc: string) (path: JsonPath) : DeserError =
        { path = path
          error = $"caught exception: {exc}" }

type SchemaType =
    | Boolean
    | String
    | Null
    | Number
    | Integer
    | Array
    | Object

module SchemaType =
    let toString ty =
        match ty with
        | Boolean -> "boolean"
        | String -> "string"
        | Null -> "null"
        | Number -> "number"
        | Integer -> "integer"
        | Array -> "array"
        | Object -> "object"

type ISerializable =
    abstract JsonValue: unit -> JsonValue

type Schema(ty: SchemaType) =
    interface ISerializable with
        override this.JsonValue() : JsonValue =
            JsonValue.Record(Array.ofList (List.rev (this.JsonProps())))

    // Set this to signal that this should be unregistered if already registered
    abstract member EqualTo: Schema -> bool

    default _.EqualTo(other: Schema) = ty = other.Type

    abstract member Valid: bool
    default _.Valid = true

    member _.Type = ty

    abstract member Clone: unit -> Schema
    default _.Clone() = Schema ty

    abstract member JsonProps: unit -> (string * JsonValue) list

    default this.JsonProps() =
        [ ("type", JsonValue.String(SchemaType.toString this.Type)) ]

    member this.JsonValue() : JsonValue = (this :> ISerializable).JsonValue()

type NullSchema() =
    inherit Schema(Null)

type StringSchema() =
    inherit Schema(String)

    let mutable enum: string array option = None
    let mutable enum0: string array option = None

    member _.InitialEnum
        with get () = enum0
        and set value = enum0 <- value

    member _.Enum
        with get () = enum
        and set value = enum <- value

    override _.EqualTo(other: Schema) =
        base.EqualTo other && enum = (other :?> StringSchema).Enum

    member _.SetEnum(values: string array) =
        let prev = Option.defaultValue Array.empty enum
        let cur = values

        if Option.isNone enum || prev.Length <> cur.Length || Array.exists2 (<>) prev cur then
            enum <- Some cur

    member _.RetainEnum(filter: string -> bool) =
        let prev = enum.Value
        let cur = Array.filter filter enum0.Value

        if prev.Length <> cur.Length || Array.exists2 (<>) prev cur then
            enum <- Some cur

    override _.Valid = not (enum |> Option.exists (Array.length >> (=) 0))

    override _.Clone() =
        let mutable ret = StringSchema()
        ret.InitialEnum <- enum0
        ret.Enum <- enum
        ret

    override _.JsonProps() : (string * JsonValue) list =
        (enum
         |> Option.map (fun x -> "enum", x |> Array.map JsonValue.String |> JsonValue.Array)
         |> Option.toList)
        @ base.JsonProps()

type ObjectSchema(properties: (string * Schema) array) =
    inherit Schema(Object)

    let mutable props = properties

    let mutable required: string array option = None

    override _.EqualTo(other: Schema) =
        base.EqualTo other
        && (let rhs = other :?> ObjectSchema

            required = rhs.Required
            && Array.length props = Array.length rhs.Properties
            && (props |> Array.forall2 (fun (k1, v1) (k2, v2) -> k1 = k2 && v1.EqualTo v2)) rhs.Properties)

    override _.Valid =
        match required with
        | None -> true
        | Some required ->
            let invalid = props |> Array.filter (snd >> _.Valid >> not)
            invalid |> Array.forall (fun (k, _) -> not (Array.contains k required))

    member _.Properties
        with get () = props
        and set value = props <- value

    member _.Required
        with get () = required
        and set value = required <- value

    override _.Clone() =
        let mutable ret = ObjectSchema(properties |> Array.map (fun (k, v) -> k, v.Clone()))

        ret.Properties <- props |> Array.map (fun (k, v) -> k, v.Clone())
        ret.Required <- required
        ret

    member _.MutateProp (name: string) (func: Schema -> unit) =
        props |> Array.find (fst >> (=) name) |> snd |> func

    override this.JsonProps() =
        (this.Required
         |> Option.map (fun x -> "required", x |> Array.map JsonValue.String |> JsonValue.Array)
         |> Option.toList)
        @ ("properties", JsonValue.Record(this.Properties |> Array.map (fun (s, x) -> s, x.JsonValue())))
          :: base.JsonProps()

type IntegerSchema() =
    inherit Schema(Integer)

    let mutable min = None
    let mutable xmin = None
    let mutable max = None
    let mutable xmax = None

    override _.EqualTo(other: Schema) =

        base.EqualTo other
        && (let rhs = other :?> IntegerSchema

            min = rhs.Minimum
            && max = rhs.Maximum
            && xmin = rhs.ExclusiveMinimum
            && xmax = rhs.ExclusiveMaximum)

    override _.Valid =
        true
        && match min, max with
           | None, _ -> true
           | _, None -> true
           | Some min, Some max -> min <= max
        && match xmin, xmax with
           | None, _ -> true
           | _, None -> true
           | Some xmin, Some xmax -> xmin + 1 < xmax
        && match xmin, max with
           | None, _ -> true
           | _, None -> true
           | Some xmin, Some max -> xmin < max
        && match min, xmax with
           | None, _ -> true
           | _, None -> true
           | Some min, Some xmax -> min < xmax

    member _.Minimum
        with get () = min
        and set value = min <- value

    member _.ExclusiveMinimum
        with get () = xmin
        and set value = xmin <- value

    member _.Maximum
        with get () = max
        and set value = max <- value

    member _.ExclusiveMaximum
        with get () = xmax
        and set value = xmax <- value

    override _.Clone() =
        let mutable ret = IntegerSchema()
        ret.Minimum <- min
        ret.Maximum <- max
        ret.ExclusiveMinimum <- xmin
        ret.ExclusiveMaximum <- xmax

        ret

    override this.JsonProps() =
        let proc s x n =
            match x with
            | Some x -> (s, JsonValue.Number(decimal x)) :: n
            | None -> n

        base.JsonProps()
        |> proc "minimum" this.Minimum
        |> proc "exclusiveMinimum" this.ExclusiveMinimum
        |> proc "maximum" this.Maximum
        |> proc "exclusiveMaximum" this.ExclusiveMaximum

type NumberSchema() =
    inherit Schema(Number)
    override _.Clone() = NumberSchema()

type BooleanSchema() =
    inherit Schema(Boolean)
    override _.Clone() = BooleanSchema()

type ArraySchema(items: Schema) =
    inherit Schema(Array)

    let mutable unique = false
    let mutable minItems = 0
    let mutable maxItems = None

    override _.EqualTo(other: Schema) =
        base.EqualTo other
        && (let rhs = other :?> ArraySchema

            unique = rhs.Unique
            && minItems = rhs.MinItems
            && maxItems = rhs.MaxItems
            && items.EqualTo rhs.Items)

    member _.Items = items

    override _.Valid = items.Valid

    member _.Unique
        with get () = unique
        and set value = unique <- value

    member _.MinItems
        with get () = minItems
        and set value = minItems <- value

    member _.MaxItems
        with get () = maxItems
        and set value = maxItems <- value

    override this.JsonProps() : (string * JsonValue) list =
        (if unique then
             [ ("uniqueItems", JsonValue.Boolean true) ]
         else
             [])
        @ Option.toList (Option.map (fun x -> "maxItems", JsonValue.Number(decimal x)) maxItems)
        @ (if minItems = 0 then
               []
           else
               [ ("minItems", JsonValue.Number(decimal minItems)) ])
        @ ("items", this.Items.JsonValue()) :: base.JsonProps()


    override _.Clone() =
        let mutable ret = ArraySchema(items.Clone())
        ret.Unique <- unique
        ret.MinItems <- minItems
        ret.MaxItems <- maxItems
        ret

type Action(name: string, description: string) =
    inherit Attribute()
    let mutable schema: ObjectSchema option = None
    let mutable schema1: ObjectSchema option = None
    let mutable desc = description

    abstract member Name: string
    abstract member MutableName: bool
    abstract member Description: string with get, set
    abstract member InitialDescription: string
    abstract member Schema: ObjectSchema option with get, set
    abstract member Clone: unit -> Action

    interface ISerializable with
        override this.JsonValue() : JsonValue =
            JsonValue.Record(
                match this.Schema with
                | None ->
                    [| "name", JsonValue.String this.Name
                       "description", JsonValue.String this.Description |]
                | Some schema ->
                    [| "name", JsonValue.String this.Name
                       "description", JsonValue.String this.Description
                       "schema", schema.JsonValue() |]
            )

    member this.Valid = not (this.Schema |> Option.exists (_.Valid >> not))
    default _.Name = name
    default this.MutableName = this.GetType() <> typeof<Action>
    default _.InitialDescription = description

    member this.EqualTo(other: Action) =
        this.Name = other.Name
        && this.Description = other.Description
        && (Option.isNone this.Schema && Option.isNone other.Schema
            || Option.defaultValue false ((this.Schema |> Option.map2 (fun a b -> a.EqualTo b)) other.Schema))

    member _.InitialSchema
        with get () = schema1
        and set (value: ObjectSchema option) = schema1 <- value

    default this.Clone() =
        let mutable ret = Action(this.Name, this.InitialDescription)
        ret.Description <- this.Description
        ret.InitialSchema <- this.InitialSchema |> Option.map (fun x -> x.Clone() :?> ObjectSchema)
        ret.Schema <- this.Schema |> Option.map (fun x -> x.Clone() :?> ObjectSchema)
        ret

    default _.Description
        with get () = desc
        and set value =
            if desc <> value then
                desc <- value

    default _.Schema
        with get () = schema
        and set (value: ObjectSchema option) = schema <- value

    member this.MutateProp (name: string) (func: Schema -> unit) = this.Schema.Value.MutateProp name func

module internal TypeInfo =
    let pascalToSnake (s: string) : string =
        let s' = s.Replace("_", "/")
        Regex.Replace(s', @"(\w)([A-Z])", "$1_$2").ToLower()

    let pascalToCamel (s: string) : string = string(s[0]).ToLower() + s.Substring 1

    let fillMissing ((x, y): TypeInfo) : obj option =
        match y with
        | Option _ ->
            let cases = FSharpType.GetUnionCases x
            Some(FSharpValue.MakeUnion(cases.[0], Array.empty))
        | _ -> None

    let optional ((_, y): TypeInfo) =
        match y with
        | Option _ -> true
        | _ -> false

    let rec schema (ty: TypeInfo) : Schema =
        match snd ty with
        | PrimitiveBool -> BooleanSchema()
        | PrimitiveInt -> IntegerSchema()
        | PrimitiveFloat -> NumberSchema()
        | PrimitiveString -> StringSchema()
        | Option x -> schema x
        | Enum(x, _) ->
            let mutable ret = StringSchema()
            ret.InitialEnum <- Some(Array.map fst x)
            ret.Enum <- Some(Array.map fst x)
            ret
        | TypeInfo'.Array x
        | List x -> ArraySchema(schema x)
        | Serializable -> raise (Exception "can't generate a schema for arbitrary types")
        | Record(x, _) ->
            let mutable ret = ObjectSchema(x |> Array.map (fun x -> x.propName, schema x.ty))

            ret.Required <- Some(x |> Array.filter (_.ty >> optional >> not) |> Array.map _.propName)
            ret
        | Union(x, _) ->
            // this is not supposed to be used anywhere so just use same schema as enum
            let mutable ret = StringSchema()
            ret.InitialEnum <- Some(Array.map _.caseName x)
            ret.Enum <- Some(Array.map _.caseName x)
            ret

    let unionSchema (ty: Case) : ObjectSchema option =
        if ty.props.Length = 0 then
            None
        else
            let mutable ret =
                ObjectSchema(ty.props |> Array.map (fun x -> x.propName, schema x.ty))

            ret.Required <- Some(ty.props |> Array.filter (_.ty >> optional >> not) |> Array.map _.propName)
            Some ret

    let rec propInfo (info: PropertyInfo) =
        { propName = pascalToCamel info.Name
          ty = fromSystemType info.PropertyType
          info = info
          attrs = info.GetCustomAttributes() |> Array.ofSeq }

    and caseInfo (info: UnionCaseInfo) =
        { caseName = pascalToSnake info.Name
          info = info
          props = Array.map propInfo (info.GetFields())
          attrs = info.GetCustomAttributes() |> Array.ofSeq }

    and fromSystemType (ty: Type) : TypeInfo =
        let objArr (arr: Array) : obj array =
            [| 1 .. arr.Length |] |> Array.map (fun i -> arr.GetValue(i - 1))

        ty,
        match ty with
        | ty when ty.IsEnum ->
            let names = Array.map pascalToSnake (Enum.GetNames ty)
            let vals = objArr (Enum.GetValues ty)
            Enum(Array.zip names vals, ty.GetCustomAttributes() |> Array.ofSeq)
        | ty when ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<option<_>> ->
            let ty' = ty.GetGenericArguments().[0]
            Option(fromSystemType ty')
        | ty when ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<list<_>> ->
            let ty' = ty.GetGenericArguments().[0]
            List(fromSystemType ty')
        | ty when ty.IsArray ->
            let ty' = ty.GetElementType()
            TypeInfo'.Array(fromSystemType ty')
        | ty when FSharpType.IsRecord ty ->
            let props = Array.map propInfo (FSharpType.GetRecordFields ty)
            Record(props, ty.GetCustomAttributes() |> Array.ofSeq)
        | ty when FSharpType.IsUnion ty ->
            let cases = Array.map caseInfo (FSharpType.GetUnionCases ty)
            Union(cases, ty.GetCustomAttributes() |> Array.ofSeq)
        | ty when ty = typeof<string> -> PrimitiveString
        | ty when
            ty = typeof<int>
            || ty = typeof<int8>
            || ty = typeof<int16>
            || ty = typeof<int32>
            || ty = typeof<int64>
            || ty = typeof<uint>
            || ty = typeof<uint8>
            || ty = typeof<uint16>
            || ty = typeof<uint32>
            || ty = typeof<uint64>
            || ty = typeof<bigint>
            || ty = typeof<byte>
            || ty = typeof<sbyte>
            ->
            PrimitiveInt
        | ty when
            ty = typeof<single>
            || ty = typeof<double>
            || ty = typeof<float>
            || ty = typeof<float32>
            || ty = typeof<decimal>
            ->
            PrimitiveFloat
        | ty when ty = typeof<bool> -> PrimitiveBool
        | ty when Array.contains typeof<ISerializable> (ty.GetInterfaces()) -> Serializable
        | _ -> raise (NotImplementedException $"type info for type {ty.FullName} is not implemented")

    let rec serialize ((ty, info): TypeInfo) (obj: obj) : JsonValue =
        let serField (info: Property, value: obj) : (string * JsonValue) list =
            let def = lazy (serialize info.ty value)

            match
                info.attrs
                |> Seq.ofArray
                |> Seq.map (function
                    | :? FieldAttr as x -> Seq.singleton x
                    | _ -> Seq.empty)
                |> Seq.concat
                |> Seq.fold
                    (fun state x ->
                        match state with
                        | None -> Some(x.Serialized info.propName value def)
                        | Some [ name, ser ] -> Some(x.Serialized name value (lazy ser))
                        | Some x -> Some x)
                    None
            with
            | None -> [ (info.propName, def.Value) ]
            | Some x -> x

        match info with
        | Enum(_name, _attrs) ->
            let name = pascalToSnake (Enum.GetName(ty, obj))
            JsonValue.String name
        | List info
        | TypeInfo'.Array info ->
            let list = obj :?> IEnumerable

            let x = list.Cast<Object>() |> Seq.map (serialize info) |> Array.ofSeq
            x |> JsonValue.Array
        | Record(info, _attrs) ->
            info
            |> Seq.ofArray
            |> Seq.map (fun info -> info, FSharpValue.GetRecordField(obj, info.info))
            |> Seq.map serField
            |> Seq.concat
            |> Array.ofSeq
            |> JsonValue.Record
        | Union(_, attrs) ->
            let tag, objs = FSharpValue.GetUnionFields(obj, ty)
            let info = caseInfo tag

            let ret =
                info.props
                |> Seq.ofArray
                |> Seq.zip objs
                |> Seq.map (fun (a, b) -> b, a)
                |> Seq.map serField
                |> Seq.concat

            let tagName =
                attrs
                |> Seq.ofArray
                |> Seq.map (function
                    | :? UnionAttr as x -> Seq.singleton x
                    | _ -> Seq.empty)
                |> Seq.concat
                |> Seq.tryPick _.TagName

            match tagName with
            | Some tagName ->
                ret
                |> Seq.append (Seq.singleton (tagName, JsonValue.String info.caseName))
                |> Array.ofSeq
                |> JsonValue.Record
            | None ->
                if Seq.isEmpty ret then
                    JsonValue.String info.caseName
                else
                    JsonValue.Record [| (info.caseName, ret |> Array.ofSeq |> JsonValue.Record) |]
        | Option info ->
            let _, fields = FSharpValue.GetUnionFields(obj, ty)

            match fields with
            | [| x |] -> serialize info x
            | _ -> JsonValue.Null
        | Serializable -> (obj :?> ISerializable).JsonValue()
        | PrimitiveBool -> JsonValue.Boolean(obj :?> bool)
        | PrimitiveString -> JsonValue.String(obj :?> string)
        | PrimitiveInt when ty = typeof<int> -> JsonValue.Number(decimal (obj :?> int))
        | PrimitiveInt when ty = typeof<int8> -> JsonValue.Number(decimal (obj :?> int8))
        | PrimitiveInt when ty = typeof<int16> -> JsonValue.Number(decimal (obj :?> int16))
        | PrimitiveInt when ty = typeof<int32> -> JsonValue.Number(decimal (obj :?> int32))
        | PrimitiveInt when ty = typeof<int64> -> JsonValue.Number(decimal (obj :?> int64))
        | PrimitiveInt when ty = typeof<uint> -> JsonValue.Number(decimal (obj :?> uint))
        | PrimitiveInt when ty = typeof<uint8> -> JsonValue.Number(decimal (obj :?> uint8))
        | PrimitiveInt when ty = typeof<uint16> -> JsonValue.Number(decimal (obj :?> uint16))
        | PrimitiveInt when ty = typeof<uint32> -> JsonValue.Number(decimal (obj :?> uint32))
        | PrimitiveInt when ty = typeof<uint64> -> JsonValue.Number(decimal (obj :?> uint64))
        | PrimitiveInt when ty = typeof<bigint> -> JsonValue.Number(decimal (obj :?> bigint))
        | PrimitiveInt when ty = typeof<byte> -> JsonValue.Number(decimal (obj :?> byte))
        | PrimitiveInt when ty = typeof<sbyte> -> JsonValue.Number(decimal (obj :?> sbyte))
        | PrimitiveFloat when ty = typeof<decimal> -> JsonValue.Number(decimal (obj :?> decimal))
        | PrimitiveFloat when ty = typeof<single> -> JsonValue.Float(float (obj :?> single))
        | PrimitiveFloat when ty = typeof<double> -> JsonValue.Float(float (obj :?> double))
        | PrimitiveFloat when ty = typeof<float> -> JsonValue.Float(float (obj :?> float))
        | PrimitiveFloat when ty = typeof<float32> -> JsonValue.Float(float (obj :?> float32))
        | PrimitiveInt
        | PrimitiveFloat -> raise (Exception "not supposed to happen")

    let rec readProps (nameMap: Map<string, string>) (path: JsonPath) props jsonProps =
        let src = Map.ofArray jsonProps

        let res, state =
            Ok()
            |> Array.mapFoldBack
                (fun (prop: Property) state ->
                    let fieldNames =
                        prop.attrs
                        |> Array.collect (function
                            | :? FieldAttr as x -> Array.ofList x.ExtraDeserializationNames
                            | _ -> Array.empty)
                        |> Array.append (Map.tryFind prop.propName nameMap |> Option.toArray)
                        |> Array.append [| prop.propName |]
                        |> Array.rev

                    match state with
                    | Ok() ->
                        match
                            fieldNames
                            |> Array.tryPick (fun x -> Map.tryFind x src |> Option.map (fun w -> x, w))
                        with
                        | Some(name, x) ->
                            match deserialize (Prop name :: path) prop.ty x with
                            | Ok x -> x, Ok()
                            | Error err -> null, Error err
                        | None ->
                            match fillMissing prop.ty with
                            | Some x -> x, Ok()
                            | None -> null, Error(DeserError.missingField (Prop(Array.head fieldNames) :: path))
                    | Error _ -> null, state)
                props

        Result.map (fun _ -> res) state

    and deserialize (path: JsonPath) (info: TypeInfo) (obj: JsonValue) : Result<obj, DeserError> =

        try
            let ty, info' = info

            match info', obj with
            | List info, JsonValue.Array y ->
                let listType = typedefof<List<_>>.MakeGenericType [| fst info |]
                let cons' = listType.GetMethod "Cons"
                let cons car cdr = cons'.Invoke(null, [| car; cdr |])
                let list = (listType.GetProperty "Empty").GetValue null

                Ok(y.Length - 1, list)
                |> Array.foldBack
                    (fun obj state ->
                        state
                        |> Result.bind (fun (i: int, ret: obj) ->
                            deserialize (Index i :: path) info obj
                            |> Result.map (fun x -> i - 1, cons x ret)))
                    y
                |> Result.map snd
            | TypeInfo'.Array info, JsonValue.Array y ->
                let arr = Array.CreateInstance(fst info, y.Length)

                y
                |> Array.fold
                    (fun state obj ->
                        state
                        |> Result.bind (fun (i: int, ret: Array) ->
                            deserialize (Index i :: path) info obj
                            |> Result.map (fun x ->
                                ret.SetValue(x, i)
                                i + 1, ret)))
                    (Ok((0, arr)))
                |> Result.map (fun x -> snd x :> obj)
            | Union(info, _), JsonValue.String name ->
                match info |> Array.tryFind (_.caseName >> (=) name) with
                | Some x when x.props.Length = 0 -> Ok(FSharpValue.MakeUnion(x.info, Array.empty))
                | Some _ -> Error(DeserError.caseData path)
                | None -> Error(DeserError.case name (info |> Array.map _.caseName) path)
            | Union(info, attrs), JsonValue.Record y ->
                let fieldMap =
                    attrs
                    |> Array.collect (function
                        | :? CaseAttr as x -> Array.ofList x.MapFieldNames
                        | _ -> [||])
                    |> Map.ofArray

                match y |> Array.tryFind (fst >> (=) "command") with
                | Some(_, JsonValue.String name) ->
                    match info |> Array.tryFind (_.caseName >> (=) name) with
                    | Some x ->
                        let res = readProps fieldMap path x.props y
                        Result.map (fun res -> FSharpValue.MakeUnion(x.info, res)) res
                    | None -> Error(DeserError.case name (info |> Array.map _.caseName) path)
                | Some(_, x) -> Error(DeserError.unexpected x "string" (Prop "command" :: path))
                | None -> Error(DeserError.missingField (Prop "command" :: path))
            | Record(props, _), JsonValue.Record y ->
                let res = readProps Map.empty path props y

                Result.map (fun res -> FSharpValue.MakeRecord(ty, res)) res
            | Enum(info, _), JsonValue.String name ->
                match info |> Array.tryFind (fst >> (=) name) with
                | Some(_, x) -> Ok x
                | None -> Error(DeserError.case name (Array.map fst info) path)
            | Option _, JsonValue.Null ->
                let cases = FSharpType.GetUnionCases ty
                Ok(FSharpValue.MakeUnion(cases.[0], Array.empty))
            | Option x, _ ->
                deserialize path x obj
                |> Result.map (fun x ->
                    let cases = FSharpType.GetUnionCases ty
                    FSharpValue.MakeUnion(cases.[1], [| x |]))
            | PrimitiveString, JsonValue.String x -> Ok(string x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<int> -> Ok(int x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<int8> -> Ok(int8 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<int16> -> Ok(int16 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<int32> -> Ok(int32 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<int64> -> Ok(int64 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<uint> -> Ok(uint x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<uint8> -> Ok(uint8 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<uint16> -> Ok(uint16 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<uint32> -> Ok(uint32 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<uint64> -> Ok(uint64 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<bigint> -> Ok(bigint x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<byte> -> Ok(byte x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<sbyte> -> Ok(sbyte x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<float> -> Ok(float x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<float32> -> Ok(float32 x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<single> -> Ok(single x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<double> -> Ok(double x)
            | PrimitiveInt, JsonValue.Number x when ty = typeof<decimal> -> Ok(decimal x)
            | PrimitiveFloat, JsonValue.Float x when ty = typeof<float> -> Ok(float x)
            | PrimitiveFloat, JsonValue.Float x when ty = typeof<float32> -> Ok(float32 x)
            | PrimitiveFloat, JsonValue.Float x when ty = typeof<single> -> Ok(single x)
            | PrimitiveFloat, JsonValue.Float x when ty = typeof<double> -> Ok(double x)
            | PrimitiveFloat, JsonValue.Float x when ty = typeof<decimal> -> Ok(decimal x)
            | PrimitiveBool, JsonValue.Boolean x -> Ok x
            | PrimitiveInt, _ -> Error(DeserError.unexpected obj "integer" path)
            | PrimitiveFloat, _ -> Error(DeserError.unexpected obj "number" path)
            | PrimitiveBool, _ -> Error(DeserError.unexpected obj "boolean" path)
            | PrimitiveString, _ -> Error(DeserError.unexpected obj "string" path)
            | Enum _, _ -> Error(DeserError.unexpected obj "string" path)
            | List _, _ -> Error(DeserError.unexpected obj "list" path)
            | TypeInfo'.Array _, _ -> Error(DeserError.unexpected obj "list" path)
            | Record _, _ -> Error(DeserError.unexpected obj "object" path)
            | Union _, _ -> Error(DeserError.unexpected obj "object" path)
            | Serializable, _ -> Error(DeserError.unexpected obj "serializable" path)
        with exc ->
            Error(DeserError.exc exc.Message path)

    let deserializeUnion
        ((info, _attrs): (Case * Action) array * Attribute array)
        (name: string)
        (obj: JsonValue)
        : Result<obj option, DeserError> =
        match info |> Array.tryFind (snd >> _.Name >> (=) name) with
        | Some(x, _) ->
            let fieldMap =
                x.attrs
                |> Array.collect (function
                    | :? CaseAttr as x -> Array.ofList x.MapFieldNames
                    | _ -> [||])
                |> Map.ofArray

            let res =
                readProps
                    fieldMap
                    []
                    x.props
                    (match obj with
                     | JsonValue.Record x -> x
                     | _ -> [||])

            Result.map (fun res -> Some(FSharpValue.MakeUnion(x.info, res))) res
        | None -> Ok None

type Context = { message: string; silent: bool }
type ActionsRegister = { actions: Action list }
type ActionsUnregister = { action_names: string list }

type ActionsForce =
    { state: string option
      query: string
      ephemeral_context: bool
      action_names: string list }

type ActionResult =
    { id: string
      success: bool
      message: string option }

[<TagName "command">]
type ClientCommand =
    | Startup of game: string
    | Context of game: string * data: Context
    | Actions_Register of game: string * data: ActionsRegister
    | Actions_Unregister of game: string * data: ActionsUnregister
    | Actions_Force of game: string * data: ActionsForce
    | Action_Result of game: string * data: ActionResult
//  | Shutdown_Ready of game: string

type ActionData =
    { id: string
      name: string
      data: string option }

[<TagName "command">]
type ServerCommand =
    //| Actions_ReregisterAll of data: ActionsReregisterAll
    //| Shutdown_Graceful of data: ShutdownGraceful
    //| Shutdown_Immediate of data: ShutdownImmediate
    | Action of data: ActionData

type internal Message =
    | MsgCmd of ClientCommand
    | MsgRetain of actions: Action list

type internal ActionInfo =
    { name: string
      description: string
      schema: JsonValue }

[<AbstractClass>]
type Game<'T>() =
    // websockets can only handle one send and one receive at a time
    // so we have to wrap sends in a mailbox
    let mutable mp: MailboxProcessor<Message> option = None

    [<VolatileFieldAttribute>]
    let mutable ws: ClientWebSocket = new ClientWebSocket()

    let acts =
        match snd (TypeInfo.fromSystemType typeof<'T>) with
        | Union(a, b) ->
            a
            |> Array.map (fun x ->
                // TODO: error message instead of random exception
                let act =
                    x.attrs
                    |> Array.pick (fun x ->
                        match x with
                        | :? Action as act -> Some act
                        | _ -> None)

                act.InitialSchema <- TypeInfo.unionSchema x
                act.Schema <- TypeInfo.unionSchema x
                x, act),
            b
        | _ -> raise (Exception "invalid actions type, must be a union")

    let findActByName =
        if fst acts |> Array.exists (snd >> _.MutableName) then
            fun s -> fst acts |> Array.find (snd >> _.Name >> (=) s) |> snd
        else
            let actMap = fst acts |> Array.map (fun (_, act) -> act.Name, act) |> Map.ofArray
            fun s -> Map.find s actMap

    let tagMap =
        fst acts |> Array.map (fun (case, act) -> case.info.Tag, act) |> Map.ofArray

    let mutable ctorMap = Map.empty

    let tagReader = FSharpValue.PreComputeUnionTagReader typeof<'T>

    let resolveAction (x: obj) : Action =
        match x with
        | :? string as s -> findActByName s
        | :? Action as act -> act
        | :? 'T as x -> Map.find (tagReader x) tagMap
        | x ->
            match Map.tryFind (x.GetHashCode()) ctorMap with
            | Some x -> x
            | None ->
                let ty = x.GetType()
                let invoke = ty.GetMethod "Invoke"
                let argType, _ = FSharpType.GetFunctionElements ty

                let args =
                    if FSharpType.IsTuple argType then
                        let variantTypes = FSharpType.GetTupleElements argType
                        FSharpValue.MakeTuple(variantTypes |> Array.map (fun _ -> null), argType)
                    else
                        null

                let instance = invoke.Invoke(x, [| args |])
                let ret = Map.find (tagReader instance) tagMap
                ctorMap <- ctorMap.Add(x.GetHashCode(), ret)
                ret

    abstract Name: string
    abstract member HandleAction: 'T -> Result<string option, string option>
    abstract member ReregisterActions: unit -> unit
    abstract member LogError: string -> unit
    abstract member LogDebug: string -> unit
    default _.ReregisterActions() = ()
    default _.LogError _ = ()
    default _.LogDebug _ = ()

    member _.Send(cmd: ClientCommand) = mp.Value.Post(MsgCmd cmd)

    member this.Context (silent: bool) (message: string) =
        this.Send(Context(this.Name, { message = message; silent = silent }))

    member this.Force(data: ActionsForce) =
        this.Send(Actions_Force(this.Name, data))

    member _.Serialize<'Y>(what: 'Y) : string =
        let ty = TypeInfo.fromSystemType typeof<'Y>
        (TypeInfo.serialize ty what).ToString JsonSaveOptions.DisableFormatting

    member _.Action(action: obj) = resolveAction action

    member _.RetainActions(actions: obj list) =
        let acts = actions |> List.map resolveAction
        mp.Value.Post(MsgRetain acts)

    member this.RegisterActions(actions: obj list) =
        let acts = actions |> List.map resolveAction
        this.Send(Actions_Register(this.Name, { actions = acts }))

    member this.UnregisterActions(actions: obj list) =
        let acts = actions |> List.map resolveAction |> List.map _.Name
        this.Send(Actions_Unregister(this.Name, { action_names = acts }))

    member this.Start(url: string option, ct: CancellationToken) : Task =
        let url =
            match url with
            | Some url -> url
            | None -> System.Environment.GetEnvironmentVariable "NEURO_SDK_WS_URL"

        if url = null then
            raise (Exception "Please set NEURO_SDK_WS_URL")

        let serverCmdTy = TypeInfo.fromSystemType typeof<ServerCommand>
        let gameName = this.Name

        let log (err: string) = this.LogError err

        let proc (cmd: ServerCommand) =
            match cmd with
            | Action data ->
                let { id = id; name = name; data = data } = data

                let data =
                    match data with
                    | Some x when x.Length > 0 ->
                        try
                            Ok(JsonValue.Parse x)
                        with exc ->
                            Error(Some exc.Message)
                    | _ -> Ok JsonValue.Null

                let res =
                    data
                    |> Result.bind (fun data ->
                        try
                            let action = TypeInfo.deserializeUnion acts name data

                            match action with
                            | Ok(Some action) -> this.HandleAction(action :?> 'T)
                            | Ok None ->
                                let names = acts |> fst |> Seq.map (snd >> _.Name) |> String.concat "\", \""

                                Error(Some $"Unknown action name: {name}, expected one of: \"{names}\"")
                            | Error err -> Error(Some $"Invalid action data: {err}")
                        with exc ->
                            log $"{exc}"
                            Error(Some $"Unhandled exception: {exc.Message}"))

                mp.Value.Post(
                    MsgCmd(
                        Action_Result(
                            gameName,
                            { id = id
                              success = Result.isOk res
                              message = Result.defaultWith (fun x -> x) res }
                        )
                    )
                )

        task {
            let clientCmdTy = TypeInfo.fromSystemType typeof<ClientCommand>

            mp <-
                Some(
                    MailboxProcessor.StartImmediate(fun mp ->
                        let mutable registered = Map.empty

                        let rec mapCommand msg =
                            match msg with
                            | MsgRetain actions ->
                                let new_reg = actions |> List.filter _.Valid |> List.map _.Name |> Set.ofList

                                let unreg =
                                    Map.keys registered |> Seq.filter (new_reg.Contains >> not) |> List.ofSeq

                                // map it again for sanitization
                                mapCommand (MsgCmd(Actions_Unregister(gameName, { action_names = unreg })))
                                @ mapCommand (MsgCmd(Actions_Register(gameName, { actions = actions })))
                            | MsgCmd(Startup game) ->
                                this.LogDebug "resetting registered actions"
                                registered <- Map.empty
                                [ (Startup game) ]
                            | MsgCmd(Actions_Register(game, { actions = actions })) ->
                                let mutable unregister = List.empty

                                let actions' =
                                    actions
                                    |> List.filter (fun x ->
                                        if not x.Valid then
                                            if registered.ContainsKey x.Name then
                                                unregister <- x.Name :: unregister
                                                this.LogDebug $"unregistering {x.Name} (invalid)"
                                                registered <- registered.Remove x.Name

                                            false
                                        else
                                            match Map.tryFind x.Name registered with
                                            | Some y when x.EqualTo y -> false
                                            | Some y ->
                                                registered <- Map.add x.Name (x.Clone()) registered
                                                unregister <- y.Name :: unregister
                                                this.LogDebug $"reregistering {x.Name}"
                                                true
                                            | None ->
                                                registered <- Map.add x.Name (x.Clone()) registered
                                                this.LogDebug $"registering {x.Name}"
                                                true)

                                if actions'.IsEmpty then
                                    if unregister.IsEmpty then
                                        []
                                    else
                                        [ Actions_Unregister(game, { action_names = unregister }) ]
                                else
                                    let act = Actions_Register(game, { actions = actions' })

                                    if unregister.IsEmpty then
                                        [ act ]
                                    else
                                        [ Actions_Unregister(game, { action_names = unregister }); act ]
                            | MsgCmd(Actions_Unregister(game, { action_names = actions })) ->
                                let actions' =
                                    actions
                                    |> List.filter (fun name ->
                                        if registered.ContainsKey name then
                                            this.LogDebug $"unregistering {name}"
                                            registered <- registered.Remove name
                                            true
                                        else
                                            false)

                                if actions'.IsEmpty then
                                    []
                                else
                                    [ Actions_Unregister(game, { action_names = actions }) ]
                            | MsgCmd msg -> [ msg ]

                        async {
                            let! ct = Async.CancellationToken

                            while true do
                                let! msg = mp.Receive()

                                do!
                                    mapCommand msg
                                    |> List.map (fun msg ->
                                        async {
                                            try
                                                let data = TypeInfo.serialize clientCmdTy msg
                                                let text = data.ToString JsonSaveOptions.DisableFormatting
                                                let buffer = Encoding.UTF8.GetBytes text

                                                this.LogDebug $"sending {text}"

                                                do!
                                                    Async.AwaitTask(
                                                        ws.SendAsync(
                                                            ArraySegment<byte>(buffer, 0, buffer.Length),
                                                            WebSocketMessageType.Text,
                                                            true,
                                                            ct
                                                        )
                                                    )
                                            with exc ->
                                                log $"Send error: {exc}"
                                        })
                                    |> Async.Sequential
                                    |> Async.Ignore
                        })
                )

            while true do
                try
                    let ws1 = new ClientWebSocket()
                    ws <- ws1
                    this.LogDebug $"Connecting to {url}"
                    do! ws1.ConnectAsync(Uri url, ct)
                    this.LogDebug $"Connected to {url}"

                    let read () =
                        task {
                            let buf = ArraySegment<byte>(Array.zeroCreate 8192)

                            let ms = new MemoryStream()
                            let! res1 = ws1.ReceiveAsync(buf, ct)
                            let mutable res = res1
                            ms.Write(buf.Array, buf.Offset, res.Count)

                            while not res.EndOfMessage do
                                let! res1 = ws1.ReceiveAsync(buf, ct)
                                res <- res1
                                ms.Write(buf.Array, buf.Offset, res.Count)

                            ms.Seek(0, SeekOrigin.Begin) |> ignore
                            let reader = new StreamReader(ms, Encoding.UTF8)
                            let ret = reader.ReadToEnd()
                            reader.Dispose()
                            ms.Dispose()
                            let x = ret, res
                            return x
                        }

                    let receive =
                        task {
                            while ws1.State = WebSocketState.Open do
                                try
                                    let! text, res = read ()
                                    this.LogDebug $"received {text}"

                                    match res.MessageType with
                                    | WebSocketMessageType.Close ->
                                        do! ws1.CloseAsync(WebSocketCloseStatus.NormalClosure, "", ct)
                                    | WebSocketMessageType.Text
                                    | WebSocketMessageType.Binary ->
                                        let data = JsonValue.Parse text
                                        let cmd = TypeInfo.deserialize [] serverCmdTy data

                                        match cmd with
                                        | Ok data -> proc (data :?> ServerCommand)
                                        | Error err -> log $"Invalid server command: {err}"
                                    | _ -> ()
                                with
                                | :? WebSocketException as exc ->
                                    log $"Websocket exception: {exc}"
                                    do! ws1.CloseAsync(WebSocketCloseStatus.InternalServerError, "", ct)
                                | exc -> log $"Unhandled receive exception: {exc}"
                        }

                    mp.Value.Post(MsgCmd(Startup gameName))

                    try
                        this.ReregisterActions()
                    with exc ->
                        log $"Reregister failed: {exc}"

                    do! receive
                with exc ->
                    log $"Connection failed: {exc}"

                do! Task.Delay(3000, ct)
        }
