namespace HollowNeuro

open System
open System.Collections
open System.Reflection
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open BepInEx
open BepInEx.Logging
open HarmonyLib
open NeuroFSharp

type Actions =
    | [<Action("map", "Show a list of points of interest, in the same area or globally.")>] ShowMap of local: bool
    | [<Action("set_waypoint", "Store the current position as a waypoint on the map")>] SetWaypoint of name: string
    | [<Action("delete_waypoint", "Delete a named waypoint")>] DeleteWaypoint of name: string
    | [<Action("shoot", "Shoot a target")>] ShootTarget of autoshoot: bool

type Dir =
    // circle (atan2) start at E and goes towards N
    | E = 0
    | NE = 1
    | N = 2
    | NW = 3
    | W = 4
    | SW = 5
    | S = 6
    | SE = 7

type Waypoint =
    { name: string
      distanceMeters: int
      asimuth: int
      direction: Dir }

type CurrentAreaMap =
    { currentAreaName: string
      [<SkipSerializingIfEquals true>]
      currentAreaMapped: bool
      [<SkipSerializingIfNone>]
      pointsOfInterest: Waypoint list option }

type Area =
    { areaName: string
      pointsOfInterest: Waypoint list }

type WorldMap =
    { currentAreaName: string
      [<SkipSerializingIfNone>]
      mappedAreas: Area list }

