namespace HollowNeuro

open HarmonyLib
open HutongGames.PlayMaker

type CustomFire(fire: Actions.FireAtTarget, getSpeedScale: CustomFire -> float32) =
    inherit Actions.RigidBody2dActionBase()
    let mutable self: FsmGameObject = null
    let mutable speedScale = 1.0f

    override _.Awake() = base.Fsm.HandleFixedUpdate <- true

    override _.OnPreprocess() = base.Fsm.HandleFixedUpdate <- true

    override this.OnEnter() =
        let obj = base.Fsm.GetOwnerDefaultTarget fire.gameObject
        self <- FsmGameObject.op_Implicit obj
        this.CacheRigidBody2d obj
        speedScale <- getSpeedScale this
        this.DoSetVelocity()

        if not fire.everyFrame then
            this.Finish()

    override this.OnFixedUpdate() =
        this.DoSetVelocity()

        if not fire.everyFrame then
            this.Finish()

    member private this.DoSetVelocity() =
        if not (isNull this.rb2d) then
            let dy =
                fire.target.Value.transform.position.y + fire.position.Value.y
                - self.Value.transform.position.y

            let dx =
                fire.target.Value.transform.position.x + fire.position.Value.x
                - self.Value.transform.position.x

            let angle =
                atan2 dy dx * float32 (180.0 / System.Math.PI)
                + if fire.spread.IsNone then
                      0f
                  else
                      UnityEngine.Random.Range(-fire.spread.Value, fire.spread.Value)

            let rad = angle * float32 (System.Math.PI / 180.0)

            let x = fire.speed.Value * speedScale * cos rad
            let y = fire.speed.Value * speedScale * sin rad

            this.rb2d.velocity <- UnityEngine.Vector2(x, y)

type CustomWait(time: unit -> float32) =
    inherit FsmStateAction()

    let mutable timer = 0.0f

    override _.OnEnter() =
        timer <- time ()
        MainClass.Instance.Logger.LogInfo $"waiting for {timer}"

        if timer <= 0f then
            base.Finish()

    override _.OnUpdate() =
        timer <- timer - UnityEngine.Time.deltaTime

        if timer <= 0f then
            MainClass.Instance.Logger.LogInfo $"finished waiting"
            base.Finish()


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
                // set layer depending on isPlayer
                do
                    let detect = __instance.FsmStates |> Array.find (_.Name >> (=) "Detect")

                    detect.Actions <-
                        Array.append
                            [| FsmLambda(fun fsm ->
                                   MainClass.Instance.Logger.LogInfo "in damager"

                                   if isPlayer then
                                       fsm.GameObject.layer <- 3

                                       let dmg =
                                           fsm.GameObject.GetComponent<DamageHero>()
                                           |> Option.ofObj
                                           |> Option.defaultWith fsm.GameObject.AddComponent<DamageHero>

                                       dmg.damageDealt <- 1
                                   else
                                       // have to restore layer back in case this is a reused ball
                                       // no need to remove DamageHero as the ball wont collide with the player either way (surely)
                                       fsm.GameObject.layer <- 15) |]
                            detect.Actions

                    detect.Actions[0].Init detect
        | "Grimmchild(Clone)", "Control" ->
            let shoot = __instance.FsmStates |> Array.find (_.Name >> (=) "Shoot")

            if shoot.Actions.Length = 10 then
                let change = __instance.FsmStates |> Array.find (_.Name >> (=) "Change")
                let antic = __instance.FsmStates |> Array.find (_.Name >> (=) "Antic")
                // give player more time to react to the sound
                // (the animation stops so dont make the delay too big)
                do
                    let newState = FsmState __instance.Fsm
                    newState.Name <- "Pre-Shoot Delay"
                    newState.Transitions <- [| FsmTransition() |]
                    newState.Transitions[0].ToState <- "Shoot"
                    newState.Transitions[0].ToFsmState <- shoot
                    newState.Transitions[0].FsmEvent <- FsmEvent.GetFsmEvent "FINISHED"
                    newState.Actions <- [| CustomWait(fun () -> if isPlayer then 0.4f else 0.0f) |]
                    antic.Transitions[0].ToState <- "Pre-Shoot Delay"
                    antic.Transitions[0].ToFsmState <- newState

                // slow down the ball when firing at player
                do
                    let fireN = shoot.Actions |> Array.findIndex (fun x -> x :? Actions.FireAtTarget)

                    shoot.Actions[fireN] <-
                        CustomFire(
                            shoot.Actions[fireN] :?> Actions.FireAtTarget,
                            fun _ -> if isPlayer then 0.4f else 1.0f
                        )

                    shoot.Actions[fireN].Init shoot

                let offx = change.Actions[1] :?> Actions.SetFloatValue
                let offy = change.Actions[4] :?> Actions.RandomFloat
                let mult (x: FsmFloat) = x.Value <- x.Value * 1.5f
                mult offx.floatValue
                mult offy.min
                mult offy.max
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
