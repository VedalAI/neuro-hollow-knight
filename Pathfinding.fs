// TODO: UnexploredRooms local
// TODO: area name reporting

namespace HollowNeuro

open System

type ResolvedReachability =
    | Yes
    | Passthru
    | No

type PathSeg =
    // lower: deepnest/acient basin/kingdom's edge
    // upper: no need
    | ByTram of direction: Dir * distance: float32 * targetScene: string option
    | ByStag of direction: Dir * distance: float32 * targetStation: string
    | ByFoot of direction: Dir * distance: float32 * targetDoor: string
    | UseDoor of targetDoor: string
    | ByElevator of direction: Dir * distance: float32

module Pathfinding =
    let followPath seg path =
        match seg, List.tryHead path |> Option.map snd with
        | ByTram(_, _, _), Some(ByTram(_, _, _)) -> false
        | ByStag(_, _, _), Some(ByStag(_, _, _)) -> false
        | UseDoor _, Some(UseDoor _) -> false
        | ByElevator(_, _), Some(ByElevator(_, _)) -> false
        | _ -> true

    let sourcePos s (_, seg) =
        match seg with
        | ByElevator(_, _) ->
            match s with
            | 79 -> 14.95f, 158.79f, None
            | 80 -> 15.149999999999999f, 34.220000000000006f, None
            | 122 -> 14.95f, 159.82f, None
            | 123 -> 14.95f, 41.52f, None
            | _ -> 0f, 0f, None
        | ByTram(_, _, _) ->
            match s with
            | 329 -> 72.61f, 10.67f, Some "door_tram_arrive"
            | 330 -> 24.93f, 10.67f, Some "door_tram_arrive"
            | 331 -> 36.11f, 10.67f, Some "door_tram_arrive"
            | 74 -> 11.28f, 10.788778f, Some "door_tram"
            | 75 -> 44.56f, 10.780245f, Some "door_tram"
            | _ -> 0f, 0f, None
        | ByStag(_, _, _) ->
            match s with
            | 7 -> 142.74000704903017f, 13.929999911393464f, None
            | 9 -> 55.64f, 5.74f, Some "door_stagExit"
            | 77 -> 44.28f, 9.1f, Some "door_stagExit"
            | 107 -> 2.8f, 9.11f, Some "door_stagExit"
            | 120 -> 2.8f, 9.11f, Some "door_stagExit"
            | 145 -> 44.28f, 9.1f, Some "door_stagExit"
            | 166 -> 44.28f, 9.1f, Some "door_stagExit"
            | 220 -> 184.35f, 16.08f, Some "door_stagExit"
            | 233 -> 44.28f, 8.59f, Some "door_stagExit"
            | 244 -> 2.8f, 9.11f, Some "door_stagExit"
            | 281 -> 2.8f, 9.11f, Some "door_stagExit"
            | 346 -> 35.6f, 5.64f, Some "door_stagExit"
            | _ -> 0f, 0f, None
        | ByFoot(_, _, td)
        | UseDoor td ->
            let doors = Generated.sceneDoorsAll s

            // UnityEngine.Debug.LogWarning $"finding: {Generated.sceneNames[s]}[{td}] ({seg})"
            let _, x, y = doors |> Array.find (fun (x, _, _) -> x = td)
            float32 x, float32 y, Some td

    let mapSceneName i =
        let name = Generated.fullSceneNames[i]

        match [| "_boss_defeated"; "_boss"; "_preload" |] |> Array.tryFind name.EndsWith with
        | Some x -> name.Substring(name.Length - x.Length)
        | None -> name

    let reachability () =
        let visMap = PlayerData.instance.scenesVisited |> Set.ofSeq

        Array.init Generated.fullSceneNames.Length (fun i ->
            match Generated.reachability i with
            | Reachability.Always -> Yes
            | Reachability.Visited ->
                if Set.contains (mapSceneName i) visMap || PlayerData.instance.mapAllRooms then
                    Yes
                else
                    No
            | Reachability.Passthru -> Passthru
            | Reachability.Never -> No)

    let stagTargets () =
        [ 9, PlayerData.instance.openedTownBuilding, "Dirtmouth"
          77, PlayerData.instance.openedCrossroads, "Forgotten Crossroads"
          107, PlayerData.instance.openedRuins1, "City Storerooms"
          120, PlayerData.instance.openedRuins2, "King's Station"
          145, PlayerData.instance.openedGreenpath, "Greenpath"
          166, PlayerData.instance.openedFungalWastes, "Queen's Station"
          220, PlayerData.instance.openedRoyalGardens, "Queen's Gardens"
          233, PlayerData.instance.openedStagNest, "Stag Nest"
          244, PlayerData.instance.openedRestingGrounds, "Resting Grounds"
          281, PlayerData.instance.openedDeepnest, "Distant Village"
          346, PlayerData.instance.openedHiddenStation, "Hidden Station" ]
        |> List.choose (fun (x, y, z) -> if y then Some(x, z) else None)

    let stag s =
        if PlayerData.instance.hasPinStag then
            match s with
            | 7 when PlayerData.instance.openedTownBuilding -> Some(142.74000704903017f, 13.929999911393464f)
            | 9 when PlayerData.instance.openedTownBuilding -> Some(55.64f, 5.74f)
            | 77 when PlayerData.instance.openedCrossroads -> Some(44.28f, 9.1f)
            | 107 when PlayerData.instance.openedRuins1 -> Some(2.8f, 9.11f)
            | 120 when PlayerData.instance.openedRuins2 -> Some(2.8f, 9.11f)
            | 145 when PlayerData.instance.openedGreenpath -> Some(44.28f, 9.1f)
            | 166 when PlayerData.instance.openedFungalWastes -> Some(44.28f, 9.1f)
            | 220 when PlayerData.instance.openedRoyalGardens -> Some(184.35f, 16.08f)
            | 233 when PlayerData.instance.openedStagNest -> Some(44.28f, 8.59f)
            | 244 when PlayerData.instance.openedRestingGrounds -> Some(2.8f, 9.11f)
            | 281 when PlayerData.instance.openedDeepnest -> Some(2.8f, 9.11f)
            | 346 when PlayerData.instance.openedHiddenStation -> Some(35.6f, 5.64f)
            | _ -> None
        else
            None

    let tram s =
        if PlayerData.instance.hasTramPass && PlayerData.instance.hasPinTram then
            match s with
            | 329 when PlayerData.instance.openedTramLower ->
                72.61f, 10.67f, [| 330, Some "Deepnest"; 331, Some "Kingdom's Edge" |]
            | 330 when PlayerData.instance.openedTramLower ->
                24.93f, 10.67f, [| 329, Some "Ancient Basin"; 331, Some "Kingdom's Edge" |]
            | 331 when PlayerData.instance.openedTramLower ->
                36.11f, 10.67f, [| 329, Some "Ancient Basin"; 330, Some "Deepnest" |]
            | 74 when PlayerData.instance.openedTramRestingGrounds -> 11.28f, 10.788778f, [| 75, None |]
            | 75 when PlayerData.instance.openedTramRestingGrounds -> 44.56f, 10.780245f, [| 74, None |]
            | _ -> 0f, 0f, [||]
        else
            0f, 0f, [||]

    let elevator s =
        match s with
        | 79 when PlayerData.instance.cityLift1 -> Some(80, 14.95, 158.79)
        | 80 when PlayerData.instance.cityLift1 -> Some(79, 15.149999999999999, 34.220000000000006)
        | 122 when PlayerData.instance.cityLift2 -> Some(123, 14.95, 159.82)
        | 123 when PlayerData.instance.cityLift2 -> Some(122, 14.95, 41.52)
        | _ -> None

    let lowerTramStations = [| 329; 330; 331 |]

    let pathfind
        sA
        (target: int -> ((int * float32 * float32) * PathSeg) list -> bool)
        (pos: (float32 * float32) option)
        =
        // find shortest path to target scene
        //let mutable q = Map.empty
        let mutable i = 0

        let q =
            System.Collections.Generic.SortedDictionary<float32 * int, int * ((int * float32 * float32) * PathSeg) list>()

        let visS = System.Collections.Generic.HashSet<int>()
        let visD = System.Collections.Generic.HashSet<int * string>()

        let reachable = reachability ()
        let reachable = reachable |> Array.map ((<>) ResolvedReachability.No)

        let addElem s sxy simpleSeg properSeg (old: ((int * float32 * float32) * PathSeg) list) chk =
            if followPath simpleSeg old && chk () then
                let dist, seg = properSeg ()
                let w = s, (sxy, seg) :: old
                q.Add((dist, i), w)
                i <- i + 1

        let popElem () =
            let ret = q |> Seq.head
            let k = ret.Key
            let v = ret.Value
            q.Remove k |> ignore
            fst k, v

        let x0, y0 =
            pos
            |> Option.defaultWith (fun () ->
                let doors = Generated.sceneDoorsAll sA
                let sx = doors |> Array.map (fun (_, x, y) -> float32 x) |> Array.sum
                let sy = doors |> Array.map (fun (_, x, y) -> float32 y) |> Array.sum
                let ax = sx / float32 (Array.length doors)
                let ay = sy / float32 (Array.length doors)
                ax, ay)

        let stagT = stagTargets ()

        let visScene sA x0 y0 old oldDist =
            let dirDist =
                fun (x: float32) (y: float32) ->
                    let dx = x - x0
                    let dy = y - y0
                    let dist = sqrt (dx * dx + dy * dy)

                    let angle = atan2 dy dx
                    enum<Dir> (int (angle * float32 (8. / Math.PI) + 16.5f) % 16), dist

            let doors =
                let reachable = Generated.sceneDoors sA

                if List.isEmpty old && List.isEmpty reachable then
                    // as fallback, access every door
                    // TODO: fold double move
                    // TODO: not go through twice
                    Generated.sceneDoorsAll sA
                else
                    // access guaranteed reachable door
                    reachable
                    |> Seq.choose (fun (x, w, y, z) -> if w then Some(x, y, z) else None)
                    |> Array.ofSeq

            doors
            |> Array.iter (fun (door, x, y) ->
                addElem
                    sA
                    (sA, float32 x, float32 y)
                    (ByFoot(Dir.N, 0f, door))
                    (fun () ->
                        let dir, dist = dirDist (float32 x) (float32 y)
                        // UnityEngine.Debug.LogWarning $"can walk {Generated.sceneNames[sA]}[->{door}]"
                        oldDist + dist, ByFoot(dir, dist, door))
                    old
                    (fun () -> visD.Contains((sA, door)) |> not))

            do
                let tx, ty, t = tram sA
                let dir, dist = if Array.isEmpty t then Dir.N, 0f else dirDist tx ty

                t
                |> Array.iter (fun (s, n) ->
                    let bt = ByTram(dir, dist, n)

                    addElem
                        s
                        (sA, float32 tx, float32 ty)
                        bt
                        (fun () ->
                            // UnityEngine.Debug.LogWarning
                            //     $"can tram {Generated.sceneNames[sA]} -> {Generated.sceneNames[s]}"

                            oldDist + dist + 50f, bt)
                        old
                        (fun () -> visS.Contains s |> not))

            elevator sA
            |> Option.iter (fun (s, x, y) ->
                addElem
                    s
                    (sA, float32 x, float32 y)
                    (ByElevator(Dir.N, 0f))
                    (fun () ->
                        let dir, dist = dirDist (float32 x) (float32 y)

                        // UnityEngine.Debug.LogWarning
                        //     $"can elev {Generated.sceneNames[sA]} -> {Generated.sceneNames[s]}"

                        oldDist + dist + 30f, ByElevator(dir, dist))
                    old
                    (fun () -> visS.Contains s |> not))

            stag sA
            |> Option.iter (fun (x, y) ->
                let dir, dist = dirDist x y

                stagT
                |> List.iter (fun (s, n) ->
                    // UnityEngine.Debug.LogWarning $"can stag {Generated.sceneNames[sA]} -> {Generated.sceneNames[s]}"

                    let bs = ByStag(dir, dist, n)

                    addElem s (sA, x, y) bs (fun () -> oldDist + dist + 50f, bs) old (fun () ->
                        visS.Contains s |> not)))

        let rec iter =
            fun () ->
                if q.Count = 0 then
                    // UnityEngine.Debug.LogWarning(
                    //     "pathfinding debug: "
                    //     + (visS
                    //        |> Array.ofSeq
                    //        |> Array.sort
                    //        |> Array.map (Array.get Generated.sceneNames)
                    //        |> String.concat ";")
                    //     + (visD
                    //        |> Array.ofSeq
                    //        |> Array.sort
                    //        |> Array.map (fun (i, s) -> $"{Array.get Generated.sceneNames i}[{s}]")
                    //        |> String.concat ";")
                    // )

                    None
                else
                    let oldDist, (s, m) = popElem ()
                    let x0, y0, d = sourcePos s (List.head m)
                    //UnityEngine.Debug.LogWarning $"pathfinding debug: {x0}/{y0} {Generated.sceneNames[s]}[{d}]: {m}"

                    let added = visS.Add s

                    if added && target s m then
                        Some(List.rev m)
                    else
                        let dirDist =
                            fun x y ->
                                let dx = x - x0
                                let dy = y - y0
                                let dist = sqrt (dx * dx + dy * dy)
                                let angle = atan2 dy dx
                                enum<Dir> (int (angle * float32 (8. / Math.PI) + 16.5f) % 16), dist

                        // visit scene
                        if added && reachable[s] then
                            visScene s x0 y0 m oldDist

                        // visit doors
                        d
                        |> Option.iter (fun d ->
                            if visD.Add((s, d)) && reachable[s] then
                                // to other room
                                let ts, td = Generated.doorTarget s d

                                if td <> "" then
                                    let ud = UseDoor td

                                    addElem
                                        ts
                                        (0, 0f, 0f)
                                        ud
                                        (fun () ->
                                            // UnityEngine.Debug.LogWarning
                                            //     $"can pass {Generated.sceneNames[s]}[{d}] -> {Generated.sceneNames[ts]}[{td}]"

                                            oldDist + 10f, ud)
                                        m
                                        (fun () -> visD.Contains((ts, td)) |> not)

                                // within room
                                Generated.doorDoors s d
                                |> List.iter (fun (door, cond, dist, dir, x, y) ->
                                    if cond then
                                        // UnityEngine.Debug.LogWarning $"can walk {Generated.sceneNames[s]}[{d} -> {door}]"

                                        let bf = ByFoot(dir, float32 dist, door)

                                        addElem
                                            s
                                            (s, float32 x, float32 y)
                                            bf
                                            (fun () -> oldDist + float32 dist, bf)
                                            m
                                            (fun () -> visD.Contains((s, door)) |> not)))

                        iter ()

        if reachable[sA] then
            if target sA [] then
                Some []
            else
                visS.Add sA |> ignore
                visScene sA x0 y0 [] 0f
                iter ()
        else
            None