module Context =
    let cnst x _ = x
    let mkPin (filter: UnityEngine.GameObject -> bool) (name: UnityEngine.GameObject -> string) = filter, name
    let getComp<'A> (o: UnityEngine.GameObject) = o.GetComponent<'A>()

    let grubPin =
        mkPin
            (getComp<GrubPin>
             >> fun d ->
                 PlayerData.instance.hasPinGrub
                 && not (PlayerData.instance.scenesGrubRescued.Contains d.name))
            (cnst "Grub")

    let stripHtml (x: string) =
        Regex(" +").Replace(Regex(@"<[^>]*>").Replace(x, " "), " ")

    let pinMap =
        let nextArea =
            mkPin
                (getComp<MapNextAreaDisplay>
                 >> fun d -> d.visitedString = "" || PlayerData.instance.GetBool d.visitedString)
                (cnst "Next area")

        let text = mkPin (cnst true) (getComp<TMPro.TMP_Text> >> _.text >> stripHtml)

        let textTrim =
            mkPin
                (cnst true)
                (getComp<TMPro.TMP_Text>
                 >> _.text
                 >> stripHtml
                 >> _.Trim()
                 >> _.Replace("\n", ""))

        let genericPin =
            mkPin (
                getComp<PlayMakerFSM>
                >> fun fsm ->
                    let killed =
                        fsm.FsmVariables.StringVariables
                        |> Array.tryFind (_.Name >> (=) "Completed Bool")

                    let has =
                        fsm.FsmVariables.StringVariables
                        |> Array.tryFind (_.Name >> (=) "Pin Type Bool")

                    let spec =
                        fsm.FsmVariables.StringVariables
                        |> Array.tryFind (_.Name >> (=) "Specific Bool")

                    let has2 =
                        fsm.FsmVariables.StringVariables
                        |> Array.tryFind (_.Name >> (=) "PlayerData Bool")

                    not (has |> Option.exists (_.Value >> PlayerData.instance.GetBool >> not))
                    && not (has2 |> Option.exists (_.Value >> PlayerData.instance.GetBool >> not))
                    && not (killed |> Option.exists (_.Value >> PlayerData.instance.GetBool))
                    && not (
                        spec
                        |> Option.exists (_.Value >> (fun x -> x = "" || PlayerData.instance.GetBool x) >> not)
                    )
            )

        Map.empty
        |> Map.add "Area Name (3)" text
        |> Map.add "Sub Area Name" text
        |> Map.add "Sub Area Name (1)" text
        |> Map.add "Sub Area Name (2)" text
        |> Map.add "Sub Area Name (3)" text
        |> Map.add "Sub Area Name (4)" textTrim
        |> Map.add "Sub Area Name - Love Tower" text
        |> Map.add "Next Area" nextArea
        |> Map.add "Next Area (1)" nextArea
        |> Map.add "Next Area (2)" nextArea
        |> Map.add "Next Area (3)" nextArea
        |> Map.add "Next Area (4)" nextArea
        |> Map.add "Next Area (5)" nextArea
        |> Map.add "Pin_Backer Ghost" (genericPin (cnst "Grave"))
        |> Map.add
            "Pin_Black_Egg"
            (mkPin (fun _ -> PlayerData.instance.hasPinBlackEgg) (cnst "Temple of the Black Egg"))
        |> Map.add "pin_banker" (genericPin (cnst "Millibelle the Banker"))
        |> Map.add "pin_bench" (genericPin (cnst "Bench"))
        |> Map.add
            "pin_blue_health"
            (mkPin
                (fun o ->
                    PlayerData.instance.hasPinCocoon
                    && PlayerData.instance.scenesEncounteredCocoon.Contains o.transform.parent.name)
                (cnst "Cocoon"))
        |> Map.add "pin_charm_slug" (genericPin (cnst "Charm Lover Salubra"))
        |> Map.add "pin_colosseum" (genericPin (cnst "Colosseum of Fools"))
        |> Map.add "pin_dream moth" (genericPin (cnst "Seer"))
        |> Map.add
            "pin_dream_tree"
            (mkPin
                (fun o ->
                    PlayerData.instance.hasPinDreamPlant
                    && PlayerData.instance.scenesEncounteredDreamPlant.Contains o.transform.parent.name
                    && not (PlayerData.instance.scenesEncounteredDreamPlantC.Contains o.transform.parent.name))
                (cnst "Whispering Root"))
        |> Map.add "pin_grub_king" (genericPin (cnst "Grubfather"))
        |> Map.add "pin_hunter" (genericPin (cnst "The Hunter"))
        |> Map.add
            "pin_jiji"
            (genericPin (fun _ ->
                if PlayerData.instance.permadeathMode > 0 then
                    "Steel Soul Jinn"
                else
                    "Confessor Jiji"))
        |> Map.add "pin_leg eater" (genericPin (cnst "Leg Eater"))
        |> Map.add "pin_mapper" (genericPin (cnst "Iselda"))
        |> Map.add "pin_nailsmith" (genericPin (cnst "Nailsmith"))
        |> Map.add "pin_relic_dealer" (genericPin (cnst "Relic Seeker Lemm"))
        |> Map.add "pin_sly" (genericPin (cnst "Sly"))
        |> Map.add "pin_sly (1)" (genericPin (cnst "Godseeker"))
        |> Map.add "pin_spa" (genericPin (cnst "Hot Spring"))
        |> Map.add "pin_stag_station" (genericPin (cnst "Stag Station"))
        |> Map.add "pin_stag_station (7)" (genericPin (cnst "Stag Station"))
        |> Map.add "pin_tram" (genericPin (cnst "Tram"))
    // |> Map.add "pop_up_backboard (1)" "pop_up_backboard (1)"

    let map () =
        GameManager.instance.gameMap.GetComponent<GameMap>()

    let checkMap () =
        let pd = PlayerData.instance

        if pd.hasMap then
            Ok()
        else if pd.openedMapperShop then
            Error(Some "The player has to purchase a map before map functionality becomes available!")
        else
            Error(Some "The player has to progress further before map functionality becomes available!")
        |> Result.bind (fun () ->
            match GameManager.instance.GetCurrentMapZone() with
            | "DREAM_WORLD"
            | "WHITE_PALACE"
            | "GODS_GLORY" -> Error(Some "You are in an uncharted territory!")
            | _ -> Ok())

    let mappedAreas () =
        let pd = PlayerData.instance
        let map = map ()

        [| map.areaDirtmouth, pd.mapDirtmouth, "DIRTMOUTH"
           map.areaCrossroads, pd.mapCrossroads, "CROSSROADS"
           map.areaGreenpath, pd.mapGreenpath, "GREEN_PATH"
           map.areaFogCanyon, pd.mapFogCanyon, "FOG_CANYON"
           map.areaQueensGardens, pd.mapRoyalGardens, "ROYAL_GARDENS"
           map.areaFungalWastes, pd.mapFungalWastes, "WASTES"
           map.areaCity, pd.mapCity, "CITY"
           map.areaWaterways, pd.mapWaterways, "WATERWAYS"
           map.areaCrystalPeak, pd.mapMines, "MINES"
           map.areaDeepnest, pd.mapDeepnest, "DEEPNEST"
           map.areaCliffs, pd.mapCliffs, "CLIFFS"
           map.areaKingdomsEdge, pd.mapOutskirts, "OUTSKIRTS"
           map.areaRestingGrounds, pd.mapRestingGrounds, "RESTING_GROUNDS"
           map.areaAncientBasin, pd.mapAbyss, "ABYSS" |]
        |> Array.filter (fun (x, mapped, y) -> mapped)
        |> Array.map (fun (x, _, y) -> x, y)

    let zoneScene () =
        let map = map ()

        if map.inRoom then
            map.doorMapZone, map.doorScene
        else
            GameManager.instance.GetCurrentMapZone(), GameManager.instance.GetSceneNameString()

    let mapArea () =
        let map = map ()
        let pd = PlayerData.instance

        let zone, scene = zoneScene ()

        match zone with
        | "ABYSS" -> map.areaAncientBasin, pd.mapAbyss
        | "CITY"
        | "KINGS_STATION"
        | "SOUL_SOCIETY"
        | "LURIENS_TOWER" -> map.areaCity, pd.mapCity
        | "CLIFFS" -> map.areaCliffs, pd.mapCliffs
        | "CROSSROADS"
        | "SHAMAN_TEMPLE" -> map.areaCrossroads, pd.mapCrossroads
        | "MINES" -> map.areaCrystalPeak, pd.mapMines
        | "DEEPNEST"
        | "BEASTS_DEN" -> map.areaDeepnest, pd.mapDeepnest
        | "FOG_CANYON"
        | "MONOMON_ARCHIVE" -> map.areaFogCanyon, pd.mapFogCanyon
        | "WASTES"
        | "QUEENS_STATION" -> map.areaFungalWastes, pd.mapFungalWastes
        | "GREEN_PATH" -> map.areaGreenpath, pd.mapGreenpath
        | "OUTSKIRTS"
        | "HIVE"
        | "COLOSSEUM" -> map.areaKingdomsEdge, pd.mapOutskirts
        | "ROYAL_GARDENS" -> map.areaQueensGardens, pd.mapRoyalGardens
        | "RESTING_GROUNDS" -> map.areaRestingGrounds, pd.mapRestingGrounds
        | "TOWN"
        | "KINGS_PASS" -> map.areaDirtmouth, pd.mapDirtmouth
        | "WATERWAYS"
        | "GODSEEKER_WASTE" -> map.areaWaterways, pd.mapWaterways
        | _ -> null, false

    let playerPos (curArea: UnityEngine.GameObject) (curScene: UnityEngine.GameObject) =
        let map = map ()
        let hero = HeroController.instance

        let tx, ty, ox, oy, sw, sh =
            if map.inRoom then
                map.doorX,
                map.doorY,
                map.doorOriginOffsetX,
                map.doorOriginOffsetY,
                map.doorSceneWidth,
                map.doorSceneHeight
            else
                hero.transform.position.x,
                hero.transform.position.y,
                typeof<GameMap>.GetField("originOffsetX", BindingFlags.Instance ||| BindingFlags.NonPublic).GetValue map
                :?> float32,
                typeof<GameMap>.GetField("originOffsetY", BindingFlags.Instance ||| BindingFlags.NonPublic).GetValue map
                :?> float32,
                typeof<GameMap>.GetField("sceneWidth", BindingFlags.Instance ||| BindingFlags.NonPublic).GetValue map
                :?> float32,
                typeof<GameMap>.GetField("sceneHeight", BindingFlags.Instance ||| BindingFlags.NonPublic).GetValue map
                :?> float32

        let sceneSpriteSize =
            curScene.GetComponent<UnityEngine.SpriteRenderer>().sprite.bounds.size

        let px =
            curArea.transform.localPosition.x
            + curScene.transform.localPosition.x
            + ((tx + ox) / sw - 0.5f) * sceneSpriteSize.x

        let py =
            curArea.transform.localPosition.y
            + curScene.transform.localPosition.y
            + ((ty + oy) / sh - 0.5f) * sceneSpriteSize.y

        // for better printing by bepinex
        sprintf
            "calculated player pos: %f/%f from area %f/%f scene %f/%f hero %f/%f origin %f/%f scene %f/%f sprite size %f/%f\nactual compass pos: %f/%f"
            px
            py
            curArea.transform.localPosition.x
            curArea.transform.localPosition.y
            curScene.transform.localPosition.x
            curScene.transform.localPosition.y
            tx
            ty
            ox
            oy
            sw
            sh
            sceneSpriteSize.x
            sceneSpriteSize.y
            map.compassIcon.transform.localPosition.x
            map.compassIcon.transform.localPosition.y
        |> printf "%s"

        px, py

    let waypoint
        (px, py)
        (area: UnityEngine.GameObject)
        (scene: UnityEngine.GameObject)
        name
        (object: UnityEngine.GameObject)
        =
        let x =
            area.transform.localPosition.x
            + scene.transform.localPosition.x
            + object.transform.localPosition.x

        let y =
            area.transform.localPosition.y
            + scene.transform.localPosition.y
            + object.transform.localPosition.y

        let dx = x - px
        let dy = y - py
        let dist = sqrt (dx * dx + dy * dy)

        let angle = atan2 dy dx

        sprintf
            "calculated pin pos %f/%f distance %f/%f angle %f from area %f/%f scene %f/%f pin %f/%f"
            x
            y
            dx
            dy
            (angle * float32 (180. / Math.PI))
            area.transform.localPosition.x
            area.transform.localPosition.y
            scene.transform.localPosition.x
            scene.transform.localPosition.x
            object.transform.localPosition.y
            object.transform.localPosition.y
        |> printf "%s"


        { name = name
          distanceMeters = int (dist * 10.0f)
          // start with e-n-w-s, negate to e-s-w-n, shift by 90 to n-e-s-w
          asimuth = int (round (450.0f - angle * float32 (180. / Math.PI))) % 360
          direction =
            // separate circle into 8 sectors
            // 7.5-0.5 = E
            // 0.5-1.5 = NE
            // ...
            // 6.5-7.5 = SE
            // (add 8.5 to (8) account for atan2 returning negative values and (0.5) round)
            // (could add 8 and use round instead but thats one more function call wow so slow)
            enum<Dir> ((int (angle * float32 (4. / Math.PI) + 8.5f)) % 8) }

    let pointsOfInterest playerPos (area: UnityEngine.GameObject) =
        let pd = PlayerData.instance

        // printfn "scenes mapped {]}"
        Seq.init area.transform.childCount (area.transform.GetChild >> _.gameObject)
        |> if pd.mapAllRooms then
               id
           else
               Seq.filter (fun scene -> scene.name = "Grub Pins" || pd.scenesMapped.Contains scene.name)
        |> Seq.collect (fun scene ->
            Seq.init scene.transform.childCount (scene.transform.GetChild >> _.gameObject)
            |> Seq.collect (fun pin ->
                if scene.name = "Grub Pins" then
                    Some grubPin
                else
                    Map.tryFind pin.name pinMap |> Option.orElseWith (fun () -> None)
                |> Option.bind (fun (allow, name) ->
                    if allow pin then
                        Some(waypoint playerPos area scene (name pin) pin)
                    else
                        None)
                |> Option.toList))
        |> List.ofSeq

    let currentArea () =
        let zone, sceneName = zoneScene ()
        let area, mapped = mapArea ()

        let name =
            match Language.Language.Get(zone, "Map Zones") with
            | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
            | x -> stripHtml x

        let scene =
            Seq.init area.transform.childCount (area.transform.GetChild >> _.gameObject)
            |> Seq.map (fun x ->
                printfn "%s %s" x.name sceneName
                x)
            |> Seq.find (_.name >> (=) sceneName)

        { currentAreaName = name
          currentAreaMapped = mapped
          pointsOfInterest =
            if mapped then
                Some(pointsOfInterest (playerPos area scene) area)
            else
                None }

    let allAreas () =
        let zone, sceneName = zoneScene ()
        let area, mapped = mapArea ()

        let name =
            match Language.Language.Get(zone, "Map Zones") with
            | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
            | x -> stripHtml x

        let scene =
            Seq.init area.transform.childCount (area.transform.GetChild >> _.gameObject)
            |> Seq.find (_.name >> (=) sceneName)

        let pos = playerPos area scene

        { currentAreaName = name
          mappedAreas =
            mappedAreas ()
            |> Array.map (fun (area, zone) ->
                let name =
                    match Language.Language.Get(zone, "Map Zones") with
                    | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
                    | x -> stripHtml x

                { areaName = name
                  pointsOfInterest = pointsOfInterest pos area })
            |> Array.toList }


