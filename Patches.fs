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

[<AutoOpen>]
module Stuff =
    let findState name (x: PlayMakerFSM) =
        x.FsmStates |> Array.find (_.Name >> (=) name)

    let mutable isPlayer = false
    let mutable fsmInitialized = false
    let mutable heroBox: UnityEngine.GameObject = null
    let mutable lastNames = []
    let mutable lastNamesCtx = []
    // names that are reserved for specific entities
    // (so labels aren't transient but have some persistence)
    let mutable reservedNames: ((string * int) * UnityEngine.GameObject) list = []

    type SpriteSwitch() =
        inherit UnityEngine.MonoBehaviour()

        [<DefaultValue>]
        val mutable other: tk2dSpriteAnimation

        [<DefaultValue>]
        val mutable isPlayer: bool

    let pickSpriteLib (orig: tk2dSpriteAnimation) =
        let sw =
            orig.GetComponent<SpriteSwitch>() :> obj
            |> Option.ofObj
            |> Option.map (fun x -> x :?> SpriteSwitch)
            |> Option.defaultWith (fun () ->
                let origS = orig.gameObject.AddComponent<SpriteSwitch>()

                let clone =
                    (UnityEngine.Object.Instantiate<UnityEngine.GameObject> orig.gameObject)
                        .GetComponent<tk2dSpriteAnimation>()

                let cloneS = clone.gameObject.GetComponent<SpriteSwitch>()
                origS.other <- clone
                cloneS.other <- orig
                cloneS.isPlayer <- true

                let mutable coll: tk2dSpriteCollectionData = null

                clone.clips <-
                    clone.clips
                    |> Array.map (fun clip ->
                        let clip = tk2dSpriteAnimationClip clip

                        clip.frames
                        |> Array.iter (fun frame ->
                            if coll = null then
                                coll <-
                                    (UnityEngine.Object.Instantiate<UnityEngine.GameObject>
                                        frame.spriteCollection.gameObject)
                                        .GetComponent<tk2dSpriteCollectionData>()

                                // do this thing to avoid manually calling Init (private)
                                // (not sure how Init is even supposed to be called since it's seemingly not a Monobehaviour thing)
                                coll.needMaterialInstance <- false
                                coll.materials <- coll.materials |> Array.map UnityEngine.Material
                                coll.materialInsts <- coll.materials
                                coll.Transient <- true

                                coll.materials
                                |> Seq.iter (fun mat -> mat.color <- UnityEngine.Color(0.5f, 0.5f, 1.0f))

                                coll.spriteDefinitions
                                |> Array.iter (fun d ->
                                    d.material <- coll.materials[d.materialId]
                                    d.materialInst <- coll.materialInsts[d.materialId])

                            frame.spriteCollection <- coll)

                        clip)

                origS)

        if isPlayer = sw.isPlayer then orig else sw.other

    let neuroSaveSlotPath (slotIndex: int) =
        System.IO.Path.Combine(
            UnityEngine.Application.persistentDataPath,
            if slotIndex = 0 then
                "neuro.dat"
            else
                $"neuro{slotIndex}.dat"
        )

    let labelTargets (targets: UnityEngine.GameObject list) =
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

    let countBosses (pd: PlayerData) =
        // seems like one boss is missing? whatever don't care
        [ pd.killedInfectedKnight
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
          ]
        |> List.map (fun x -> if x then 1 else 0)
        |> List.sum

    type SheetNameStore() =
        inherit UnityEngine.MonoBehaviour()

        [<DefaultValue>]
        val mutable public sheetName: string

