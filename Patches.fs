namespace HollowNeuro

open HarmonyLib
open HutongGames.PlayMaker
open System
open System.Reflection

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

    let mutable isPlayer = Option.None
    let mutable fsmInitialized = false
    let mutable heroBox: UnityEngine.GameObject = null
    let mutable lastNamesCtx = []
    let mutable lastSent = System.DateTime.Now.AddDays -1.
    let mutable lastHp: int option = Option.None
    let mutable lastCtx = ""
    let charmsToHide = [ 2; 40 ]

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
                Context.logger.LogError $"creating new sprite for {orig.name}"
                let origS = orig.gameObject.AddComponent<SpriteSwitch>()

                let clone =
                    (UnityEngine.Object.Instantiate<UnityEngine.GameObject> orig.gameObject)
                        .GetComponent<tk2dSpriteAnimation>()

                // it seems that this doesn't cause memory leaks because anim object persists across scenes (or something like that)
                UnityEngine.Object.DontDestroyOnLoad clone.gameObject
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

                                UnityEngine.Object.DontDestroyOnLoad coll.gameObject
                                //SceneManager.DontDestroyOnLoad coll.gameObject

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

        if isPlayer.IsSome = sw.isPlayer then orig else sw.other

    let neuroSaveSlotPath (slotIndex: int) =
        System.IO.Path.Combine(
            UnityEngine.Application.persistentDataPath,
            if slotIndex = 0 then
                "neuro.dat"
            else
                $"neuro{slotIndex}.dat"
        )

    let disableMap = false
    let disableBallTint = false
    let grimmRange = 7.81f
    let patchedGrimmRange = grimmRange * 2.f

    type SheetNameStore() =
        inherit UnityEngine.MonoBehaviour()

        [<DefaultValue>]
        val mutable public sheetName: string

    let createDefinitionForRegionInTexture
        (name: string)
        (textureDimensions: UnityEngine.Vector2)
        (uvRegion: UnityEngine.Rect)
        (trimRect: UnityEngine.Rect)
        (anchor: UnityEngine.Vector2)
        : tk2dSpriteDefinition =
        let scale = 1.0f / 64.0f

        let height = uvRegion.height
        let width = uvRegion.width
        let texX = textureDimensions.x
        let texY = textureDimensions.y

        let def = tk2dSpriteDefinition ()

        def.flipped <- tk2dSpriteDefinition.FlipMode.None
        def.extractRegion <- false
        def.name <- name
        def.colliderType <- tk2dSpriteDefinition.ColliderType.Unset

        let eps = UnityEngine.Vector2(0.001f, 0.001f)

        let v2 =
            UnityEngine.Vector2((uvRegion.x + eps.x) / texX, 1.0f - (uvRegion.y + uvRegion.height + eps.y) / texY)

        let v3 =
            UnityEngine.Vector2((uvRegion.x + uvRegion.width - eps.x) / texX, 1.0f - (uvRegion.y - eps.y) / texY)

        let offset =
            UnityEngine.Vector2(trimRect.x - anchor.x, 0.0f - trimRect.y + anchor.y)

        let offset = offset * scale

        let pos0 = UnityEngine.Vector3((0.0f - anchor.x) * scale, anchor.y * scale, 0.0f)

        let pos1 =
            pos0
            + UnityEngine.Vector3(trimRect.width * scale, (0.0f - trimRect.height) * scale, 0.0f)

        let r0 = UnityEngine.Vector3(0.0f, (0.0f - height) * scale, 0.0f)
        let r1 = r0 + UnityEngine.Vector3(width * scale, height * scale, 0.0f)

        def.positions <-
            [| UnityEngine.Vector3(r0.x + offset.x, r0.y + offset.y, 0.0f)
               UnityEngine.Vector3(r1.x + offset.x, r0.y + offset.y, 0.0f)
               UnityEngine.Vector3(r0.x + offset.x, r1.y + offset.y, 0.0f)
               UnityEngine.Vector3(r1.x + offset.x, r1.y + offset.y, 0.0f) |]

        def.uvs <-
            [| UnityEngine.Vector2(v2.x, v2.y)
               UnityEngine.Vector2(v3.x, v2.y)
               UnityEngine.Vector2(v2.x, v3.y)
               UnityEngine.Vector2(v3.x, v3.y) |]

        def.normals <- [||]
        def.tangents <- [||]
        def.indices <- [| 0; 3; 1; 2; 3; 0 |]

        let bMin = UnityEngine.Vector3(pos0.x, pos1.y, 0.0f)
        let bMax = UnityEngine.Vector3(pos1.x, pos0.y, 0.0f)

        def.boundsData <- [| (bMax + bMin) / 2.0f; bMax - bMin |]
        def.untrimmedBoundsData <- [| (bMax + bMin) / 2.0f; bMax - bMin |]
        def.texelSize <- UnityEngine.Vector2(scale, scale)

        def

