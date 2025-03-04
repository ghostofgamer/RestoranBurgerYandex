using Ads;
using TheSTAR.GUI;
using UnityEngine;
using Zenject;

public class AssemblyScreen : GuiScreen
{
    [SerializeField] private PointerButton closeButton;
    [SerializeField] private AssemblyHintContainer assemblyHint;

    private GameController game;
    private GuiController gui;
    private CameraController cameras;
    private GameWorldInteraction worldInteraction;
    private TutorialController tutorial;
    private FullAd _fullAd;

    [Inject]
    private void Construct(GameController game, GuiController gui, CameraController cameras,
        TutorialController tutorial,FullAd fullAd)
    {
        this.game = game;
        this.gui = gui;
        this.cameras = cameras;
        this.tutorial = tutorial;
        _fullAd = fullAd;
    }

    public override void Init()
    {
        base.Init();

        closeButton.Init(OnCloseClick);
        assemblyHint.Init();
    }

    protected override void OnShow()
    {
        if (!Application.isMobilePlatform)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;  
        }
        
        base.OnShow();
        cameras.DeactivateRayVision();
        game.OnStartAssemblingFocus();
        gui.FindUniversalElement<LookAroundContainer>().SetClickOnlyMove(true);

        TriggerTutorial();
    }

    protected override void OnHide()
    {
        base.OnHide();

        if (tutorial.InTutorial) tutorial.BreakTutorial();
    }

    private void OnCloseClick()
    {
        if (!Application.isMobilePlatform)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
            _fullAd.Show();
#else
        Debug.Log("Full ad is not shown because this is not a web build.");
#endif
        
        cameras.TempFocus(null, true);
        cameras.ActivateRayVision();
        game.OnEndAssemblingFocus();
        gui.ShowMainScreen();
        gui.FindUniversalElement<LookAroundContainer>().SetClickOnlyMove(false);
    }

    private void TriggerTutorial()
    {
        if (tutorial.IsCompleted(TutorialType.TakeCutlet) && !tutorial.IsCompleted(TutorialType.AssemblyBurger))
        {
            tutorial.TryShowInWorld(TutorialType.AssemblyBurger, new TutorInWorldFocus[] { }, out _);
            return;
        }
    }
}