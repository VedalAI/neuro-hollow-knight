namespace HollowNeuro

open System
open System.Reflection
open System.Runtime.InteropServices
open System.Text.RegularExpressions
open BepInEx
open BepInEx.Logging
open HarmonyLib
open NeuroFSharp
open UnityEngine.SceneManagement

type Tactic =
    | ShootNearestFirst
    | ShootFurthestFirst
    | ShootMostHpFirst
    | ShootLeastHpFirst
    | DontShoot

type Actions =
    | [<Action("map",
               "Show a list of points of interest, in the current area or globally. You can then use the `pathfind_room` action to find the shortest path to the room that contains them.")>] ShowMap of
        local: bool
    //| [<Action("pathfind_area", "Find the shortest path to an area.")>] PathfindArea of name: string
    | [<Action("pathfind_room", "Pathfind to a room by its id.")>] PathfindRoom of roomId: int
    | [<Action("unexplored_exits", "Find a list of unexplored rooms, in the current area or globally.")>] UnexploredRooms of
        local: bool
    | [<Action("create_waypoint", "Store the current position as a waypoint on the map")>] CreateWaypoint of
        name: string
    | [<Action("delete_waypoint", "Delete a named waypoint")>] DeleteWaypoint of name: string
    | [<Action("shoot_player", "Shoot the player with a single shot. Can be repeated to fire multiple times.")>] ShootPlayer of
        dealDamage: bool
    | [<Action("set_tactic",
               "Set the enemy targeting tactic used for autofire, i.e. which enemies to prioritize when shooting.")>] SetTactic of
        tactic: Tactic

type PointOfInterest =
    { name: string
      [<SkipSerializingIfNone>]
      roomId: int option
      distanceMeters: int
      direction: Dir
      [<SkipSerializingIfEquals false>]
      userCreated: bool }

type CurrentAreaMap =
    { currentAreaName: string
      [<SkipSerializingIfEquals true>]
      currentAreaMapped: bool
      [<SkipSerializingIfNone>]
      pointsOfInterest: PointOfInterest list option }

type Area =
    { areaName: string
      pointsOfInterest: PointOfInterest list }

type WorldMap =
    { currentAreaName: string
      [<SkipSerializingIfNone>]
      mappedAreas: Area list }

type Entity =
    { name: string
      distance: float32
      [<SkipSerializingIfNone>]
      hp: int option
      [<SkipSerializingIfNone>]
      maxHp: int option
      inShootRange: bool
      [<SkipSerializingIfEquals false>]
      currentlyInvincible: bool }