type PathfindingBall() =
    inherit UnityEngine.MonoBehaviour()
    static let mutable path: (int * float32 * float32) list = List.empty
    static let mutable object: UnityEngine.GameObject = null

    do object <- base.gameObject
    static member Waiting = object <> null && not object.activeSelf && path |> List.isEmpty |> not
    static member Object = object

    static member InitWith p =
        UnityEngine.Debug.LogError $"ball init {p}"
        path <- p
        let h = HeroController.instance.gameObject.transform
        let t = object.transform
        t.localScale <- UnityEngine.Vector3(1f, 1f, 0f)
        t.parent <- h.parent

    static member Target =
        let scene =
            Generated.sceneIdx (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)

        let rec update () =
            match path with
            | [] -> None
            | (s, x, y) :: r when
                s = scene
                && (GameManager.instance.sm.darknessLevel = 0 || PlayerData.instance.hasLantern)
                ->
                // position around the player
                let h = HeroController.instance.gameObject.transform

                // distance between hero and target
                let dist =
                    UnityEngine.Vector2.Distance(
                        UnityEngine.Vector2(h.position.x, h.position.y),
                        UnityEngine.Vector2(x, y)
                    )

                let dist = min dist 5.0f

                let path = UnityEngine.Vector2(x - h.position.x, y - h.position.y).normalized * dist

                Some(UnityEngine.Vector2(h.localPosition.x + path.x, h.localPosition.y + path.y))
            | _ :: r ->
                path <- r
                update ()

        update ()

    member this.Update() =
        match PathfindingBall.Target with
        | None ->
            let fadeStep = UnityEngine.Time.deltaTime / 0.5f
            let t = this.gameObject.transform

            if t.localScale.x <= fadeStep then
                t.localScale <- UnityEngine.Vector3(1.0f, 1.0f, this.gameObject.transform.localScale.z)
                this.gameObject.SetActive false
            else
                t.localScale <-
                    UnityEngine.Vector3(
                        t.localScale.x - fadeStep,
                        t.localScale.y - fadeStep,
                        this.gameObject.transform.localScale.z
                    )
        | Some target ->
            let t = this.gameObject.transform
            t.parent <- HeroController.instance.transform.parent
            let current = UnityEngine.Vector2(t.localPosition.x, t.localPosition.y)
            let interpolated = (target * 1f + current * 29f) / 30f
            t.localPosition <- UnityEngine.Vector3(interpolated.x, interpolated.y, t.localPosition.z)
