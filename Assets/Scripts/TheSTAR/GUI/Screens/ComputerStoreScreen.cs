using System.Collections.Generic;
using Configs;
using TheSTAR.Data;
using TheSTAR.Sound;
using TheSTAR.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TheSTAR.GUI
{
    public class ComputerStoreScreen : GuiScreen
    {
        [SerializeField] private PointerButton closeButton;
        [SerializeField] private PointerButton clearButton;
        [SerializeField] private PointerButton buyButton;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private Transform cartItemsParent;
        [SerializeField] private ComputerProductUI slotPrefab;
        [SerializeField] private CartElementSlot cartElementSlotPrefab;
        [SerializeField] private TextMeshProUGUI totalItemsCostText;
        [SerializeField] private PointerButton manageBtn;
        [SerializeField] private ScrollRect scroller;
        [SerializeField] private ScrollRect resultScroller;
        [SerializeField] private GameObject comingSoonObject;

        [Header("Sections")]
        [SerializeField] private PointerButton productsBtn_Active;
        [SerializeField] private PointerButton productsBtn_Inactive;
        [SerializeField] private PointerButton decorationsBtn_Active;
        [SerializeField] private PointerButton decorationsBtn_Inactive;
        
        private ConfigHelper<ItemsConfig> itemsConfig = new();
        private Dictionary<ItemType, int> shoppingCart;

        private int totalItemsInCart;
        private DollarValue totalItemsCost;
        private ComputerStoreSectionType currentSectionType = default;
        private List<ComputerProductUI> createdItemSlots = new();
        private List<CartElementSlot> createdCartItemsList = new();
        
        private const int MaxItemsInCartLimit = 99;

        private GuiController gui;
        private XpController xp;
        private Delivery delivery;
        private TutorialController tutorial;
        private SoundController sounds;

        [Inject]
        private void Consruct(
            GuiController gui,
            DataController data, 
            GameController game,
            XpController xp,
            Delivery delivery,
            TutorialController tutorial,
            SoundController sounds)
        {
            this.gui = gui;
            this.xp = xp;
            this.delivery = delivery;
            this.tutorial = tutorial;
            this.sounds = sounds;
        }

        public override void Init()
        {
            base.Init();

            closeButton.Init(() =>
            {
                gui.ShowMainScreen();
            });

            productsBtn_Inactive.Init(() =>
            {
                if (currentSectionType == ComputerStoreSectionType.Products) return;
                productsBtn_Active.gameObject.SetActive(true);
                productsBtn_Inactive.gameObject.SetActive(false);
                decorationsBtn_Active.gameObject.SetActive(false);
                decorationsBtn_Inactive.gameObject.SetActive(true);
                LoadItemSlots(ComputerStoreSectionType.Products);
            });

            decorationsBtn_Inactive.Init(() =>
            {
                productsBtn_Active.gameObject.SetActive(false);
                productsBtn_Inactive.gameObject.SetActive(true);
                decorationsBtn_Active.gameObject.SetActive(true);
                decorationsBtn_Inactive.gameObject.SetActive(false);
                if (currentSectionType == ComputerStoreSectionType.Decorations) return;
                LoadItemSlots(ComputerStoreSectionType.Decorations);
            });

            clearButton.Init(OnClearClick);
            buyButton.Init(OnBuyClick);
            manageBtn.Init(() =>
            {
                gui.Show<ComputerManageScreen>();
            });
        }

        /// <summary>
        /// Подгружает предметы определённой секции в левую часть экрана
        /// </summary>
        private void LoadItemSlots(ComputerStoreSectionType sectionType)
        {
            int currentLevel = xp.CurrentLevel;

            ItemType[] productTypes = null;
            
            if (sectionType == ComputerStoreSectionType.Products)
            {
                productTypes = itemsConfig.Get.ProductItemTypes;
            }
            else if (sectionType == ComputerStoreSectionType.Decorations)
            {
                productTypes = itemsConfig.Get.DecorationItemTypes;
            }

            int slotIndex = 0;
            ComputerProductUI slot;

            for (; slotIndex < productTypes.Length; slotIndex++)
            {
                if (createdItemSlots.Count <= slotIndex)
                {
                    slot = Instantiate(slotPrefab, slotsParent);
                    createdItemSlots.Add(slot);
                }
                else
                {
                    slot = createdItemSlots[slotIndex];
                    slot.gameObject.SetActive(true);
                }

                var itemType = productTypes[slotIndex];
                slot.Init(sounds, itemType, OnGetClick, scroller);
                var itemData = itemsConfig.Get.Item(itemType);
                slot.SetVisual(itemData.MainData.Name, itemData.CostData.BuyCost, itemData.OtherData.BoxValue, itemData.MainData.IconSprite);

                if(itemType==ItemType.FrenchFriesFrozen||itemType==ItemType.FriesPackingPaper)
                    slot.SetValueSoonPanel(true);
                // Debug.Log(itemType);
                
                if (currentLevel >= itemData.XpData.NeededLevelForBuy) slot.SetUnlocked();
                else slot.SetLocked(itemData.XpData.NeededLevelForBuy);
            }

            // лишние слоты
            for (; slotIndex < createdItemSlots.Count; slotIndex++)
            {
                slot = createdItemSlots[slotIndex];
                slot.gameObject.SetActive(false);
            }

            comingSoonObject.SetActive(productTypes.Length == 0);

            currentSectionType = sectionType;
        }

        /// <summary>
        /// Обновляет список товаров, добавленых в карзину (отображается справа)
        /// </summary>
        [ContextMenu("UpdateCartList")]
        private void UpdateCartList()
        {
            int cartItemIndex = 0;
            CartElementSlot slot;

            foreach (var element in shoppingCart)
            {
                if (createdCartItemsList.Count <= cartItemIndex)
                {
                    slot = Instantiate(cartElementSlotPrefab, cartItemsParent);
                    createdCartItemsList.Add(slot);
                }
                else
                {
                    slot = createdCartItemsList[cartItemIndex];
                    slot.gameObject.SetActive(true);
                }

                var itemType = element.Key;
                var itemData = itemsConfig.Get.Item(itemType);
                slot.Init(sounds, itemType, OnChangeItemsCountFromResultList, resultScroller);
                slot.SetVisual(itemData.MainData.Name, itemData.CostData.BuyCost * (element.Value * itemData.OtherData.BoxValue), element.Value);

                cartItemIndex++;
            }

            // лишние слоты
            for (; cartItemIndex < createdCartItemsList.Count; cartItemIndex++)
            {
                slot = createdCartItemsList[cartItemIndex];
                slot.gameObject.SetActive(false);
            }
        }

        private void OnChangeItemsCountFromResultList(ItemType itemType, int value)
        {
            if (!shoppingCart.ContainsKey(itemType)) return;
            shoppingCart[itemType] = value;
            if (shoppingCart[itemType] <= 0) shoppingCart.Remove(itemType);

            UpdateVisualByShoppingCart();
        }

        protected override void OnShow()
        {
            base.OnShow();
            
            shoppingCart = new();

            LoadItemSlots(currentSectionType);
            UpdateVisualByShoppingCart();

            if (!tutorial.IsCompleted(TutorialType.FirstDelivery))
            {
                createdItemSlots[0].SetTutorial(true);
                createdItemSlots[1].SetTutorial(true);
                createdItemSlots[2].SetTutorial(true);
            }
            else
            {
                createdItemSlots[0].SetTutorial(false);
                createdItemSlots[1].SetTutorial(false);
                createdItemSlots[2].SetTutorial(false);
            }
        }

        private void OnGetClick(ItemType itemType, int boxesCount)
        {
            if (totalItemsInCart >= MaxItemsInCartLimit) return;
            if (totalItemsInCart + boxesCount > MaxItemsInCartLimit) boxesCount = MaxItemsInCartLimit - totalItemsInCart;

            ArrayUtility.AddValue(shoppingCart, itemType, boxesCount);
            UpdateVisualByShoppingCart();
        }

        private void UpdateVisualByShoppingCart()
        {
            totalItemsInCart = 0;
            totalItemsCost = new();

            foreach (var element in shoppingCart)
            {
                totalItemsInCart += element.Value;

                var itemData = itemsConfig.Get.Item(element.Key);
                totalItemsCost += itemData.CostData.BuyCost * (itemData.OtherData.BoxValue * element.Value);
            }

            //itemsInCartCountText.text = totalItemsInCart.ToString();
            totalItemsCostText.text = TextUtility.FormatPrice(totalItemsCost, true); // TextUtility.NumericValueToText(totalItemsCost, NumericTextFormatType.DollarPriceCompactInt);
        
            UpdateCartList();
        }

        private void OnClearClick()
        {
            if (shoppingCart.Count <= 0) return;

            shoppingCart.Clear();
            UpdateVisualByShoppingCart();
        }
        
        private void OnBuyClick()
        {
            if (shoppingCart.Count <= 0) return;
            
            delivery.TryBuyProductsForDelivery(shoppingCart);
        }
    }
}

public enum ComputerStoreSectionType
{
    Products,
    Decorations
}