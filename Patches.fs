namespace HollowNeuro

open System.Reflection
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

[<HarmonyPatch>]
type public Patches() =
    static let mutable fsmInitialized = false

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

            // make grimmchild spawn on game start
            // this works by enabling the Wait action which delays spawn by 0.25s
            // this gives time for grimmchild to despawn if already spawned (which happens upon LEVEL LOADED)
            // if not already despawned, the spawn check runs before grimmchild despawns and prevents spawn
            // (yes this is confusing and took literal days to figure out)
            let sg =
                FSMUtility.LocateFSM(__instance.transform.Find("Charm Effects").gameObject, "Spawn Grimmchild")

            let sp = sg.FsmStates |> Seq.find (_.Name >> (=) "Spawn Pause")
            sp.Actions[0].Enabled <- true

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


    // [<HarmonyPatch(typeof<WaitForHeroInPosition>, nameof Unchecked.defaultof<WaitForHeroInPosition>.OnEnter)>]
    // [<HarmonyPrefix>]
    // static member public Waaaaaa(__instance: WaitForHeroInPosition) =
    //     MainClass.Instance.Logger.LogInfo $"already in position? {HeroController.instance.isHeroInPosition}"

    //     if HeroController.instance <> null && HeroController.instance.isHeroInPosition then
    //         __instance.Fsm.Event __instance.sendEvent

    [<HarmonyPatch(typeof<Fsm>, nameof (Unchecked.defaultof<Fsm>.ProcessEvent))>]
    [<HarmonyPrefix>]
    static member public Proc(__instance: Fsm, fsmEvent: FsmEvent, eventData: FsmEventData) =
        if __instance.Active && not (FsmEvent.IsNullOrEmpty fsmEvent) then
            // MainClass.Instance.Logger.LogInfo $"{__instance.Owner.name}/{__instance.Name}: processing {fsmEvent.Name}"
            ()

    [<HarmonyPatch(typeof<GameMap>, nameof (Unchecked.defaultof<GameMap>.SetupMap))>]
    [<HarmonyPrefix>]
    static member public SetupMap(__instance: GameMap) =
        let inst = __instance

        let pd = PlayerData.instance

        Array.init inst.transform.childCount inst.transform.GetChild
        |> Array.iter (fun x ->
            Array.init x.gameObject.transform.childCount x.gameObject.transform.GetChild
            |> Array.iter (fun y ->
                if pd.scenesMapped.Contains y.gameObject.name || true then
                    MainClass.Instance.Logger.LogInfo $"scene {y.gameObject.name}"

                    Array.init y.gameObject.transform.childCount y.gameObject.transform.GetChild
                    |> Array.iter (fun z ->
                        MainClass.Instance.Logger.LogInfo $"- {z.gameObject.name}"

                        if y.gameObject.name = "Grub Pins" then
                            let w1, w2 = Context.grubPin
                            MainClass.Instance.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"
                        else
                            Context.pinMap
                            |> Map.tryFind z.gameObject.name
                            |> Option.iter (fun (w1, w2) ->
                                MainClass.Instance.Logger.LogInfo $"= ({w1 z.gameObject}) {w2 z.gameObject}"))
                else
                    MainClass.Instance.Logger.LogInfo $"skipping scene {y.gameObject.name}"))

    [<HarmonyPatch(typeof<FsmLog>, "AddEntry")>]
    [<HarmonyPrefix>]
    static member public LogFsm(entry: FsmLogEntry, sendToUnityLog: bool) =
        //absMainClass.Instance.Logger.LogInfo "a"
        //entry.DebugLog()
        ()

(*[<HarmonyPatch(typeof<NewsItemObject>, nameof Unchecked.defaultof<NewsItemObject>.SetNewsItem)>]
    [<HarmonyPatch(typeof<CPlayerInfoSteam>, nameof Unchecked.defaultof<CPlayerInfoSteam>.GetDiseaseUnlocked)>]
    [<HarmonyPostfix>]
    static member public DiseaseUnlock(__result: bool byref) = __result <- true
    [<HarmonyPatch(typeof<CMainMenuScreen>, "ShowDynamicPopup")>]
    [<HarmonyPrefix>]
    static member public DontShowDynamicPopups() = false*)
