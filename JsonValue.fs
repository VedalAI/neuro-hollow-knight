// --------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation 2005-2012.
// This sample code is provided "as is" without warranty of any kind.
// We disclaim all warranties, either express or implied, including the
// warranties of merchantability and fitness for a particular purpose.
//
// A simple F# portable parser for JSON data
// --------------------------------------------------------------------------------------

// This is taken from FSharp.Data, that package includes HTTP requests and stuff who needs that anyway

namespace NeuroFSharp

open System
open System.IO
open System.ComponentModel
open System.Globalization
open System.Text

[<AutoOpen>]
module private Helpers =

    /// Convert the result of TryParse to option type
    let asOption =
        function
        | true, v -> Some v
        | _ -> None

type internal TextConversions private () =
    /// Turns empty or null string value into None, otherwise returns Some
    static member AsString str =
        if String.IsNullOrWhiteSpace str then None else Some str

    static member AsDecimal cultureInfo (text: string) =
        Decimal.TryParse(text, NumberStyles.Currency, cultureInfo) |> asOption

    /// if useNoneForMissingValues is true, NAs are returned as None, otherwise Some Double.NaN is used
    static member AsFloat cultureInfo (text: string) =
        Double.TryParse(text, NumberStyles.Any, cultureInfo) |> asOption

module internal UnicodeHelper =
    let getUnicodeSurrogatePair num =
        let codePoint = num - 0x010000u
        let HIGH_TEN_BIT_MASK = 0xFFC00u // 1111|1111|1100|0000|0000
        let LOW_TEN_BIT_MASK = 0x003FFu // 0000|0000|0011|1111|1111
        let leadSurrogate = (codePoint &&& HIGH_TEN_BIT_MASK >>> 10) + 0xD800u
        let trailSurrogate = (codePoint &&& LOW_TEN_BIT_MASK) + 0xDC00u
        char leadSurrogate, char trailSurrogate

/// Specifies the formatting behaviour of JSON values
[<RequireQualifiedAccess>]
type JsonSaveOptions =
    /// Format (indent) the JsonValue
    | None = 0

    /// Print the JsonValue in one line in a compact way
    | DisableFormatting = 1

    /// Print the JsonValue in one line in a compact way,
    /// but place a single space after every comma
    /// https://github.com/fsprojects/FSharp.Data/issues/1482
    | CompactSpaceAfterComma = 2

/// Represents a JSON value. Large numbers that do not fit in the
/// Decimal type are represented using the Float case, while
/// smaller numbers are represented as decimals to avoid precision loss.
[<RequireQualifiedAccess>]
[<StructuredFormatDisplay("{_Print}")>]
type JsonValue =
    | String of string
    | Number of decimal
    | Float of float
    | Record of properties: (string * JsonValue)[]
    | Array of elements: JsonValue[]
    | Boolean of bool
    | Null

    /// <exclude />
    [<EditorBrowsableAttribute(EditorBrowsableState.Never)>]
    [<CompilerMessageAttribute("This method is intended for use in generated code only.",
                               10001,
                               IsHidden = true,
                               IsError = false)>]
    member x._Print =
        let str = x.ToString()

        if str.Length > 512 then
            str.Substring(0, 509) + "..."
        else
            str

    /// Serializes the JsonValue to the specified System.IO.TextWriter.
    member x.WriteTo(w: TextWriter, saveOptions, ?indentationSpaces: int) =
        let indentationSpaces = defaultArg indentationSpaces 2

        let newLine =
            if saveOptions = JsonSaveOptions.None then
                fun indentation plus ->
                    w.WriteLine()
                    System.String(' ', indentation + plus) |> w.Write
            else
                fun _ _ -> ()

        let propSep = if saveOptions = JsonSaveOptions.None then "\": " else "\":"

        let comma () =
            match saveOptions with
            | JsonSaveOptions.None
            | JsonSaveOptions.DisableFormatting -> w.Write ","
            | JsonSaveOptions.CompactSpaceAfterComma -> w.Write ", "
            | _ -> failwith "Invalid JsonSaveOptions"

        let rec serialize indentation =
            function
            | Null -> w.Write "null"
            | Boolean b -> w.Write(if b then "true" else "false")
            | Number number -> w.Write number
            | Float v when Double.IsPositiveInfinity v -> w.Write "Inf"
            | Float v when Double.IsNegativeInfinity v -> w.Write "-Inf"
            | Float v when Double.IsNaN v -> w.Write "NaN"
            | Float number -> w.Write (number.ToString "0.######")
            | String s ->
                w.Write "\""
                JsonValue.JsonStringEncodeTo w s
                w.Write "\""
            | Record properties ->
                w.Write "{"

                for i = 0 to properties.Length - 1 do
                    let k, v = properties.[i]

                    if i > 0 then
                        comma ()

                    newLine indentation indentationSpaces
                    w.Write "\""
                    JsonValue.JsonStringEncodeTo w k
                    w.Write propSep
                    serialize (indentation + indentationSpaces) v

                newLine indentation 0
                w.Write "}"
            | Array elements ->
                w.Write "["

                for i = 0 to elements.Length - 1 do
                    if i > 0 then
                        comma ()

                    newLine indentation indentationSpaces
                    serialize (indentation + indentationSpaces) elements.[i]

                if elements.Length > 0 then
                    newLine indentation 0

                w.Write "]"

        serialize 0 x

    // Encode characters that are not valid in JS string. The implementation is based
    // on https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/HttpUtility.cs
    static member internal JsonStringEncodeTo (w: TextWriter) (value: string) =
        if not (String.IsNullOrEmpty value) then
            for i = 0 to value.Length - 1 do
                let c = value.[i]
                let ci = int c

                if ci >= 0 && ci <= 7 || ci = 11 || ci >= 14 && ci <= 31 then
                    w.Write("\\u{0:x4}", ci) |> ignore
                else
                    match c with
                    | '\b' -> w.Write "\\b"
                    | '\t' -> w.Write "\\t"
                    | '\n' -> w.Write "\\n"
                    | '\f' -> w.Write "\\f"
                    | '\r' -> w.Write "\\r"
                    | '"' -> w.Write "\\\""
                    | '\\' -> w.Write "\\\\"
                    | _ -> w.Write c

    member x.ToString(saveOptions, ?indentationSpaces: int) =
        let w = new StringWriter(CultureInfo.InvariantCulture)
        x.WriteTo(w, saveOptions, ?indentationSpaces = indentationSpaces)
        w.GetStringBuilder().ToString()

    member x.ToString(?indentationSpaces: int) =
        x.ToString(JsonSaveOptions.None, ?indentationSpaces = indentationSpaces)

    override x.ToString() = x.ToString JsonSaveOptions.None