[<HarmonyPatch>]
type public Patches() =
    [<HarmonyPatch(typeof<DialogueBox>, nameof (Unchecked.defaultof<DialogueBox>.SetConversation))>]
    [<HarmonyPostfix>]
    static member public RememberDialogueSheet(__instance: DialogueBox, sheetName: string) =
        // pages use some weird character indices, couldn't get it to work, show unpaginated
        let mesh = __instance.gameObject.GetComponent<TMPro.TextMeshPro>()
        MainClass.Instance.Game.ShowDialogue sheetName mesh.text

    [<HarmonyPatch(typeof<Actions.ListenForQuickMap>, nameof (Unchecked.defaultof<Actions.ListenForQuickMap>.OnUpdate))>]
    [<HarmonyPrefix>]
    static member public DisableMapKeyAction() = false

    [<HarmonyPatch(typeof<HeroController>, nameof (Unchecked.defaultof<HeroController>.CanQuickMap))>]
    [<HarmonyPostfix>]
    static member public DisableQuickMap(__result: bool byref) = __result <- false

    [<HarmonyPatch(typeof<CheatManager>, "IsCheatsEnabled", MethodType.Getter)>]
    [<HarmonyPostfix>]
    static member public EnableCheats(__result: bool byref) = __result <- true

    [<HarmonyPatch(typeof<PlayerData>, "SetupNewPlayerData")>]
    [<HarmonyPostfix>]
    static member public EnableGrimmChild(__instance: PlayerData) =
        __instance.charmCost_40 <- 0
        __instance.gotCharm_40 <- true
        __instance.equippedCharm_40 <- true
        __instance.newCharm_40 <- false
        __instance.charmsOwned <- 1
    // __instance.hasCharm <- true
    // __instance.EquipCharm 40

    [<HarmonyPatch(typeof<InvCharmBackboard>, "OnEnable")>]
    [<HarmonyPrefix>]
    static member public DontShowCharm40(__instance: InvCharmBackboard) =
        if __instance.gotCharmString = "gotCharm_40" then
            __instance.gotCharmString <- ""

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

            let sp = findState "Spawn Pause" sg
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
        let state s = findState s __instance
        let n = __instance.gameObject.name

        let n =
            if n.EndsWith "(Clone)" then
                n.Substring(0, n.Length - "(Clone)".Length)
            else
                n

        match n, __instance.FsmName with
        | "Inv", "UI Inventory" ->
            let a = (state "Any Other Panes?").Actions[1] :?> Actions.PlayerDataBoolTest
            a.isTrue <- a.isFalse
        | "Inventory", "Inventory Control" ->
            [ (state "Single Pane?").Actions[7]
              (state "Next Map").Actions[0]
              (state "Next Map 2").Actions[0]
              (state "Next Map 3").Actions[0]
              ]
            |> List.iter (fun x ->
                let x = x :?> Actions.PlayerDataBoolTest
                x.isTrue <- x.isFalse)
        | "40", "charm_show_if_collected" ->
            let chk = state "Check"

            match chk.Actions[chk.Actions.Length - 1] with
            | :? Actions.PlayerDataBoolTest as a ->
                chk.Actions[chk.Actions.Length - 1] <-
                    FsmLambda(fun fsm ->
                        let ownerDefaultTarget = fsm.GetOwnerDefaultTarget a.gameObject

                        if ownerDefaultTarget <> null then
                            let comp = ownerDefaultTarget.GetComponent<GameManager>()

                            if comp <> null then
                                let boolCheck = comp.GetPlayerDataBool a.boolName.Value

                                fsm.Event(
                                    if boolCheck && not (a.boolName.Value.EndsWith "_40") then
                                        a.isTrue
                                    else
                                        a.isFalse
                                ))
            | _ -> ()
        | "Charm Effects", "Spawn Grimmchild" ->
            // wait for 0.25s to give the old grimmchild instance time to despawn
            let sp = state "Spawn Pause"
            sp.Actions[0].Enabled <- true
        | "Enemy Damager", "Attack" ->
            if
                __instance.FsmVariables.BoolVariables.Length = 1
                && __instance.FsmVariables.FloatVariables.Length = 1
            then
                // set layer depending on isPlayer
                do
                    let detect = state "Detect"

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
                                       // just set the damage value instead
                                       // default lv4 is 11 damage
                                       // likely final boss count is 20-30
                                       // divide by 2 = end up at 10-15
                                       // divide by 1.75 = end up at 11-17
                                       // divide by 1.5 = end up at 13-20
                                       // 1.5 seems excessive, 2 is fine but perhaps just a tiny bit conservative?
                                       // try 1.75 (7/4) for now
                                       (fsm.Variables.FindFsmInt "Damage").Value <-
                                           countBosses PlayerData.instance * 4 / 7 + 1

                                       fsm.GameObject.layer <- 15) |]
                            detect.Actions

                    detect.Actions[0].Init detect
        // never set nightmareLanterAppeared to true
        | "Sycophant Dream", "Activate Lantern" ->
            let init = state "Init"
            let act = init.Actions[init.Actions.Length - 1] :?> Actions.PlayerDataBoolTest
            act.isFalse <- act.isTrue
        // never set nightmareLanterLit to true
        | "grimm_brazier", "grimm_brazier" ->
            let init = state "Init"
            let act = init.Actions[init.Actions.Length - 1] :?> Actions.PlayerDataBoolTest
            act.isFalse <- act.isTrue
        // hopefully the above is enough to never enable the grimm troupe content
        | "Grimmchild", "Control" ->
            let shoot = state "Shoot"

            if shoot.Actions.Length = 10 then
                let change = state "Change"
                let antic = state "Antic"
                let chk = state "Check For Target"

                do
                    // treat lv1 as lv2 for target checking purposes
                    let cmp = chk.Actions[0] :?> Actions.IntCompare
                    cmp.equal <- cmp.greaterThan

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

                    let setBallColor =
                        FsmLambda(fun fsm ->
                            let ball = fsm.Variables.FindFsmGameObject "Flameball"
                            let t = ball.Value.GetComponent<tk2dSpriteAnimator>()

                            t.Library <- pickSpriteLib t.Library

                            Seq.init ball.Value.transform.childCount ball.Value.transform.GetChild
                            |> Seq.iter (fun child ->
                                let t = child.gameObject.GetComponent<tk2dSpriteAnimator>()

                                if t <> null then
                                    t.Library <- pickSpriteLib t.Library))

                    shoot.Actions[fireN].Init shoot
                    setBallColor.Init shoot
                    shoot.Actions <- shoot.Actions |> Array.insertAt fireN setBallColor

                // increase follow distance by ~2 times
                // (for player safety)
                do
                    let offx = change.Actions[1] :?> Actions.SetFloatValue
                    let offy = change.Actions[4] :?> Actions.RandomFloat
                    let mult (x: FsmFloat) = x.Value <- x.Value * 1.4f
                    mult offx.floatValue
                    mult offy.min
                    mult offy.max
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

        let names0 = List.sort (List.map fst targets)

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
                "Player" :: names0
            else
                names0

        // if targetable enemies are different now, tell neuro
        if names <> lastNames then
            let act = g.Action SetTargets

            act.MutateProp "targets" (fun x ->
                let x = x :?> NeuroFSharp.ArraySchema
                let x = x.Items :?> NeuroFSharp.StringSchema
                x.Enum <- Some(names |> Array.ofList))

            g.RegisterActions [ act ]

            lastNames <- names

        if names <> lastNamesCtx then
            if not (List.isEmpty names0) then
                let parenMsg =
                    if names |> List.exists (String.exists ((=) '(')) then
                        " Numbers in () are used to distinguish duplicate names."
                    else
                        ""

                g.Context
                    true
                    $"Entities around you: {g.Serialize names}.{parenMsg} Your targets, in order of priority: {g.Serialize g.Targets}. Use the `set_targets` action to change the target list."

            lastNamesCtx <- names
