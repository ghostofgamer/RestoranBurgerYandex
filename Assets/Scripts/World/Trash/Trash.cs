using UnityEngine;
using Zenject;

public class Trash : MonoBehaviour
{
    [SerializeField] private TouchInteractive touchInteractive;
    [SerializeField] private TutorInWorldFocus tutorFocus;

    public TutorInWorldFocus TutorFocus => tutorFocus;

    private GameWorldInteraction gameWorldInteraction;

    [Inject]
    private void Construct(GameWorldInteraction gameWorldInteraction)
    {
        this.gameWorldInteraction = gameWorldInteraction;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        touchInteractive.OnClickEvent += () =>
        {
            gameWorldInteraction.OnTrashClick(this);
        };
    }

    private void OnCollisionEnter(Collision other)
    {
        var trashDestroyable = other.gameObject.GetComponent<TrashDestroyable>();
        if (!trashDestroyable) return;

        trashDestroyable.OnEnterTrash(this);
    }
}