// --------------------------------------------------------------------------------------
// JSON parser
// --------------------------------------------------------------------------------------

type private JsonParser(jsonText: string) =

    let mutable i = 0
    let s = jsonText

    let buf = StringBuilder() // pre-allocate buffers for strings

    // Helper functions

    let isNumChar c =
        Char.IsDigit c || c = '.' || c = 'e' || c = 'E' || c = '+' || c = '-'

    let throw (msg: string) =
        failwith $"Invalid JSON starting at character {i}: {msg}"

    let ensure cond msg =
        if not cond then
            throw msg


    let rec skipCommentsAndWhitespace () =
        let skipComment () =
            // Supported comment syntax:
            // - // ...{newLine}
            // - /* ... */
            if i < s.Length && s.[i] = '/' then
                i <- i + 1

                if i < s.Length && s.[i] = '/' then
                    i <- i + 1

                    while i < s.Length && s.[i] <> '\r' && s.[i] <> '\n' do
                        i <- i + 1
                else if i < s.Length && s.[i] = '*' then
                    i <- i + 1

                    while i + 1 < s.Length && s.[i] <> '*' && s.[i + 1] <> '/' do
                        i <- i + 1

                    ensure (i + 1 < s.Length && s.[i] = '*' && s.[i + 1] = '/') "invalid comment"
                    i <- i + 2

                true

            else
                false

        let skipWhitespace () =
            let initialI = i

            while i < s.Length && Char.IsWhiteSpace s.[i] do
                i <- i + 1

            initialI <> i // return true if some whitespace was skipped

        if skipWhitespace () || skipComment () then
            skipCommentsAndWhitespace ()

    // Recursive descent parser for JSON that uses global mutable index
    let rec parseValue cont =
        skipCommentsAndWhitespace ()
        ensure (i < s.Length) "unexpected end of input"

        match s.[i] with
        | '"' -> cont (JsonValue.String(parseString ()))
        | '-' -> cont (parseNum ())
        | c when Char.IsDigit c -> cont (parseNum ())
        | '{' -> parseObject cont
        | '[' -> parseArray cont
        | 't' -> cont (parseLiteral ("true", JsonValue.Boolean true))
        | 'f' -> cont (parseLiteral ("false", JsonValue.Boolean false))
        | 'n' -> cont (parseLiteral ("null", JsonValue.Null))
        | _ -> throw "invalid JSON value"

    and parseString () =
        ensure (i < s.Length && s.[i] = '"') "expected a string"
        i <- i + 1

        while i < s.Length && s.[i] <> '"' do
            if s.[i] = '\\' then
                ensure (i + 1 < s.Length) "unclosed string"

                match s.[i + 1] with
                | 'b' -> buf.Append '\b' |> ignore
                | 'f' -> buf.Append '\f' |> ignore
                | 'n' -> buf.Append '\n' |> ignore
                | 't' -> buf.Append '\t' |> ignore
                | 'r' -> buf.Append '\r' |> ignore
                | '\\' -> buf.Append '\\' |> ignore
                | '/' -> buf.Append '/' |> ignore
                | '"' -> buf.Append '"' |> ignore
                | 'u' ->
                    ensure (i + 5 < s.Length) "invalid escape code"

                    let hexdigit d =
                        if d >= '0' && d <= '9' then int32 d - int32 '0'
                        elif d >= 'a' && d <= 'f' then int32 d - int32 'a' + 10
                        elif d >= 'A' && d <= 'F' then int32 d - int32 'A' + 10
                        else failwith "hexdigit"

                    let unicodeChar (s: string) =
                        if s.Length <> 4 then
                            failwith "unicodeChar"

                        char (
                            hexdigit s.[0] * 4096
                            + hexdigit s.[1] * 256
                            + hexdigit s.[2] * 16
                            + hexdigit s.[3]
                        )

                    let ch = unicodeChar (s.Substring(i + 2, 4))
                    buf.Append ch |> ignore
                    i <- i + 4 // the \ and u will also be skipped past further below
                | 'U' ->
                    ensure (i + 9 < s.Length) "invalid escape code"

                    let unicodeChar (s: string) =
                        if s.Length <> 8 then
                            failwithf "unicodeChar (%O)" s

                        if s.[0..1] <> "00" then
                            failwithf "unicodeChar (%O)" s

                        UnicodeHelper.getUnicodeSurrogatePair
                        <| System.UInt32.Parse(s, NumberStyles.HexNumber)

                    let lead, trail = unicodeChar (s.Substring(i + 2, 8))
                    buf.Append lead |> ignore
                    buf.Append trail |> ignore
                    i <- i + 8 // the \ and u will also be skipped past further below
                | _ -> throw "invalid escape character"

                i <- i + 2 // skip past \ and next char
            else
                buf.Append(s.[i]) |> ignore
                i <- i + 1

        ensure (i < s.Length && s.[i] = '"') "unclosed string"
        i <- i + 1
        let str = buf.ToString()
        buf.Clear() |> ignore
        str

    and parseNum () =
        let start = i

        while i < s.Length && isNumChar s.[i] do
            i <- i + 1

        let len = i - start
        let sub = s.Substring(start, len)

        match TextConversions.AsDecimal CultureInfo.InvariantCulture sub with
        | Some x -> JsonValue.Number x
        | _ ->
            match TextConversions.AsFloat CultureInfo.InvariantCulture sub with
            | Some x -> JsonValue.Float x
            | _ -> throw "invalid number"

    and parsePair cont =
        let key = parseString ()
        skipCommentsAndWhitespace ()
        ensure (i < s.Length && s.[i] = ':') "missing colon after key"
        i <- i + 1
        skipCommentsAndWhitespace ()
        parseValue (fun v -> cont (key, v))

    and parseObject cont =
        ensure (i < s.Length && s.[i] = '{') "expected an object"
        i <- i + 1
        skipCommentsAndWhitespace ()
        let pairs = ResizeArray<_>()

        let parseObjectEnd () =
            ensure (i < s.Length && s.[i] = '}') "unclosed object"
            i <- i + 1
            let res = pairs.ToArray() |> JsonValue.Record
            cont res

        if i < s.Length && s.[i] = '"' then
            parsePair (fun p ->
                pairs.Add p
                skipCommentsAndWhitespace ()

                let rec parsePairItem () =
                    if i < s.Length && s.[i] = ',' then
                        i <- i + 1
                        skipCommentsAndWhitespace ()

                        parsePair (fun p ->
                            pairs.Add p
                            skipCommentsAndWhitespace ()
                            parsePairItem ())
                    else
                        parseObjectEnd ()

                parsePairItem ())
        else
            parseObjectEnd ()

    and parseArray cont =
        ensure (i < s.Length && s.[i] = '[') "expected an array"
        i <- i + 1
        skipCommentsAndWhitespace ()
        let vals = ResizeArray<_>()

        let parseArrayEnd () =
            ensure (i < s.Length && s.[i] = ']') "unclosed array"
            i <- i + 1
            let res = vals.ToArray() |> JsonValue.Array
            cont res

        if i < s.Length && s.[i] <> ']' then
            parseValue (fun v ->
                vals.Add v
                skipCommentsAndWhitespace ()

                let rec parseArrayItem () =
                    if i < s.Length && s.[i] = ',' then
                        i <- i + 1
                        skipCommentsAndWhitespace ()

                        parseValue (fun v ->
                            vals.Add v
                            skipCommentsAndWhitespace ()
                            parseArrayItem ())
                    else
                        parseArrayEnd ()

                parseArrayItem ())
        else
            parseArrayEnd ()

    and parseLiteral (expected, r) =
        ensure (i + expected.Length <= s.Length) $"expected `{expected}`"

        for j in 0 .. expected.Length - 1 do
            ensure (s.[i + j] = expected.[j]) $"expected `{expected}`"

        i <- i + expected.Length
        r

    // Start by parsing the top-level value
    member _.Parse() =
        let value = parseValue id
        skipCommentsAndWhitespace ()

        if i <> s.Length then
            throw "trailing characters found"

        value

    member _.ParseMultiple() =
        seq {
            while i <> s.Length do
                yield parseValue id
                skipCommentsAndWhitespace ()
        }

type JsonValue with

    /// Parses the specified JSON string
    static member Parse text = JsonParser(text).Parse()

    /// Parses the specified string into multiple JSON values
    static member ParseMultiple text = JsonParser(text).ParseMultiple()