module Context =
    let mutable logger: ManualLogSource = null
    let mutable dumpingLocal = true
    let cnst x _ = x
    let mkPin (filter: UnityEngine.GameObject -> bool) (name: UnityEngine.GameObject -> string) = filter, name
    let getComp<'A> (o: UnityEngine.GameObject) = o.GetComponent<'A>()
    let mapActions: obj list = [ ShowMap; CreateWaypoint; DeleteWaypoint; PathfindRoom ]

    let grubPin =
        mkPin
            (getComp<GrubPin>
             >> fun d ->
                 PlayerData.instance.hasPinGrub
                 && not (PlayerData.instance.scenesGrubRescued.Contains d.name))
            (cnst "Grub")

    let stripHtml (x: string) =
        Regex(" +").Replace(Regex(@"<[^>]*>").Replace(x.Replace("<br>", "\n"), " "), " ")

    let countBosses (pd: PlayerData) =
        // seems like one boss is missing? whatever don't care
        [| pd.killedInfectedKnight
           pd.killedMawlek
           pd.killedNailBros
           pd.killedJarCollector
           pd.killedMegaBeamMiner
           pd.killedDungDefender
           pd.killedFalseKnight
           pd.killedFlukeMother
           pd.killedLobsterLancer
           pd.killedNailsage
           pd.killedGrimm
           pd.killedBigFly
           pd.killedHiveKnight
           pd.killedHollowKnight
           pd.hornet1Defeated
           pd.hornetOutskirtsDefeated
           pd.killedMantisLord
           pd.killedMegaMossCharger
           pd.killedMimicSpider
           pd.killedOblobble
           pd.killedPaintmaster
           pd.killedFinalBoss
           pd.killedMageKnight
           pd.killedTraitorLord
           pd.killedMegaJellyfish
           pd.killedBigBuzzer
           pd.killedBlackKnight
           pd.killedZote
           pd.killedGhostHu
           pd.killedGhostGalien
           pd.killedGhostAladar
           pd.killedGhostMarkoth
           pd.killedGhostMarmu
           pd.killedGhostNoEyes
           pd.killedGhostXero
           pd.killedNightmareGrimm
           pd.mageLordDefeated
           pd.mageLordDreamDefeated
           pd.lurienDefeated
           pd.hegemolDefeated
           pd.monomonDefeated
           pd.infectedKnightDreamDefeated
           pd.falseKnightDreamDefeated
           pd.nailsmithKilled
           // comment to fix annoying formatting
           |]
        |> Array.map (fun x -> if x then 1 else 0)
        |> Array.sum

    let pinMap =
        let areaMap s =
            match s with
            | "visitedAbyss" -> "ABYSS"
            | "visitedCity" -> "CITY"
            | "visitedCliffs" -> "CLIFFS"
            | "visitedCrossroads" -> "CROSSROADS"
            | "visitedDeepnest" -> "DEEPNEST"
            | "visitedFogCanyon" -> "FOG_CANYON"
            | "visitedFungalWastes" -> "WASTES"
            | "visitedFungus" -> "FUNGUS"
            | "visitedGreenpath" -> "GREENPATH"
            | "visitedMines" -> "MINES"
            | "visitedMines10" -> "MINES10"
            | "visitedOutskirts" -> "OUTSKIRTS"
            | "visitedRestingGrounds" -> "RESTING_GROUNDS"
            | "visitedRoyalGardens" -> "ROYAL_GARDENS"
            | "visitedRuins" -> "RUINS"
            | "visitedWaterways" -> "WATERWAYS"
            | _ -> ""

        let nextArea =
            mkPin
                (getComp<MapNextAreaDisplay>
                 >> fun d ->
                     d.visitedString <> "" && not (PlayerData.instance.GetBool d.visitedString)
                     || dumpingLocal && areaMap d.visitedString <> "")
                (getComp<MapNextAreaDisplay>
                 >> fun d ->
                     if d.visitedString = "" then
                         "Area transition"
                     else if PlayerData.instance.GetBool d.visitedString then
                         let n = areaMap d.visitedString

                         if n = "" then
                             "Area transition"
                         else
                             $"Area transition: "
                             + match Language.Language.Get(n, "Map Zones") with
                               | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
                               | "#!#RUINS#!#" -> "Ruins"
                               | "#!#FUNGUS#!#" -> "Fungal Wastes (subarea)"
                               | "#!#MINES10#!#" -> "Crystal Peak (subarea)"
                               | x -> (stripHtml x).Replace("\n", " ").Replace("  ", " ").Replace("  ", " ")
                     else
                         "Unexplored area transition")

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

                    let defaultTo d (x: HutongGames.PlayMaker.FsmString option) =
                        let v = x |> Option.map _.Value |> Option.defaultValue ""
                        if v = "" then d else PlayerData.instance.GetBool v

                    defaultTo true has
                    && defaultTo true has2
                    && not (defaultTo false killed)
                    && defaultTo true spec
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

    let mappedAreas forceInclude =
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
        |> Array.filter (fun (x, mapped, y) -> mapped || forceInclude x y)

    let zoneScene () =
        let map = map ()

        if map.inRoom then
            map.doorMapZone, map.doorScene
        else
            GameManager.instance.GetCurrentMapZone(), GameManager.instance.GetSceneNameString()

    let areaFromZone zone =
        let map = map ()
        let pd = PlayerData.instance

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

    let mapArea = zoneScene >> fst >> areaFromZone

    let playerPos () =
        let _zone, sceneName = zoneScene ()
        let area, _mapped = mapArea ()

        let scene =
            Seq.init area.transform.childCount (area.transform.GetChild >> _.gameObject)
            |> Seq.find (_.name >> (=) sceneName)

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
            scene.GetComponent<UnityEngine.SpriteRenderer>().sprite.bounds.size

        let px =
            area.transform.localPosition.x
            + scene.transform.localPosition.x
            + ((tx + ox) / sw - 0.5f) * sceneSpriteSize.x

        let py =
            area.transform.localPosition.y
            + scene.transform.localPosition.y
            + ((ty + oy) / sh - 0.5f) * sceneSpriteSize.y

        // for better printing by bepinex
        sprintf
            "calculated player pos: %f/%f from area %f/%f scene %f/%f hero %f/%f origin %f/%f scene %f/%f sprite size %f/%f\nactual compass pos: %f/%f"
            px
            py
            area.transform.localPosition.x
            area.transform.localPosition.y
            scene.transform.localPosition.x
            scene.transform.localPosition.y
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

    let customWaypoint (userCreated: bool) (px, py) name (x, y) roomId =
        let dx = x - px
        let dy = y - py
        let dist = sqrt (dx * dx + dy * dy)

        let angle = atan2 dy dx

        { name = name
          roomId = roomId
          distanceMeters = int (dist * 10.0f)
          direction =
            // separate circle into 8 sectors
            // 15.5-0.5 = E
            // 0.5-1.5 = NE
            // ...
            // 14.5-15.5 = SEE
            // (add 16.5 to (16) account for atan2 returning negative values and (0.5) round)
            // (could add 16 and use round instead but thats one more function call wow so slow)
            enum<Dir> (int (angle * float32 (8. / Math.PI) + 16.5f) % 16)
          userCreated = userCreated }

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

        let sceneName = if scene.name = "Grub Pins" then object.name else scene.name
        let roomId = Generated.sceneIdx sceneName

        customWaypoint false (px, py) name (x, y) (if roomId = 0 then None else Some roomId)

    let pointsOfInterest playerPos (area: UnityEngine.GameObject) =
        let pd = PlayerData.instance

        // printfn "scenes mapped {]}"
        Seq.init area.transform.childCount (area.transform.GetChild >> _.gameObject)
        |> if pd.mapAllRooms then
               id
           else
               Seq.filter (fun scene ->
                   if
                       scene.name = "Grub Pins"
                       || pd.scenesMapped.Contains scene.name
                       || scene.activeSelf
                   then
                       logger.LogInfo $"including scene {scene.name}"
                       true
                   else
                       logger.LogWarning $"skipping scene {scene.name}"
                       false)
        |> Seq.collect (fun scene ->
            Seq.init scene.transform.childCount (scene.transform.GetChild >> _.gameObject)
            |> Seq.collect (fun pin ->
                if scene.name = "Grub Pins" then
                    Some grubPin
                else
                    Map.tryFind pin.name pinMap
                    |> Option.orElseWith (fun () ->
                        logger.LogError $"unknown pin type {pin.name}"
                        None)
                |> Option.bind (fun (allow, name) ->
                    logger.LogWarning $"allowed {name pin} ({scene.name}/{pin.name})? {allow pin}"

                    if allow pin then
                        Some(waypoint playerPos area scene (name pin) pin)
                    else
                        None)
                |> Option.toList))
        |> List.ofSeq

    let currentArea () =
        let zone, _sceneName = zoneScene ()
        let area, mapped = mapArea ()

        let name =
            match Language.Language.Get(zone, "Map Zones") with
            | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
            | x -> stripHtml x

        { currentAreaName = name
          currentAreaMapped = mapped
          pointsOfInterest =
            if mapped then
                Some(pointsOfInterest (playerPos ()) area)
            else
                None }

    let allAreas (extra: UnityEngine.GameObject -> PointOfInterest list) =
        let zone, _sceneName = zoneScene ()

        let name =
            match Language.Language.Get(zone, "Map Zones") with
            | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
            | x -> stripHtml x

        let pos = playerPos ()

        { currentAreaName = name
          mappedAreas =
            mappedAreas (fun x _ -> extra x <> [])
            |> Array.map (fun (area, mapped, zone) ->
                let name =
                    match Language.Language.Get(zone, "Map Zones") with
                    | "#!#DIRTMOUTH#!#" -> "Dirtmouth"
                    | x -> stripHtml x

                { areaName = name
                  pointsOfInterest = List.append (extra area) (if mapped then pointsOfInterest pos area else []) })
            |> Array.toList }


