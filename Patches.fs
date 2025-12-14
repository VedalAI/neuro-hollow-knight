namespace HollowNeuro

open HarmonyLib
open HutongGames.PlayMaker

type WaitAction(act: Actions.Wait) =
    inherit FsmStateAction()
    let mutable startTime = 0.0f

    override this.OnEnter() : unit =
        if act.realTime then
            startTime <- FsmTime.RealtimeSinceStartup
        else
            startTime <- 0.0f

        if act.time.Value <= 0.0f then
            MainClass.Instance.Logger.LogInfo $"finishing immediately"

        act.OnEnter()

        if act.Finished then
            this.Finish()

    override this.OnUpdate() : unit =
        if act.realTime then
            MainClass.Instance.Logger.LogInfo $"progress {FsmTime.RealtimeSinceStartup - startTime}/{act.time.Value}"
        else
            MainClass.Instance.Logger.LogInfo $"progress {startTime}/{act.time.Value}"
            startTime <- startTime + UnityEngine.Time.deltaTime

        act.OnUpdate()

        if act.Finished then
            this.Finish()

type CustomAction() =
    inherit Actions.SendEventByName()

    override _.OnEnter() : unit =
        MainClass.Instance.Logger.LogInfo "enter"
        base.OnEnter()

    override _.OnUpdate() : unit =
        MainClass.Instance.Logger.LogInfo "update"
        base.OnUpdate()

type IsGameplaySceneAction(act: Actions.CallMethodProper) =
    inherit FsmStateAction()

    override this.OnEnter() : unit =
        let ret = GameManager.instance.IsGameplayScene()
        act.storeResult.SetValue ret
        MainClass.Instance.Logger.LogInfo $"is gameplay {GameManager.instance.sceneName}? {ret}"

        let grimm = UnityEngine.GameObject.FindGameObjectsWithTag "Grimmchild"

        grimm
        |> Array.iter (fun g ->
            MainClass.Instance.Logger.LogInfo $"grimm aself {g.activeSelf} ahier {g.activeInHierarchy}"
            ())

        this.Finish()

