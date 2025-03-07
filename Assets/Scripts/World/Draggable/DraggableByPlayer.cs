using UnityEngine;
using Zenject;

public class DraggableByPlayer : Draggable
{
    [SerializeField] private TouchInteractive touchInteractive;
    [SerializeField] private Rigidbody rigidBody;

    private GameWorldInteraction gameWorldInteraction;
    [SerializeField] private bool _isLoadVersion;

    [Inject]
    private void Construct(GameWorldInteraction gameWorldInteraction)
    {
        this.gameWorldInteraction = gameWorldInteraction;
    }

    private void Start()
    {
        Init();
    }

    public void InitGameWorld(GameWorldInteraction gameWorldInteraction)
    {
        this.gameWorldInteraction = gameWorldInteraction;
    }

    public void InitGameWorldInteraction(GameWorldInteraction gameWorldInteraction)
    {
        this.gameWorldInteraction = gameWorldInteraction;
    }

    private void Init()
    {
        touchInteractive.OnClickEvent += () =>
        {
            gameWorldInteraction.OnClickToDraggable(this);
        };
    }

    public override void OnStartDrag(Dragger dragger, bool autoChangeInteractable)
    {
        base.OnStartDrag(dragger, autoChangeInteractable);
        rigidBody.isKinematic = true;

        if (autoChangeInteractable) touchInteractive.SetInteractable(false);
    }

    public override void OnEndDrag(bool autoChangeInteractable)
    {
        base.OnEndDrag(autoChangeInteractable);

        rigidBody.isKinematic = false;
        if (autoChangeInteractable) touchInteractive.SetInteractable(true);
    }

    public override void Throw(Vector3 force)
    {
        rigidBody.AddForce(force, ForceMode.Impulse);
    }
}