(*module Native =
    [<DllImport "libprofiler.so">]
    extern void profiler_reset()

    [<DllImport "libprofiler.so">]
    extern void profiler_save()*)

type WaypointPos =
    { x: float32
      y: float32
      zone: string
      room: int option }

type SaveData =
    { [<SkipSerializingIfNone>]
      waypoints: Map<string, WaypointPos> option }

type UnloadAssets() =
    interface System.Collections.IEnumerator with
        override this.MoveNext() = false
        override this.Current: obj = UnityEngine.Resources.UnloadUnusedAssets()
        override this.Reset() : unit = ()

type Game(plugin: MainClass) =
    inherit Game<Actions>()

    let saveDataType = TypeInfo.fromSystemType typeof<SaveData>
    let mutable tactic = Tactic.DontShoot
    let mutable playerShots = []
    let saveData0: SaveData = { waypoints = None }
    let mutable _saveData: SaveData = saveData0
    let mutable hasMap = false
    let mutable showFm = false
    (* let mutable animOffset = 0

    let mutable healths: UnityEngine.GameObject array =
        [| null; null; null; null; null; null; null; null; null; null; null |]

    member _.Healths = healths*)

    member _.DequeuePlayerShot() =
        match playerShots with
        | a :: b ->
            playerShots <- b
            Some a
        | [] -> None

    member _.Tactic = tactic

    member this.ShowDialogue sheet text =
        this.Context false $"`{sheet}` says: {Context.stripHtml text}"

    member this.SaveDataStr() =
        try
            this.Serialize _saveData
        with exc ->
            Printf.kprintf this.LogError "Data save error: %s " (exc.ToString())
            ""

    member this.SaveData
        with get () = _saveData
        and set x =
            _saveData <- x

            (this.Action DeleteWaypoint).MutateProp "name" (fun x ->
                let x = x :?> StringSchema
                x.Enum <- Some(Option.defaultValue Map.empty _saveData.waypoints |> Map.keys |> Array.ofSeq))

            if hasMap then
                this.RegisterActions [ DeleteWaypoint ]

    member this.LoadData(s: string) =
        try
            this.LogDebug $"loading save data {s}"

            this.SaveData <-
                TypeInfo.deserialize [] saveDataType (JsonValue.Parse s)
                |> Result.map (fun x -> x :?> SaveData)
                |> Result.mapError (fun x ->
                    this.LogDebug $"save data load error: {x}"
                    x)
                |> Result.defaultValue saveData0

            this.ReregisterActions()
        with exc ->
            if s <> "" then
                Printf.kprintf this.LogError "Data load error: %s " (exc.ToString())

            this.SaveData <- saveData0

    override this.ReregisterActions() =
        if hasMap then
            this.RegisterActions Context.mapActions

        this.RegisterActions [ SetTactic; ShootPlayer ]

    override _.Name = "Hollow Knight"

    override this.HandleAction(action: Actions) =

        match action with
        | PathfindRoom sB ->
            let sA = Generated.sceneIdx (SceneManager.GetActiveScene().name)
            let hero = HeroController.instance
            let px, py = hero.transform.position.x, hero.transform.position.y

            match Pathfinding.pathfind sA sB (Some(px, py)) with
            | None ->
                Error(
                    Some
                        "Path not found! Perhaps the target area isn't reachable, or the map is incomplete, or you have to purchase map pins to see more transportation options."
                )
            | Some path ->
                this.LogDebug $"path: {path}"

                let desc =
                    path
                    |> List.collect (fun x ->
                        match x with
                        | ByTram(dir, dist, Some sc) -> Some(dir, dist, $"take the tram to {sc}")
                        | ByStag(dir, dist, sc) -> Some(dir, dist, $"take the stag to {sc}")
                        | ByTram(dir, dist, None) -> Some(dir, dist, "take the tram")
                        | ByFoot(dir, dist, _) -> Some(dir, dist, "")
                        | UseDoor _ -> None
                        | ByElevator(dir, dist) -> Some(dir, dist, "take the elevator")
                        |> Option.toList)
                    |> List.collect (fun (dir, dist, desc) ->
                        let dir = dir.ToString().ToUpper()

                        List.append
                            (if dist > 10f then
                                 [ $"walk {int (dist / 2f)} meters towards {dir}" ]
                             else
                                 [])
                            (if desc = "" then [] else [ desc ]))
                    |> String.concat "; "

                Ok(Some $"The path is: {desc}")


        | ShowMap local ->
            Context.dumpingLocal <- local

            Context.checkMap ()
            |> Result.bind (fun () ->
                let px, py = Context.playerPos ()

                let mkExtra (area: UnityEngine.GameObject) =
                    let extra =
                        this.SaveData.waypoints
                        |> Option.defaultValue Map.empty
                        |> Seq.filter (_.Value >> _.zone >> Context.areaFromZone >> fst >> (=) area)
                        |> Seq.map (fun x ->
                            Context.customWaypoint true (px, py) x.Key (x.Value.x, x.Value.y) x.Value.room)
                        |> List.ofSeq

                    extra

                if local then
                    let ctx = Context.currentArea ()
                    let zone, _sceneName = Context.zoneScene ()
                    let area, _ = Context.areaFromZone zone
                    let extra = mkExtra area

                    let ctx =
                        if List.isEmpty extra then
                            ctx
                        else
                            { ctx with
                                pointsOfInterest =
                                    Some(ctx.pointsOfInterest |> Option.defaultValue [] |> List.append extra) }

                    let ctx =
                        { ctx with
                            pointsOfInterest = ctx.pointsOfInterest |> Option.map (List.sortBy _.distanceMeters) }

                    Ok(Some(this.Serialize ctx))
                else
                    let ctx = Context.allAreas mkExtra
                    Ok(Some(this.Serialize ctx)))
        | CreateWaypoint name ->
            Context.checkMap ()
            |> Result.bind (fun () ->
                let wp = this.SaveData.waypoints |> Option.defaultValue Map.empty

                if wp |> Map.containsKey name then
                    Error(Some $"waypoint `{name}` already exists! delete it first if you want to change its position")
                else
                    let pd = PlayerData.instance

                    let lim =
                        List.sum (
                            List.map
                                (fun (a, b) -> if a then b else 0)
                                [ pd.hasMarker_b, pd.spareMarkers_b
                                  pd.hasMarker_r, pd.spareMarkers_r
                                  pd.hasMarker_w, pd.spareMarkers_w
                                  pd.hasMarker_y, pd.spareMarkers_y ]
                        )

                    if Map.count wp < lim then
                        let zone, _ = Context.zoneScene ()
                        let x, y = Context.playerPos ()

                        let wp =
                            wp
                            |> Map.add
                                name
                                { x = x
                                  y = y
                                  zone = zone
                                  room =
                                    let x = Generated.sceneIdx (SceneManager.GetActiveScene().name)
                                    if x = 0 then None else Some x }

                        this.SaveData <-
                            { this.SaveData with
                                waypoints = Some wp }

                        let keys = Map.keys wp |> Seq.toArray
                        Ok(Some $"waypoint `{name}` added! current user waypoints: {this.Serialize keys}")
                    else
                        if lim = 0 then
                            "You are not yet allowed to create waypoints! The player must buy a set of map markers from Iselda first."
                        else
                            let help =
                                if pd.hasMarker_r && pd.hasMarker_b && pd.hasMarker_w && pd.hasMarker_y then
                                    "Delete an old waypoint in order to create new ones!"
                                else
                                    "You can delete an old waypoint or tell the player to buy additional markers from Iselda to increase the limit."

                            $"You have reached your current limit of {lim} waypoints! {help}"
                        |> Some
                        |> Error)
        | DeleteWaypoint name ->
            Context.checkMap ()
            |> Result.bind (fun () ->
                let wp = this.SaveData.waypoints |> Option.defaultValue Map.empty

                if wp |> Map.containsKey name then
                    let wp = Map.remove name wp

                    this.SaveData <-
                        { this.SaveData with
                            waypoints = Some wp }

                    let keys = Map.keys wp |> Seq.toArray
                    Ok(Some $"Waypoint `{name}` deleted! Remaining user waypoints: {this.Serialize keys}")
                else
                    let keys = Map.keys wp |> Seq.toArray
                    Error(Some $"Waypoint `{name}` not found! Existing user waypoints: {this.Serialize keys}"))
        | ShootPlayer dealDamage ->
            if GameManager.instance.IsGameplayScene() then
                playerShots <- dealDamage :: playerShots
                Ok(Some "Queued a shot towards the player. Repeat the shoot_player action to queue more shots.")
            else
                Error(Some "The player hasn't started playing yet!")
        | SetTactic t ->
            tactic <- t
            Ok(Some $"Successfully set the tactic to {this.Serialize t}.")

    override _.LogError error =
        let fff = "fff"
        plugin.Logger.LogError $"{DateTime.UtcNow}.{DateTime.UtcNow.ToString fff} {error}"

    override _.LogDebug error =
        let fff = "fff"
        plugin.Logger.LogInfo $"{DateTime.UtcNow}.{DateTime.UtcNow.ToString fff} {error}"

    member this.Update() =
        (*try
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

            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F6 then
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)

                plugin.StartCoroutine(UnloadAssets()) |> ignore

            if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F7 then
                showFm <- not showFm
                PlayMakerFSM.BroadcastEvent(if showFm then "FIRST MAP UP" else "FIRST MAP DOWN")*)

        (*if UnityEngine.Input.GetKeyDown UnityEngine.KeyCode.F8 then
                animOffset <- animOffset + 1

            Array.zip
                healths
                ([| "Blue Appear"
                    "Blue Idle"
                    "Health Bound"
                    "Health Empty"
                    "Health Appear"
                    "Health Break"
                    "Blue Break Fast"
                    "Blue Break"
                    "Health Idle"
                    "Health Max Up"
                    "Health Refill" |]
                 |> Array.permute (fun i -> (i + animOffset) % 11))
            |> Array.filter (fst >> (<>) null)
            |> Array.iter (fun (h, n) ->
                let a = h.GetComponent<tk2dSpriteAnimator>()

                if a.CurrentClip = null || a.CurrentClip.name <> n then
                    a.Play n
                    a.CurrentClip.wrapMode <- tk2dSpriteAnimationClip.WrapMode.Loop)*)
        (*with exc ->
            this.Context true $"Exception while handling player input: {exc}"*)

        if PlayerData.instance <> null then
            let gcLevel = Math.Min(4, 2 + Context.countBosses PlayerData.instance / 8)

            if PlayerData.instance.grimmChildLevel <> gcLevel then
                PlayerData.instance.grimmChildLevel <- gcLevel

                UnityEngine.GameObject.FindGameObjectsWithTag "Grimmchild"
                |> Array.choose (fun gc -> PlayMakerFSM.FindFsmOnGameObject(gc, "Control") |> Option.ofObj)
                |> Array.iter (fun fsm -> fsm.SendEvent "LEVEL UP")

            // converts grimmchild charm to carefree melody, dangerous
            PlayerData.instance.nymmInTown <- false
            // double safety (this is already patched on fsm side)
            PlayerData.instance.nightmareLanternAppeared <- false
            PlayerData.instance.nightmareLanternLit <- false

        let hasMap2 = PlayerData.instance <> null && PlayerData.instance.hasMap

        if hasMap2 <> hasMap then
            hasMap <- hasMap2

            (if hasMap then
                 this.RegisterActions
             else
                 this.UnregisterActions)
                Context.mapActions

    member _.LateUpdate() =
        HutongGames.PlayMaker.FsmLog.LoggingEnabled <- true