type CustomTrigger2D(act: Actions.Trigger2dEvent) =
    inherit FsmStateAction()
    let mutable proxy: PlayMakerUnity2DProxy = null

    let trigger (desc: string) (collisionInfo: UnityEngine.Collider2D) =
        MainClass.Instance.Logger.LogInfo $"trigger {desc}: {collisionInfo.gameObject.name}"

        if
            collisionInfo.gameObject.tag = act.collideTag.Value
            || act.collideTag.IsNone
            || System.String.IsNullOrEmpty act.collideTag.Value
            || act.collideTag.Value = "Untagged"
        then
            MainClass.Instance.Logger.LogInfo "sending event"
            act.Fsm.Event act.sendEvent

    let collision (desc: string) (collisionInfo: UnityEngine.Collision2D) =
        MainClass.Instance.Logger.LogInfo
            $"collision {desc}: {collisionInfo.collider.tag}/{collisionInfo.otherCollider.tag}/{collisionInfo.gameObject.name}"

        if
            collisionInfo.gameObject.tag = act.collideTag.Value
            || act.collideTag.IsNone
            || System.String.IsNullOrEmpty act.collideTag.Value
            || act.collideTag.Value = "Untagged"
        then
            MainClass.Instance.Logger.LogInfo "sending event"
            act.Fsm.Event act.sendEvent

    override _.Init(state: FsmState) : unit =
        base.Init state
        act.Init state

    override _.Reset() : unit = act.Reset()

    override this.OnEnter() : unit =
        proxy <-
            base.Owner.GetComponent<PlayMakerUnity2DProxy>()
            |> Option.ofObj
            |> Option.defaultWith base.Owner.AddComponent<PlayMakerUnity2DProxy>

        let proxy2 =
            base.Owner.transform.parent.gameObject.GetComponent<PlayMakerUnity2DProxy>()
            |> Option.ofObj
            |> Option.defaultWith base.Owner.transform.parent.gameObject.AddComponent<PlayMakerUnity2DProxy>

        // match act.trigger with
        // | PlayMakerUnity2d.Trigger2DType.OnTriggerEnter2D -> proxy.AddOnCollisionEnter2dDelegate (collision "enter")
        // | PlayMakerUnity2d.Trigger2DType.OnTriggerStay2D -> proxy.AddOnCollisionStay2dDelegate (collision "stay")
        // | PlayMakerUnity2d.Trigger2DType.OnTriggerExit2D -> proxy.AddOnCollisionExit2dDelegate (collision "exit")
        // | _ -> ()
        [ proxy, "damager"; proxy2, "ball" ]
        |> List.iter (fun (proxy, name) ->
            proxy.AddOnCollisionEnter2dDelegate(collision $"enter {name}")
            proxy.AddOnCollisionStay2dDelegate(collision $"stay {name}")
            proxy.AddOnCollisionExit2dDelegate(collision $"exit {name}")
            proxy.AddOnTriggerEnter2dDelegate(trigger $"enter {name}")
            proxy.AddOnTriggerStay2dDelegate(trigger $"stay {name}")
            proxy.AddOnTriggerExit2dDelegate(trigger $"exit {name}"))

        if (this.Fsm.Variables.FindFsmBool "isPlayer").Value then
            MainClass.Instance.Logger.LogInfo $"balllayer {base.Owner.layer}/{base.Owner.gameObject.transform.parent.gameObject.layer} -> {MainClass.Instance.Game.BallLayer}"

            UnityEngine.Physics2D.SetLayerCollisionMask(
                3,
                UnityEngine.Physics2D.GetLayerCollisionMask 15
                ||| (1 <<< MainClass.Instance.Game.BallLayer)
            )

            for i in 0..31 do
                let mask = UnityEngine.Physics2D.GetLayerCollisionMask i
                // copy 3 from 15
                let mask = mask &&& ~~~(1 <<< 3) ||| (mask &&& (1 <<< 15) >>> 12)

                let mask =
                    if i = MainClass.Instance.Game.BallLayer then
                        mask ||| (1 <<< 3) // ||| (1 <<< 15)// ||| (1 <<< 14)
                    else
                        mask

                UnityEngine.Physics2D.SetLayerCollisionMask(i, mask)
            //UnityEngine.Physics2D.SetLayerCollisionMask(3, UnityEngine.Physics2D.GetLayerCollisionMask 15) // ||| (1 <<< MainClass.Instance.Game.BallLayer))
            base.Owner.layer <- 3

            let dmg =
                base.Owner.GetComponent<DamageHero>()
                |> Option.ofObj
                |> Option.defaultWith base.Owner.AddComponent<DamageHero>

            dmg.damageDealt <- 1
        // if (this.Fsm.Variables.FindFsmBool "isPlayer").Value then
        //     MainClass.Instance.Logger.LogInfo $"setting collide layer to empty"
        //     act.collideLayer.Value <- HeroController.instance.gameObject.layer
        // else
        //     MainClass.Instance.Logger.LogInfo $"setting collide layer to enemies"
        //     act.collideLayer.Value <- "Enemies"
        // act.collideTag.Value <- "Untagged"
        // MainClass.Instance.Logger.LogInfo $"tag: {act.collideTag.Value}"

        act.OnEnter()

    override _.OnExit() : unit = act.OnExit()
    override _.ErrorCheck() : string = act.ErrorCheck()

    // override _.DoCollisionEnter2D(collisionInfo: UnityEngine.Collision2D) : unit =
    //     MainClass.Instance.Logger.LogInfo $"collision enter: {collisionInfo}"
    //
    // override _.DoTriggerEnter(other: UnityEngine.Collider) : unit =
    //     MainClass.Instance.Logger.LogInfo $"trigger enter: {other}"

type SetIsPlayerAction() =
    inherit FsmStateAction()
    static let mutable isPlayer = false

    static member IsPlayer
        with set x = isPlayer <- x

    override this.OnEnter() : unit =
        let damager = (this.Fsm.Variables.FindFsmGameObject "Damager").Value
        let fsm = damager.LocateMyFSM "Attack"
        (fsm.FsmVariables.FindFsmBool "isPlayer").Value <- isPlayer
        MainClass.Instance.Logger.LogDebug $"Set isPlayer to {isPlayer}"
        this.Finish()

type FsmLogger(s: string) =
    inherit FsmStateAction()

    override this.OnEnter() : unit =
        MainClass.Instance.Logger.LogDebug s
        this.Finish()

module Util =
    let stateByName name (x: FsmState array) = x |> Array.find (_.Name >> (=) name)