[<HarmonyPatch>]
type public Patches() =
    static let mutable l10nPatched = false

    [<HarmonyPatch(typeof<tk2dSpriteCollectionData>, "Init")>]
    [<HarmonyPrefix>]
    static member public InitCln(__instance: tk2dSpriteCollectionData) =
        if __instance.gameObject.name = "HUD Cln" then
            __instance.materials <- [| __instance.materials[0]; UnityEngine.Material __instance.materials[0] |]
            __instance.textures <- [| __instance.textures[0]; MainClass.Instance.Tex |]
            __instance.materials[1].mainTexture <- MainClass.Instance.Tex

    [<HarmonyPatch(typeof<tk2dSpriteAnimator>,
                   nameof (Unchecked.defaultof<tk2dSpriteAnimator>.Play: string -> unit),
                   [| typeof<string> |])>]
    [<HarmonyPrefix>]
    static member public PlayTk2dAnim(__instance: tk2dSpriteAnimator, name: string) =
        if __instance.Library <> null then
            if __instance.Library.gameObject.name = "HUD Anim" then
                if Array.length __instance.Library.clips = 64 then
                    __instance.Library.clips <- Array.append __instance.Library.clips [| __instance.Library.clips[0] |]
                    let sc = __instance.Library.clips[0].frames[0].spriteCollection

                    let ts =
                        UnityEngine.Vector2(float32 sc.textures[1].width, float32 sc.textures[1].height)

                    let mutable startY = 0

                    [| "Blue Appear", 143, 31, 9
                       "Health Empty", 67, 59, 1
                       "Health Appear", 119, 130, 5
                       "Health Bound", 65, 70, 1
                       "Blue Break", 127, 124, 5
                       "Health Break", 127, 158, 6
                       "Health Idle", 65, 70, 45
                       "Blue Idle", 99, 149, 99
                       "Blue Break Fast", 127, 124, 5
                       "Health Refill", 119, 130, 6
                       "Health Max Up", 126, 117, 14 |]
                    |> Array.iter (fun (cname, cw, ch, clen) ->
                        //let hi = __instance.Library.clips |> Array.find (_.name >> (=) "Health Idle")
                        //let hiLen = 45
                        let c = __instance.Library.clips |> Array.find (_.name >> (=) cname)
                        let b = Array.length sc.spriteDefinitions

                        let cdefs =
                            Array.init clen (fun i ->
                                let def =
                                    createDefinitionForRegionInTexture
                                        $"{cname}_{i}"
                                        ts
                                        // uv
                                        (UnityEngine.Rect(float32 (cw * i), float32 startY, float32 cw, float32 ch))
                                        // trim rect
                                        (UnityEngine.Rect(0f, 0f, float32 cw, float32 ch))
                                        // anchor
                                        (UnityEngine.Vector2(float32 cw / 2f, float32 ch / 2f))

                                def.untrimmedBoundsData <-
                                    [| UnityEngine.Vector3(0f, -0.5f, 0f); UnityEngine.Vector3(126f, 167f, 0f) |]

                                def.materialId <- 1
                                def.material <- sc.materials[def.materialId]
                                def.materialInst <- sc.materialInsts[def.materialId]

                                def)

                        startY <- startY + ch
                        sc.spriteDefinitions <- Array.append sc.spriteDefinitions cdefs

                        c.frames |> Seq.iteri (fun i fr -> fr.spriteId <- b + i))

    [<HarmonyPatch(typeof<DialogueBox>, nameof (Unchecked.defaultof<DialogueBox>.SetConversation))>]
    [<HarmonyPostfix>]
    static member public RememberDialogueSheet(__instance: DialogueBox, sheetName: string) =
        // pages use some weird character indices, couldn't get it to work, show unpaginated
        let mesh = __instance.gameObject.GetComponent<TMPro.TextMeshPro>()
        MainClass.Instance.Game.ShowDialogue sheetName mesh.text

    [<HarmonyPatch(typeof<Actions.ListenForQuickMap>, nameof (Unchecked.defaultof<Actions.ListenForQuickMap>.OnUpdate))>]
    [<HarmonyPrefix>]
    static member public DisableMapKeyAction() = not disableMap

    [<HarmonyPatch(typeof<HeroController>, nameof (Unchecked.defaultof<HeroController>.CanQuickMap))>]
    [<HarmonyPostfix>]
    static member public DisableQuickMap(__result: bool byref) =
        if disableMap then
            __result <- false

    [<HarmonyPatch(typeof<CheatManager>, "IsCheatsEnabled", MethodType.Getter)>]
    [<HarmonyPostfix>]
    static member public EnableCheats(__result: bool byref) = __result <- true

    [<HarmonyPatch(typeof<HeroController>, nameof (Unchecked.defaultof<HeroController>.TakeDamage))>]
    [<HarmonyPrefix>]
    static member public Take0Damage
        (
            __instance: HeroController,
            go: UnityEngine.GameObject,
            damageSide: GlobalEnums.CollisionSide,
            damageAmount: int
        ) =
        if damageAmount <= 0 && go.name = "Enemy Damager" then
            // CanTakeDamage
            if
                __instance.damageMode <> GlobalEnums.DamageMode.NO_DAMAGE
                && __instance.transitionState = GlobalEnums.HeroTransitionState.WAITING_TO_TRANSITION
                && not __instance.cState.invulnerable
                && not __instance.cState.recoiling
                && not __instance.playerData.isInvincible
                && not __instance.cState.dead
                && not __instance.cState.hazardDeath
                && not BossSceneController.IsTransitioning
            then
                (typeof<HeroController>.GetMethod("CancelAttack", BindingFlags.NonPublic ||| BindingFlags.Instance))
                    .Invoke(__instance, [||])
                |> ignore

                if __instance.cState.wallSliding then
                    __instance.cState.wallSliding <- false
                    __instance.wallSlideVibrationPlayer.Stop() |> ignore

                if __instance.cState.touchingWall then
                    __instance.cState.touchingWall <- false

                if __instance.cState.recoilingLeft || __instance.cState.recoilingRight then
                    (typeof<HeroController>
                        .GetMethod("CancelRecoilHorizontal", BindingFlags.NonPublic ||| BindingFlags.Instance))
                        .Invoke(__instance, [||])
                    |> ignore

                if __instance.cState.bouncing || __instance.cState.shroomBouncing then
                    (typeof<HeroController>.GetMethod("CancelBounce", BindingFlags.NonPublic ||| BindingFlags.Instance))
                        .Invoke(__instance, [||])
                    |> ignore

                    let rb2d = __instance.GetComponent<UnityEngine.Rigidbody2D>()
                    rb2d.velocity <- UnityEngine.Vector2(rb2d.velocity.x, 0f)

                __instance.GetComponent<HeroAudioController>().PlaySound GlobalEnums.HeroSounds.TAKE_HIT

                if
                    __instance.cState.nailCharging
                    || typeof<HeroController>
                        .GetField("nailChargeTimer", BindingFlags.NonPublic ||| BindingFlags.Instance)
                        .GetValue
                           __instance
                       :?> float32
                       <> 0f
                then
                    __instance.cState.nailCharging <- false
                    // to reset nailChargeTimer
                    __instance.CanNailArt() |> ignore

                __instance.StartCoroutine(
                    typeof<HeroController>
                        .GetMethod("StartRecoil", BindingFlags.NonPublic ||| BindingFlags.Instance)
                        .Invoke(__instance, [| damageSide; true; damageAmount |])
                    :?> System.Collections.IEnumerator
                )
                |> ignore

    [<HarmonyPatch(typeof<PlayerData>, "SetupNewPlayerData")>]
    [<HarmonyPostfix>]
    static member public EnableGrimmChild(__instance: PlayerData) =
        __instance.grimmChildLevel <- 2
        __instance.charmCost_40 <- 0
        __instance.gotCharm_40 <- true
        __instance.equippedCharm_40 <- true
        __instance.newCharm_40 <- false
        __instance.charmsOwned <- 1
        // and compass
        __instance.charmCost_2 <- 0
        __instance.gotCharm_2 <- true
        __instance.equippedCharm_2 <- true
        __instance.newCharm_2 <- false
        __instance.charmsOwned <- 2

    // __instance.hasCharm <- true
    // __instance.EquipCharm 40

    [<HarmonyPatch(typeof<InvCharmBackboard>, "OnEnable")>]
    [<HarmonyPrefix>]
    static member public DontShowHiddenCharms(__instance: InvCharmBackboard) =
        charmsToHide
        |> List.iter (fun ch ->
            if __instance.gotCharmString = $"gotCharm_{ch}" then
                __instance.gotCharmString <- "")

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

    (*[<HarmonyPatch(typeof<global.Language.Language>,
                   nameof (global.Language.Language.Get: (string * string) -> string),
                   [| typeof<string>; typeof<string> |])>]
    [<HarmonyPrefix>]*)
    static member public LanguageGet(key: string, sheetTitle: string, __result: string byref) =
        match
            match key with
            | "BUTTON_DESC_HOLD_PATCHED" -> Some "Ask Evil to guide you through Hallownest."
            | "GET_MAP_2" -> Some "All of your collected map data, pins, and markers will be uploaded to her GPS."
            | "MARKER_B_DESC" ->
                Some
                    "These markers are new additions to the shop. Evil can use them to mark interesting spots on her map!<br><br>The colour is quite soothing, don't you think?"
            | "MARKER_Y_DESC" ->
                Some
                    "These markers are new additions to the shop. Evil can use them to mark interesting spots on her map!<br><br>This understated colour makes me think of hunting underground for treasure..."
            | "MARKER_W_DESC" ->
                Some
                    "These markers are new additions to the shop. Evil can use them to mark interesting spots on her map!<br><br>The material I used for this colour is quite rare, so it costs a little more."
            | "MARKER_R_DESC" ->
                Some
                    "These markers are new additions to the shop. Evil can use them to mark interesting spots on her map!<br><br>This colour could be used to remind you of hard-won battles."
            | "PIN_DESC_BENCH" ->
                Some
                    "These pins will mark benches and other rest spots on Evil's map. Useful if you're exhausted and just need to find somewhere to sit!"
            | "SHOP_DESC_QUILL" ->
                Some
                    "You'll need this if you want Evil's map to be updated with new areas as you explore.<br><br>It's essential for anyone serious about mapping!"
            | "PIN_DESC_GHOST" ->
                Some
                    "Cornifer told me he's found some interesting looking graves and shrines in the depths.<br><br>Evil can use these pins to mark down any interesting graves on her map. Go and pay your respects."
            | "PIN_DESC_DREAMPLANT" ->
                Some
                    "Cornifer has been telling me about these strange whispering roots he's been seeing.<br><br>I made some pins so Evil can record their locations herself."
            | "PIN_DESC_SHOP" ->
                Some
                    "Evil can use these pins to mark shopkeepers or any other interesting bugs you find on your travels.<br><br>Every so often, you should pass by and see how they're doing. I'm sure they'd like that."
            | "PIN_DESC_COCOON" ->
                Some
                    "Have you seen those beautiful blue cocoons? I made these pins so Evil can keep track of them.<br><br>The cocoons are pretty, but very delicate. Please be careful around them."
            | _ -> Option.None
        with
        | Some x ->
            __result <- x
            false
        | None -> true

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

        if not l10nPatched then
            l10nPatched <- true

            MainClass.Instance.Harmony.Patch(
                typeof<Language.Language>
                    .GetMethod(
                        nameof (global.Language.Language.Get: (string * string) -> string),
                        [| typeof<string>; typeof<string> |]
                    ),
                prefix = HarmonyMethod(typeof<Patches>.GetMethod "LanguageGet")
            )
            |> ignore

        match n, __instance.FsmName with
        | "First Map", _ ->
            let ch =
                Array.init
                    __instance.gameObject.transform.childCount
                    (__instance.gameObject.transform.GetChild >> _.gameObject)

            ch
            |> Array.tryFind (_.name >> (=) "Button")
            |> Option.filter _.activeSelf
            |> Option.iter (fun btn ->
                let gm3 = ch |> Array.find (_.name >> (=) "Text 3")
                let gm2 = ch |> Array.find (_.name >> (=) "Text 2")
                let holdText = ch |> Array.find (_.name >> (=) "Text Hold")
                UnityEngine.Object.Destroy btn
                UnityEngine.Object.Destroy gm3
                //UnityEngine.Object.Destroy gm2
                let t = holdText.GetComponent<SetTextMeshProGameText>()
                t.convName <- "BUTTON_DESC_HOLD_PATCHED")

        | "Heart Pieces", _ ->
            let p = __instance.gameObject

            if (p.transform.GetChild 0).localPosition.x > 0f then
                Array.init p.transform.childCount (p.transform.GetChild >> _.gameObject)
                |> Array.iter (fun p ->
                    p.transform.localPosition <- UnityEngine.Vector3(0f, 0f, -0.001f)
                    p.transform.localScale <- UnityEngine.Vector3(1f, 1f, 1.23f)

                    let n =
                        match p.name with
                        | "Pieces 1" -> 1
                        | "Pieces 2" -> 2
                        | "Pieces 3" -> 3
                        | "Pieces 4" -> 4
                        | _ -> 0

                    let s = p.GetComponent<UnityEngine.SpriteRenderer>()

                    let s' =
                        UnityEngine.Sprite.Create(
                            MainClass.Instance.Pieces[n],
                            UnityEngine.Rect(0f, 0f, 395f, 537f),
                            UnityEngine.Vector2(0.5f, 0.5f),
                            64f
                        )

                    s.sprite <- s')

                let s = p.GetComponent<UnityEngine.SpriteRenderer>()

                let s' =
                    UnityEngine.Sprite.Create(
                        MainClass.Instance.Pieces[0],
                        UnityEngine.Rect(0f, 0f, 395f, 537f),
                        UnityEngine.Vector2(0.5f, 0.5f),
                        64f
                    )

                s.sprite <- s'
                s.transform.localScale <- UnityEngine.Vector3(0.728f, 0.728f, 1f)
                s.transform.localPosition <- UnityEngine.Vector3(-7.3f, -3.536f, -1.22f)
                let b = s.GetComponent<UnityEngine.BoxCollider2D>()
                b.offset <- UnityEngine.Vector2(0.05f, -0.05f)

                b.size <-
                    UnityEngine.Vector2(
                        b.size.x * float32 (1.27 * 1.4 / 0.728),
                        b.size.y * float32 (0.86 * 1.4 / 0.728)
                    )

                s.GetComponents<SetPosIfPlayerdataBool>()
                |> Array.iter (fun x ->
                    x.XPos <- -7.7f

                    if PlayerData.instance.GetBool x.playerDataBool then
                        s.transform.localPosition <-
                            UnityEngine.Vector3(x.XPos, s.transform.localPosition.y, s.transform.localPosition.z))

        | "Inv", "UI Inventory" ->
            let a = (state "Any Other Panes?").Actions[1] :?> Actions.PlayerDataBoolTest
            a.isTrue <- a.isFalse
        | "Inv", "Button Control" ->
            let chk = state "Check Item"

            chk.Transitions <-
                chk.Transitions
                |> Array.filter (fun x ->
                    match x.FsmEvent.Name with
                    | "INV_NAME_MAP"
                    | "INV_NAME_MAPQUILL"
                    | "INV_NAME_QUILL" -> false
                    | _ -> true)
        | "Inventory", "Inventory Control" ->
            [ (state "Single Pane?").Actions[7]
              (state "Next Map").Actions[0]
              (state "Next Map 2").Actions[0]
              (state "Next Map 3").Actions[0] ]
            |> List.iter (fun x ->
                let x = x :?> Actions.PlayerDataBoolTest
                x.isTrue <- x.isFalse)
        | x, "charm_show_if_collected" when charmsToHide |> List.exists (_.ToString() >> (=) x) ->
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
                                    if
                                        boolCheck
                                        && not (
                                            charmsToHide |> List.exists (fun x -> a.boolName.Value.EndsWith $"_{x}")
                                        )
                                    then
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

                                   match isPlayer with
                                   | Some d ->
                                       fsm.GameObject.layer <- 3

                                       let dmg =
                                           fsm.GameObject.GetComponent<DamageHero>()
                                           |> Option.ofObj
                                           |> Option.defaultWith fsm.GameObject.AddComponent<DamageHero>

                                       dmg.damageDealt <- d
                                   | None ->
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
                                           Context.countBosses PlayerData.instance * 4 / 7 + 1

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
        // banker: act as if no grimmchild
        | "Banker", "Conversation Control" ->
            let g = state "Grimmchild?"
            let act = g.Actions[g.Actions.Length - 1] :?> Actions.PlayerDataBoolTest
            act.isTrue <- act.isFalse
        // brumm torch npc: act as if no grimmchild
        | "Brumm Torch NPC", "Conversation Control" ->
            let chk = state "Check Active"
            let act = chk.Actions[0] :?> Actions.PlayerDataBoolTest
            act.isTrue <- act.isFalse
        // queen npc: never read grimmchild state (keep at false)
        | "Queen", "Conversation Control" ->
            let chk = state "Convo Choice"

            if chk.Actions.Length = 18 then
                chk.Actions <- Array.removeAt 4 chk.Actions
        // flamebearer: act as if no grimmchild
        | "Flamebearer Spawn", "Spawn Control" ->
            let chk = state "State"
            let act = chk.Actions[4] :?> Actions.PlayerDataBoolTest
            act.isTrue <- act.isFalse
        // hopefully the above is enough to never enable the grimm troupe content
        | "Grimmchild", "Control" ->
            let range =
                Seq.init __instance.gameObject.transform.childCount __instance.gameObject.transform.GetChild
                |> Seq.find (_.name >> (=) "Enemy Range")

            range.GetComponent<UnityEngine.CircleCollider2D>().radius <- patchedGrimmRange
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
                    newState.Actions <- [| CustomWait(fun () -> if isPlayer.IsSome then 0.4f else 0.0f) |]
                    antic.Transitions[0].ToState <- "Pre-Shoot Delay"
                    antic.Transitions[0].ToFsmState <- newState

                // slow down the ball when firing at player
                do
                    let fireN = shoot.Actions |> Array.findIndex (fun x -> x :? Actions.FireAtTarget)

                    shoot.Actions[fireN] <-
                        CustomFire(
                            shoot.Actions[fireN] :?> Actions.FireAtTarget,
                            fun _ -> if isPlayer.IsSome then 0.4f else 1.0f
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

                    if not disableBallTint then
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

        let dist (x: UnityEngine.GameObject) =
            (__instance.transform.position - x.transform.position).magnitude

        let distFast (x: UnityEngine.GameObject) =
            (__instance.transform.position - x.transform.position).sqrMagnitude

        let inRange x =
            distFast x < grimmRange * grimmRange
            && UnityEngine.Physics2D.Linecast(
                __instance.transform.position,
                HeroController.instance.gameObject.transform.position,
                256
               )
               |> UnityEngine.RaycastHit2D.op_Implicit
               |> not

        let targets0 =
            let cmp f = fun x y -> compare (f x) (f y)

            __instance.enemyList
            |> List.ofSeq
            |> List.sortWith (
                match MainClass.Instance.Game.Tactic with
                | ShootNearestFirst -> cmp distFast
                | ShootFurthestFirst -> cmp (distFast >> (~-))
                | ShootLeastHpFirst -> cmp (_.GetComponent<HealthManager>() >> Option.ofObj >> Option.map _.hp)
                | ShootMostHpFirst -> cmp (_.GetComponent<HealthManager>() >> Option.ofObj >> Option.map (_.hp >> (~-)))
                | DontShoot -> fun _ _ -> 0
            )

        let targets =
            HeroController.instance.gameObject :: targets0
            |> List.map (fun x ->
                if x = HeroController.instance.gameObject then
                    { name = "Player"
                      distance = dist x / 2.0f
                      hp = Some PlayerData.instance.health
                      maxHp = Some PlayerData.instance.CurrentMaxHealth
                      inShootRange = inRange x
                      currentlyInvincible = false }
                else
                    let hm = x.GetComponent<HealthManager>() |> Option.ofObj

                    { name =
                        if x = HeroController.instance.gameObject then
                            "Player"
                        else
                            x.name
                      distance = dist x / 2.0f
                      hp = hm |> Option.map _.hp
                      maxHp = Option.None
                      inShootRange = inRange x
                      currentlyInvincible = hm |> Option.map _.IsInvincible |> Option.defaultValue false })

        let mutable shot = Option.None

        if
            inRange HeroController.instance.gameObject
            && (shot <- MainClass.Instance.Game.DequeuePlayerShot()
                shot.IsSome)
        then
            isPlayer <- shot |> Option.map (fun x -> if x then 1 else 0)

            __result <-
                if heroBox = null then
                    HeroController.instance.gameObject
                else
                    heroBox
        else if MainClass.Instance.Game.Tactic = DontShoot then
            __result <- null
        else
            __result <- null
            isPlayer <- Option.None
            __result <- targets0 |> List.tryFind inRange |> Option.defaultValue null

        let names = targets |> List.map _.name |> List.sort

        // if any *enemy* name not contained in old list
        // or if 5 seconds have passed
        let sendAnyway = (System.DateTime.Now - lastSent).TotalSeconds >= 5.

        if sendAnyway || names |> List.exists (fun x -> not (List.contains x lastNamesCtx)) then
            // if only distances changed, dont send even if sendAnyway is true
            let ctx = g.Serialize(targets |> List.map (fun x -> { x with distance = 0.0f }))

            if ctx <> lastCtx then
                lastCtx <- ctx

                let hpCtx =
                    if lastHp = Some PlayerData.instance.health then
                        ""
                    else
                        match lastHp with
                        | Some x when x = PlayerData.instance.health -> ""
                        | None -> ""
                        | Some x when PlayerData.instance.health = 0 -> $"The player has been dealt lethal damage! "
                        | Some x when PlayerData.instance.health > x ->
                            $"The player healed by {PlayerData.instance.health - x} hitpoints. "
                        | Some x when PlayerData.instance.health < x ->
                            $"The player took {x - PlayerData.instance.health} damage. "
                        | Some x -> raise (exn "unreachable")

                lastHp <- Some PlayerData.instance.health

                g.Context
                    true
                    $"{hpCtx}Entities around you: {g.Serialize targets}. Your current targeting tactic: {g.Serialize g.Tactic}. Use the `set_tactic` action to change the tactic to help or hinder the player."

                lastNamesCtx <- names

            lastSent <- System.DateTime.Now