module Native =
    [<DllImport "libprofiler.so">]
    extern void profiler_reset()

    [<DllImport "libprofiler.so">]
    extern void profiler_save()

type Game(plugin: MainClass) =
    inherit Game<Actions>()

    override this.ReregisterActions() =
        this.RegisterActions [ ShowMap; SetWaypoint; DeleteWaypoint; ShootTarget ]

    override _.Name = "Hollow Knight"

    override this.HandleAction(action: Actions) =

        match action with
        | ShowMap local ->
            let pd = PlayerData.instance
            let m = GameManager.instance.gameMap.GetComponent<GameMap>()
            let gm = GameManager.instance
            let zone = gm.GetCurrentMapZone()

            Context.checkMap ()
            |> Result.bind (fun () ->
                if local then
                    let ctx = Context.currentArea ()
                    Ok(Some(this.Serialize ctx))
                else
                    let ctx = Context.allAreas ()
                    Ok(Some(this.Serialize ctx)))

        // Array.init m.transform.childCount m.transform.GetChild
        // |> Array.iter (fun x ->
        //     Array.init x.gameObject.transform.childCount x.gameObject.transform.GetChild
        //     |> Array.iter (fun y ->
        //         if pd.scenesMapped.Contains y.gameObject.name || true then
        //             plugin.Logger.LogInfo $"scene {y.gameObject.name}"

        //             Array.init y.gameObject.transform.childCount y.gameObject.transform.GetChild
        //             |> Array.iter (fun z ->
        //                 plugin.Logger.LogInfo $"- {z.gameObject.name}"

        //                 if y.gameObject.name = "Grub Pins" then
        //                     let w1, w2 = Context.grubPin
        //                     plugin.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"
        //                 else
        //                     Context.pinMap
        //                     |> Map.tryFind z.gameObject.name
        //                     |> Option.iter (fun (w1, w2) ->
        //                         plugin.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"))
        //         else
        //             plugin.Logger.LogInfo $"skipping scene {y.gameObject.name}"))
        // todo
        | SetWaypoint name ->
            // todo
            Context.checkMap () |> Result.map (fun () -> None)
        | DeleteWaypoint name ->
            // todo
            Context.checkMap () |> Result.map (fun () -> None)
        | ShootTarget name ->
            // todo
            Ok None

    override _.LogError error =
        let fff = "fff"
        plugin.Logger.LogError $"{DateTime.UtcNow}.{DateTime.UtcNow.ToString fff} {error}"

    override _.LogDebug error =
        let fff = "fff"
        plugin.Logger.LogInfo $"{DateTime.UtcNow}.{DateTime.UtcNow.ToString fff} {error}"

    member this.Update() =
        try
            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F1 then
                Native.profiler_reset ()

            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F2 then
                Native.profiler_save ()

            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F10 then
                let json = UnityEngine.JsonUtility.ToJson(HeroController.instance.playerData, true)
                System.IO.File.WriteAllText("hero.json", json)

            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F5 then
                UnityEngine.JsonUtility.FromJsonOverwrite(
                    System.IO.File.ReadAllText "hero.json",
                    HeroController.instance.playerData
                )
        with exc ->
            this.Context true $"Exception while handling player input: {exc}"

    member this.LateUpdate() =
        PlayerData.instance
        |> Option.ofObj
        |> Option.iter (fun pd ->
            pd.hasPinBench <- true
            pd.mapAllRooms <- true)

        HutongGames.PlayMaker.FsmLog.LoggingEnabled <- true