[<HarmonyPatch>]
type public Patches() =
    static let mutable fsmInitialized = false
    static let mutable heroBox: UnityEngine.GameObject = null
    static let mutable lastNames = []
    // names that are reserved for specific entities
    // (so labels aren't transient but have some persistence)
    static let mutable reservedNames: ((string * int) * UnityEngine.GameObject) list = []

    static let neuroSaveSlotPath (slotIndex: int) =
        System.IO.Path.Combine(
            UnityEngine.Application.persistentDataPath,
            if slotIndex = 0 then
                "neuro.dat"
            else
                $"neuro{slotIndex}.dat"
        )

    static let labelTargets (targets: UnityEngine.GameObject list) =
        reservedNames <-
            reservedNames
            |> List.filter (fun (_, x) ->
                // somehow, null checks aren't enough, i have to try-catch it
                try
                    // NPE here even with x <> null... maybe activeSelf property's native code is somehow returning null...?
                    // ...or accesses something null or whatever
                    x.activeSelf
                with :? System.NullReferenceException ->
                    false)

        targets
        |> List.mapFold
            (fun state target ->
                // if target has a reserved name, use that
                // otherwise, find the first non-reserved name and use that
                // (map is probably more overhead than savings but whatever)
                match reservedNames |> List.tryFind (snd >> (=) target) with
                | Some((name, count), _) -> ((if count = 0 then name else $"{name} ({count + 1})"), target), state
                | None ->
                    let name = target.name

                    let rec findUnusedName count =
                        if reservedNames |> List.exists (fst >> (=) (name, count)) then
                            findUnusedName (count + 1)
                        else
                            reservedNames <- ((name, count), target) :: reservedNames

                            ((if count = 0 then name else $"{name} ({count + 1})"), target),
                            Map.add name (count + 1) state

                    findUnusedName (Map.tryFind name state |> Option.defaultValue 0))
            Map.empty
        |> fst

    [<HarmonyPatch(typeof<CheatManager>, "IsCheatsEnabled", MethodType.Getter)>]
    [<HarmonyPostfix>]
    static member public EnableCheats(__result: bool byref) = __result <- true

    [<HarmonyPatch(typeof<PlayerData>, "SetupNewPlayerData")>]
    [<HarmonyPostfix>]
    static member public EnableGrimmChild(__instance: PlayerData) =
        __instance.charmCost_40 <- 0
        __instance.gotCharm_40 <- true
        __instance.equippedCharm_40 <- true
        __instance.charmsOwned <- 1
        __instance.hasCharm <- true
        __instance.EquipCharm 40

    [<HarmonyPatch(typeof<HeroController>, "SetupGameRefs")>]
    [<HarmonyPostfix>]
    static member public SetupHeroGameRefs(__instance: HeroController) =
        if fsmInitialized then
            ()
        else
            fsmInitialized <- true

            // allow grimmchild to spawn on game start
            // this works by enabling the Wait action which delays spawn by 0.25s
            // this gives time for grimmchild to despawn if already spawned (which happens upon LEVEL LOADED)
            // if not already despawned, the spawn check runs before grimmchild despawns and prevents spawn
            // (yes this is confusing and took literal days to figure out)
            let sg =
                (__instance.transform.Find "Charm Effects").gameObject.LocateMyFSM "Spawn Grimmchild"

            let sp = Util.stateByName "Spawn Pause" sg.FsmStates
            sp.Actions[0].Enabled <- true

    // let spawn = sg.FsmStates |> Array.find (_.Name >> (=) "Spawn")

    // let grimmchild =
    //     (spawn.Actions |> Array.find (fun x -> x :? Actions.SpawnObjectFromGlobalPool)
    //     :?> Actions.SpawnObjectFromGlobalPool)
    //         .gameObject.Value

    // let grimmfsm = FSMUtility.LocateFSM(grimmchild, "Control")
    // grimmfsm.Fsm.InitData()
    // let shoot = grimmfsm.FsmStates |> Array.find (_.Name >> (=) "Shoot")

    // let ball =
    //     (shoot.Actions |> Array.find (fun x -> x :? Actions.SpawnObjectFromGlobalPool)
    //     :?> Actions.SpawnObjectFromGlobalPool)
    //         .gameObject.Value

    // let ballfsm = FSMUtility.LocateFSM(ball, "Control")
    // ballfsm.Fsm.InitData()
    // let ballInit = ballfsm.FsmStates |> Array.find (_.Name >> (=) "Init")

    // ballInit.LoadActions()

    // let damager =
    //     (ballInit.Actions |> Array.find (fun x -> x :? Actions.ActivateGameObject)
    //     :?> Actions.ActivateGameObject)
    //         .gameObject.GameObject.Value

    // let dfsm = FSMUtility.LocateFSM(damager, "Attack")
    // dfsm.Fsm.InitData()
    // let isPlayer = FsmBool "isPlayer"
    // isPlayer.Value <- false
    // dfsm.FsmVariables.BoolVariables <- Array.append dfsm.FsmVariables.BoolVariables [| isPlayer |]
    // let n = shoot.Actions |> Array.findIndex (fun x -> x :? Actions.SetFsmInt)
    // let a, b = Array.splitAt n shoot.Actions
    // let setIsPlayer = SetIsPlayerAction()
    // shoot.Actions <- Array.concat [| a; [| setIsPlayer |]; b |]
    // setIsPlayer.Init shoot



    // let fsm = __instance.proxyFSM
    // let initState = fsm.FsmStates |> Seq.find (_.Name >> (=) "Init")
    // let ev = new HutongGames.PlayMaker.Actions.SendEventByName()
    // let ev = CustomAction()
    // let et = new FsmEventTarget()
    // et.target <- FsmEventTarget.EventTarget.BroadcastAll
    // ev.eventTarget <- et
    // ev.sendEvent <- FsmString.op_Implicit "CHARM EQUIP CHECK"
    // ev.delay <- FsmFloat 0f
    // initState.Actions <- Array.append initState.Actions [| ev |]
    // Printf.ksprintf MainClass.Instance.Logger.LogInfo "%d - %s" initState.Actions.Length $"{ev}"
    // ev.Init initState

    [<HarmonyPatch(typeof<HeroBox>, "Start")>]
    [<HarmonyPrefix>]
    static member public HeroBoxStart(__instance: HeroBox) =
        heroBox <- __instance.gameObject
        Printf.ksprintf MainClass.Instance.Logger.LogInfo "hblayer %d" __instance.gameObject.layer

    [<HarmonyPatch(typeof<HeroBox>, "OnTriggerEnter2D")>]
    [<HarmonyPrefix>]
    static member public TriggerEnter(__instance: HeroBox, otherCollider: UnityEngine.Collider2D) =
        MainClass.Instance.Logger.LogInfo $"theroenter {otherCollider.gameObject.name}"

    // [<HarmonyPatch(typeof<HeroBox>, "OnTriggerStay2D")>]
    // [<HarmonyPrefix>]
    // static member public TriggerStay(__instance: HeroBox, otherCollider: UnityEngine.Collider2D) =
    //     MainClass.Instance.Logger.LogInfo $"therostay {otherCollider.gameObject.name}"


    // [<HarmonyPatch(typeof<WaitForHeroInPosition>, nameof Unchecked.defaultof<WaitForHeroInPosition>.OnEnter)>]
    // [<HarmonyPrefix>]
    // static member public Waaaaaa(__instance: WaitForHeroInPosition) =
    //     MainClass.Instance.Logger.LogInfo $"already in position? {HeroController.instance.isHeroInPosition}"

    //     if HeroController.instance <> null && HeroController.instance.isHeroInPosition then
    //         __instance.Fsm.Event __instance.sendEvent

    [<HarmonyPatch(typeof<Fsm>, nameof (Unchecked.defaultof<Fsm>.ProcessEvent))>]
    [<HarmonyPrefix>]
    static member public Proc(__instance: Fsm, fsmEvent: FsmEvent) =
        if __instance.Active && not (FsmEvent.IsNullOrEmpty fsmEvent) then
            //MainClass.Instance.Logger.LogInfo $"{__instance.Owner.name}/{__instance.Name}: processing {fsmEvent.Name}"
            ()

    // [<HarmonyPatch(typeof<GameMap>, nameof (Unchecked.defaultof<GameMap>.SetupMap))>]
    // [<HarmonyPrefix>]
    // static member public SetupMap(__instance: GameMap) =
    //     let inst = __instance

    //     let pd = PlayerData.instance

    //     Array.init inst.transform.childCount inst.transform.GetChild
    //     |> Array.iter (fun x ->
    //         Array.init x.gameObject.transform.childCount x.gameObject.transform.GetChild
    //         |> Array.iter (fun y ->
    //             if pd.scenesMapped.Contains y.gameObject.name || true then
    //                 MainClass.Instance.Logger.LogInfo $"scene {y.gameObject.name}"

    //                 Array.init y.gameObject.transform.childCount y.gameObject.transform.GetChild
    //                 |> Array.iter (fun z ->
    //                     MainClass.Instance.Logger.LogInfo $"- {z.gameObject.name}"

    //                     if y.gameObject.name = "Grub Pins" then
    //                         let w1, w2 = Context.grubPin
    //                         MainClass.Instance.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"
    //                     else
    //                         Context.pinMap
    //                         |> Map.tryFind z.gameObject.name
    //                         |> Option.iter (fun (w1, w2) ->
    //                             MainClass.Instance.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"))
    //             else
    //                 MainClass.Instance.Logger.LogInfo $"skipping scene {y.gameObject.name}"))

    // [<HarmonyPatch(typeof<FsmLog>, "AddEntry")>]
    // [<HarmonyPrefix>]
    // static member public LogFsm(entry: FsmLogEntry) =
    //     //absMainClass.Instance.Logger.LogInfo "a"
    //     entry.DebugLog()

    [<HarmonyPatch(typeof<GameManager>, nameof (Unchecked.defaultof<GameManager>.ClearSaveFile))>]
    [<HarmonyPrefix>]
    static member public ClearNeuroData(saveSlot: int, callback: System.Action<bool> byref) =
        let path = neuroSaveSlotPath saveSlot

        let cb = callback

        callback <-
            System.Action<bool>(fun success ->
                if success then
                    try
                        System.IO.File.Delete path
                    with _ ->
                        ()

                if cb <> null then
                    cb.Invoke success)

    [<HarmonyPatch(typeof<GameManager>, nameof (Unchecked.defaultof<GameManager>.LoadGame))>]
    [<HarmonyPrefix>]
    static member public LoadNeuroData(saveSlot: int, callback: System.Action<bool> byref) =
        let path = neuroSaveSlotPath saveSlot

        let slotData =
            try
                System.IO.File.ReadAllText path
            with exc ->
                UnityEngine.Debug.LogException exc
                ""

        let cb = callback

        callback <-
            System.Action<bool>(fun success ->
                if success then
                    MainClass.Instance.Game.LoadData slotData

                if cb <> null then
                    cb.Invoke success)

    [<HarmonyPatch(typeof<GameManager>,
                   nameof (Unchecked.defaultof<GameManager>.SaveGame),
                   [| typeof<int>; typeof<System.Action<bool>> |])>]
    [<HarmonyPrefix>]
    static member public SaveNeuroData(__instance: GameManager, saveSlot: int, callback: System.Action<bool> byref) =
        if not __instance.gameConfig.disableSaveGame then
            let cb = callback
            let path = neuroSaveSlotPath saveSlot
            let pathTmp = path + ".new"
            let pathBak = path + ".bak"

            let wroteNeuroData =
                try
                    let data = MainClass.Instance.Game.SaveData()
                    System.IO.File.WriteAllText(pathTmp, data)
                    true
                with exc ->
                    UnityEngine.Debug.LogException exc
                    false

            callback <-
                System.Action<bool>(fun success ->
                    if wroteNeuroData && success then
                        try
                            if System.IO.File.Exists path then
                                System.IO.File.Replace(pathTmp, path, pathBak)
                            else
                                System.IO.File.Move(pathTmp, path)
                        with exc ->
                            UnityEngine.Debug.LogException exc

                    if cb <> null then
                        cb.Invoke success)

            ()

    [<HarmonyPatch(typeof<PlayMakerUnity2DProxy>, nameof (Unchecked.defaultof<PlayMakerUnity2DProxy>.Start))>]
    [<HarmonyPostfix>]
    static member public DebugProxy(__instance: PlayMakerUnity2DProxy) = () // __instance.debug <- true

    [<HarmonyPatch(typeof<PlayMakerFSM>, "Awake")>]
    [<HarmonyPostfix>]
    static member public FsmAwake(__instance: PlayMakerFSM) =
        MainClass.Instance.Logger.LogInfo $"awake {__instance.gameObject.name} - {__instance.FsmName}"

        match __instance.gameObject.name, __instance.FsmName with
        | "Enemy Damager", "Attack" ->
            if
                __instance.FsmVariables.BoolVariables.Length = 1
                && __instance.FsmVariables.FloatVariables.Length = 1
            then
                let isPlayer = FsmBool "isPlayer"
                isPlayer.Value <- false

                __instance.FsmVariables.BoolVariables <-
                    Array.append __instance.FsmVariables.BoolVariables [| isPlayer |]

                let detect = __instance.FsmStates |> Array.find (_.Name >> (=) "Detect")
                detect.Actions[0] <- CustomTrigger2D(detect.Actions[0] :?> Actions.Trigger2dEvent)
                detect.Actions[0].Init detect
                let inv = __instance.FsmStates |> Array.find (_.Name >> (=) "Invincible?")
                let log0 = FsmLogger "pre detect"
                let log1 = FsmLogger "post detect"
                detect.Actions <- Array.concat [| [| log0 |]; detect.Actions; [| log1 |] |]
                let log0 = FsmLogger "pre inv"
                let log1 = FsmLogger "post inv"
                inv.Actions <- Array.concat [| [| log0 |]; inv.Actions; [| log1 |] |]

                MainClass.Instance.Logger.LogInfo "added bool var"
        | "Grimmchild(Clone)", "Control" ->
            let shoot = __instance.FsmStates |> Array.find (_.Name >> (=) "Shoot")

            if shoot.Actions.Length = 10 then
                let n = shoot.Actions |> Array.findIndex (fun x -> x :? Actions.SetFsmInt)
                let setIsPlayer = SetIsPlayerAction()
                shoot.Actions <- Array.insertAt n setIsPlayer shoot.Actions
                setIsPlayer.Init shoot
                MainClass.Instance.Logger.LogInfo "added SetIsPlayer"
            else
                MainClass.Instance.Logger.LogInfo "not updating grimmchild control"
        | _ -> ()

    [<HarmonyPatch(typeof<GrimmEnemyRange>, nameof (Unchecked.defaultof<GrimmEnemyRange>.GetTarget))>]
    [<HarmonyPostfix>]
    static member public GrimmTarget(__instance: GrimmEnemyRange, __result: UnityEngine.GameObject byref) =
        let g = MainClass.Instance.Game

        let targets =
            __instance.enemyList
            |> Seq.filter (fun x ->
                UnityEngine.Physics2D.Linecast(__instance.transform.position, x.transform.position, 256)
                |> UnityEngine.RaycastHit2D.op_Implicit
                |> not)
            |> Seq.sortBy (_.transform.position >> (-) __instance.transform.position >> _.sqrMagnitude)
            |> List.ofSeq
            |> labelTargets

        let rec selectTarget t =
            match t with
            | [] ->
                g.Targets <- t
                null
            | "Player" :: xs ->
                g.Targets <- xs
                SetIsPlayerAction.IsPlayer <- true

                if heroBox = null then
                    HeroController.instance.gameObject
                else
                    heroBox
            | x :: xs ->
                match List.tryFind (fst >> (=) x) targets with
                | Some x ->
                    g.Targets <- t
                    SetIsPlayerAction.IsPlayer <- false
                    snd x
                | None -> selectTarget xs

        __result <- selectTarget g.Targets

        let names = List.sort (List.map fst targets)

        let names =
            if
                UnityEngine.Physics2D.Linecast(
                    __instance.transform.position,
                    HeroController.instance.gameObject.transform.position,
                    256
                )
                |> UnityEngine.RaycastHit2D.op_Implicit
                |> not
            then
                "Player" :: names
            else
                names

        // if targetable enemies are different now, tell neuro
        if names <> lastNames then
            let act = g.Action SetTargets

            act.MutateProp "targets" (fun x ->
                let x = x :?> NeuroFSharp.ArraySchema
                let x = x.Items :?> NeuroFSharp.StringSchema
                x.Enum <- Some(names |> Array.ofList))

            g.RegisterActions [ act ]

            let parenMsg =
                if names |> List.exists (String.exists ((=) '(')) then
                    " Numbers in () are used to distinguish duplicate names."
                else
                    ""

            g.Context
                true
                $"Targetable entities: {g.Serialize names}.{parenMsg} Your targets, in order of priority: {g.Serialize g.Targets}. Use the `set_targets` action to change the target list."

        lastNames <- names

(*[<HarmonyPatch(typeof<NewsItemObject>, nameof Unchecked.defaultof<NewsItemObject>.SetNewsItem)>]
    [<HarmonyPatch(typeof<CPlayerInfoSteam>, nameof Unchecked.defaultof<CPlayerInfoSteam>.GetDiseaseUnlocked)>]
    [<HarmonyPostfix>]
    static member public DiseaseUnlock(__result: bool byref) = __result <- true
    [<HarmonyPatch(typeof<CMainMenuScreen>, "ShowDynamicPopup")>]
    [<HarmonyPrefix>]
    static member public DontShowDynamicPopups() = false*)
