namespace HollowNeuro

open HarmonyLib
open HutongGames.PlayMaker

type CustomTrigger2D(act: Actions.Trigger2dEvent) =
    inherit FsmStateAction()
    // let mutable proxy: PlayMakerUnity2DProxy = null

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

    override this.OnEnter() : unit =
        if (this.Fsm.Variables.FindFsmBool "isPlayer").Value then
            base.Owner.layer <- 3

            let dmg =
                base.Owner.GetComponent<DamageHero>()
                |> Option.ofObj
                |> Option.defaultWith base.Owner.AddComponent<DamageHero>

            dmg.damageDealt <- 1
        else
            // have to restore layer back in case this is a reused ball
            // no need to remove DamageHero as the ball wont collide with the player either way (surely)
            base.Owner.layer <- 15

        act.OnEnter()

    override _.OnExit() : unit = act.OnExit()

type FsmLambda(f: Fsm -> unit) =
    inherit FsmStateAction()

    override this.OnEnter() : unit =
        f this.Fsm
        this.Finish()

module Util =
    let stateByName name (x: FsmState array) = x |> Array.find (_.Name >> (=) name)

[<HarmonyPatch>]
type public Patches() =
    static let mutable isPlayer = false
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

    [<HarmonyPatch(typeof<HeroBox>, "Start")>]
    [<HarmonyPrefix>]
    static member public HeroBoxStart(__instance: HeroBox) =
        heroBox <- __instance.gameObject
        Printf.ksprintf MainClass.Instance.Logger.LogInfo "hblayer %d" __instance.gameObject.layer

    [<HarmonyPatch(typeof<HeroBox>, "OnTriggerEnter2D")>]
    [<HarmonyPrefix>]
    static member public TriggerEnter(__instance: HeroBox, otherCollider: UnityEngine.Collider2D) =
        MainClass.Instance.Logger.LogInfo $"theroenter {otherCollider.gameObject.name}"

    [<HarmonyPatch(typeof<Fsm>, nameof (Unchecked.defaultof<Fsm>.ProcessEvent))>]
    [<HarmonyPrefix>]
    static member public Proc(__instance: Fsm, fsmEvent: FsmEvent) =
        if __instance.Active && not (FsmEvent.IsNullOrEmpty fsmEvent) then
            //MainClass.Instance.Logger.LogInfo $"{__instance.Owner.name}/{__instance.Name}: processing {fsmEvent.Name}"
            ()

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

            callback <-
                System.Action<bool>(fun success ->
                    if success then
                        try
                            let path = neuroSaveSlotPath saveSlot
                            let pathTmp = path + ".new"
                            let pathBak = path + ".bak"
                            let data = MainClass.Instance.Game.SaveData()
                            System.IO.File.WriteAllText(pathTmp, data)

                            if System.IO.File.Exists path then
                                System.IO.File.Replace(pathTmp, path, pathBak)
                            else
                                System.IO.File.Move(pathTmp, path)
                        with exc ->
                            UnityEngine.Debug.LogException exc

                    if cb <> null then
                        cb.Invoke success)

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
                detect.Actions <- Array.concat [| detect.Actions |]
                inv.Actions <- Array.concat [| inv.Actions |]

                MainClass.Instance.Logger.LogInfo "added bool var"
        | "Grimmchild(Clone)", "Control" ->
            let shoot = __instance.FsmStates |> Array.find (_.Name >> (=) "Shoot")

            if shoot.Actions.Length = 10 then
                let n = shoot.Actions |> Array.findIndex (fun x -> x :? Actions.SetFsmInt)

                let setIsPlayer =
                    FsmLambda(fun fsm ->
                        let damager = (fsm.Variables.FindFsmGameObject "Damager").Value
                        let fsm = damager.LocateMyFSM "Attack"
                        (fsm.FsmVariables.FindFsmBool "isPlayer").Value <- isPlayer)

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
                isPlayer <- true

                if heroBox = null then
                    HeroController.instance.gameObject
                else
                    heroBox
            | x :: xs ->
                match List.tryFind (fst >> (=) x) targets with
                | Some x ->
                    g.Targets <- t
                    isPlayer <- false
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
                $"Entities around you: {g.Serialize names}.{parenMsg} Your targets, in order of priority: {g.Serialize g.Targets}. Use the `set_targets` action to change the target list."

        lastNames <- names