and [<BepInPlugin("org.chayleaf.hollowneur", "HollowNeuro", "1.0.0")>] MainClass() =
    inherit BaseUnityPlugin()
    let mutable harmony = null
    let mutable game = None
    let cts = new Threading.CancellationTokenSource()

    [<DefaultValue>]
    val mutable public Logger: ManualLogSource

    [<DefaultValue>]
    static val mutable private instance: MainClass

    static member Instance = MainClass.instance
    member _.Game = game.Value

    member this.Awake() =
        System.IO.Directory.CreateDirectory "fsms" |> ignore
        let log = base.Logger

        try
            this.Logger <- base.Logger
            // HutongGames.PlayMaker.FsmLog.MirrorDebugLog <- true

            MainClass.instance <- this
            harmony <- Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly())
            let cnt = Seq.fold (fun x _ -> x + 1) 0 (harmony.GetPatchedMethods())

            typeof<CheatManager>.GetMethod("Init", BindingFlags.NonPublic ||| BindingFlags.Static).Invoke(null, [||])
            |> ignore
            // PerformanceHUD.Shared.enabled <- true

            game <-
                Some(
                    let game = Game this
                    game.Start(None, cts.Token) |> ignore
                    game
                )

            this.Logger.LogInfo $"Plugin HollowNeuro is loaded with {cnt} patches!"
        with exc ->
            this.Logger.LogError $"ERROR {exc}"

    member _.LateUpdate() = game |> Option.iter _.LateUpdate()
    member _.Update() = game |> Option.iter _.Update()