and [<BepInPlugin("org.chayleaf.hollowneur", "HollowNeuro", "1.0.0")>] MainClass() =
    inherit BaseUnityPlugin()
    let mutable harmony = null
    let mutable game = None
    let mutable tex: UnityEngine.Texture2D = UnityEngine.Texture2D(2, 2)

    let mutable pieces: UnityEngine.Texture2D array =
        Array.init 5 (fun _ -> UnityEngine.Texture2D(2, 2))

    let cts = new Threading.CancellationTokenSource()

    [<DefaultValue>]
    val mutable public Logger: ManualLogSource

    [<DefaultValue>]
    static val mutable private instance: MainClass

    static member Instance = MainClass.instance
    member _.Game = game.Value
    member _.Tex = tex
    member _.Pieces = pieces
    member _.Harmony = harmony

    member this.Awake() =
        IO.Directory.CreateDirectory "fsms" |> ignore

        try
            this.Logger <- base.Logger
            Context.logger <- base.Logger
            // HutongGames.PlayMaker.FsmLog.MirrorDebugLog <- true

            let enemyDamagerLayer = 15
            // layers 3, 4, 6, 7 unused, hijack one for our purposes
            let playerDamagerLayer = 3
            let ballLayer = 20

            UnityEngine.Physics2D.SetLayerCollisionMask(
                playerDamagerLayer,
                UnityEngine.Physics2D.GetLayerCollisionMask enemyDamagerLayer
                ||| (1 <<< ballLayer)
            )

            for i in 0..31 do
                let mask = UnityEngine.Physics2D.GetLayerCollisionMask i
                // copy collision bit for player from enemy
                let mask =
                    mask >>> enemyDamagerLayer &&& 1 <<< playerDamagerLayer
                    ||| (mask &&& ~~~(1 <<< playerDamagerLayer))

                let mask =
                    if i = ballLayer then
                        mask ||| (1 <<< playerDamagerLayer)
                    else
                        mask

                UnityEngine.Physics2D.SetLayerCollisionMask(i, mask)

            do
                let asm = Assembly.GetExecutingAssembly()
                let st = asm.GetManifestResourceStream "HollowNeuro.Resources.texture.png"

                using (new System.IO.MemoryStream()) (fun ms ->
                    st.CopyTo ms
                    UnityEngine.ImageConversion.LoadImage(tex, ms.ToArray()) |> ignore)

                Seq.init 5 (fun i -> asm.GetManifestResourceStream $"HollowNeuro.Resources.hp{i}.png")
                |> Seq.map (fun st ->
                    using (new System.IO.MemoryStream()) (fun ms ->
                        st.CopyTo ms
                        ms.ToArray()))
                |> Seq.iteri (fun i data -> UnityEngine.ImageConversion.LoadImage(pieces[i], data) |> ignore)


            MainClass.instance <- this
            harmony <- Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly())
            // AppDomain.CurrentDomain.GetAssemblies()
            // |> Array.filter (_.FullName >> fun x -> x.Contains "Assembly-CSharp")
            // |> Seq.collect _.GetTypes()
            // |> Seq.filter _.IsClass
            // |> Seq.filter (_.IsGenericType >> not)
            // |> Seq.collect _.GetMethods()
            // |> Seq.filter (_.Name >> (=) "OnTriggerEnter2D")
            // |> Seq.iter (fun met ->
            //     harmony.Patch(met, new HarmonyMethod(1))
            //     )

            let cnt = Seq.fold (fun x _ -> x + 1) 0 (harmony.GetPatchedMethods())

            //typeof<CheatManager>.GetMethod("Init", BindingFlags.NonPublic ||| BindingFlags.Static).Invoke(null, [||])
            //|> ignore
            // PerformanceHUD.Shared.enabled <- true

            game <-
                Some(
                    let game = Game this
                    game.Start(None, cts.Token) |> ignore

                    (game.Action DeleteWaypoint).MutateProp "name" (fun x ->
                        let x = x :?> StringSchema
                        x.Enum <- Some [||])

                    game
                )

            this.Logger.LogInfo $"Plugin HollowNeuro is loaded with {cnt} patches!"
        with exc ->
            this.Logger.LogError $"ERROR {exc}"

    member _.LateUpdate() = game |> Option.iter _.LateUpdate()
    member _.Update() = game |> Option.iter _.Update()
