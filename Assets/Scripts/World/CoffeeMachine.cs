using UnityEngine;
using Zenject;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private TouchInteractive touchInteractive;
    [SerializeField] private Filler filler;
    [SerializeField] private Transform coffeeFillTran;
    [SerializeField] private TutorInWorldFocus tutorFocus;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private ItemsHandler[] _itemsHandlers;
    
    private GameWorldInteraction gameWorldInteraction;
    private ItemsController _itemsController;
    
    public Filler Filler => filler;
    public TutorInWorldFocus TutorFocus => tutorFocus;

    [Inject]
    private void Consruct(ItemsController items,GameWorldInteraction gameWorldInteraction)
    {
        this.gameWorldInteraction = gameWorldInteraction;
        _itemsController = items;
        _itemsController.CoffeeChanged += ReturnPosition;
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {                
        touchInteractive.OnClickEvent += () =>
        {
            Debug.Log("COFFFEEEEEE");
            gameWorldInteraction.OnClickCoffeeMachine(this);
        };

        filler.OnChangeFillPercentEvent += (percent) =>
        {
            Debug.Log("FILLER");
            coffeeFillTran.localScale = new Vector3(1, percent, 1);  
        };
    }

    private void ReturnPosition(Item item )
    {
        if (item.GetComponent<EmbeddableItem>().Dragger.CurrentDraggable == null)
        {
            Debug.Log("НООООЛЬ");
            return;
        }

        Item newItem = item.GetComponent<EmbeddableItem>().Dragger.CurrentDraggable.GetComponent<Item>();
        
        if (newItem != null)
        {
            Debug.Log("Не НОЛ");
            newItem.transform.parent = transform;
            newItem.GetComponent<Rigidbody>().isKinematic = true;

            var productSection = ItemSectionType.Cup;
        
            foreach (var itemsHandler in _itemsHandlers)
            {
                Debug.Log("МЕСТо");
                if (itemsHandler.HavePlace(newItem.ItemType, out var availablePlace))
                {
                    Debug.Log("МЕСТо есть");
                    availablePlace.StartDrag(newItem.Draggable);
                    return; 
                }
            }
        }
        
        /*item.transform.parent = transform;
        item.GetComponent<Rigidbody>().isKinematic = true;

        var productSection = ItemSectionType.Cup;
        
        foreach (var itemsHandler in _itemsHandlers)
        {
            if (itemsHandler.HavePlace(item.ItemType, out var availablePlace))
            {
                availablePlace.StartDrag(item.Draggable);
                return; 
            }
        }*/
        
        
        
        
        /*
        item.transform.parent = transform;
        item.GetComponent<Rigidbody>().isKinematic = true;
        
        if (!_itemsHandler.HavePlace(item.ItemType, out var availablePlace)) return;

        var productSection = ItemSectionType.Cup;
        availablePlace.StartDrag(item.Draggable);
        /*if (_itemsHandler.CanPlaceBySectionType(productSection))
        {
            availablePlace.StartDrag(item.Draggable);
            return;
        }#1#
        
        
        // gameWorldInteraction.TryPlaceItemToItemsHandler(_itemsHandler);
        Debug.Log("Бокалы надо вернуть ");*/
    }
}