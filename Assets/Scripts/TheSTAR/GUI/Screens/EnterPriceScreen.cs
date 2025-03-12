using System;
using Configs;
using LocalizationContent;
using TheSTAR.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TheSTAR.GUI
{
    public class EnterPriceScreen : GuiScreen
    {
        private const string English = "en";
        private const string Russian = "ru";
        private const string Turkish = "tr";
        
        [SerializeField] private PointerButton acceptButton;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI profitText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI marketPriceText;
        [SerializeField] private Scrollbar scrollbar;

        [SerializeField] private Color greenColor;
        [SerializeField] private Color redColor;
        [SerializeField] private Color _recomendationColor;
        
        private ItemType itemType;
        private DollarValue currentPrice;
        private ItemData itemData;
        private Action<ItemType, DollarValue> acceptAction;
        private int minSaleCostSimple;
        private int maxSaleCostSimple;
        // private DollarValue _maxRecomendationPrice;

        private GuiController gui;

        private readonly ConfigHelper<ItemsConfig> itemsConfig = new();

        [Inject]
        private void Construct(GuiController gui)
        {
            this.gui = gui;
        }

        public override void Init()
        {
            base.Init();

            acceptButton.Init(OnAcceptClick);

            scrollbar.onValueChanged.AddListener((value) =>
            {
                currentPrice = new((int)MathUtility.ProgressToValue(value, minSaleCostSimple, maxSaleCostSimple));
                DisplayCurrentPrice();
            });
        }

        public void Init(ItemType itemType, DollarValue currentPrice, Action<ItemType, DollarValue> acceptAction)
        {
            var localization = Localization.Instance.GetCurrentLanguage();
            
            this.itemType = itemType;
            this.currentPrice = currentPrice;
            this.acceptAction = acceptAction;

            itemData = itemsConfig.Get.Item(itemType);
            

            // DollarValue recommendedPrice = itemData.CostData.SaleCostRec;
            /*int totalCents = itemData.CostData.SaleCostRec.dollars * 100 + itemData.CostData.SaleCostRec.cents;
            totalCents = (int)(totalCents * 1.15f);
            _maxRecomendationPrice = new DollarValue
            {
                dollars = totalCents / 100,
                cents = totalCents % 100
            };*/
            
            icon.sprite = itemData.MainData.IconSprite;
            // nameText.text = itemData.MainData.Name;
            
            switch (localization)
            {
                case English:
                    costText.text = $"Cost: {TextUtility.FormatPrice(itemData.CostData.BuyCost)}";
                    marketPriceText.text = $"Recommended: {TextUtility.FormatPrice(itemData.CostData.SaleCostRec)}";
                    nameText.text = itemData.MainData.Name;
                    break;

                case Turkish:
                    costText.text = $"Maliyet: {TextUtility.FormatPrice(itemData.CostData.BuyCost)}";
                    marketPriceText.text = $"Önermek: {TextUtility.FormatPrice(itemData.CostData.SaleCostRec)}";
                    nameText.text = itemData.MainData.TurName;
                    break;

                case Russian:
                    costText.text = $"Расходы: {TextUtility.FormatPrice(itemData.CostData.BuyCost)}";
                    marketPriceText.text = $"Рекомендуется: {TextUtility.FormatPrice(itemData.CostData.SaleCostRec)}";
                    nameText.text = itemData.MainData.RusName;
                    break;

                default:
                    costText.text = $"Cost: {TextUtility.FormatPrice(itemData.CostData.BuyCost)}";
                    marketPriceText.text = $"Recommended: {TextUtility.FormatPrice(itemData.CostData.SaleCostRec)}";
                    nameText.text = itemData.MainData.Name;
                    break;
            }
            
            
            /*costText.text = $"Cost: {TextUtility.FormatPrice(itemData.CostData.BuyCost)}";
            marketPriceText.text = $"Recommended: {TextUtility.FormatPrice(itemData.CostData.SaleCostRec)}";*/

            minSaleCostSimple = itemData.CostData.SaleCostMin.ToSimpleValue();
            maxSaleCostSimple = itemData.CostData.SaleCostMax.ToSimpleValue();
            int current = currentPrice.ToSimpleValue();
            float progress = MathUtility.GetProgress(current, minSaleCostSimple, maxSaleCostSimple);
            scrollbar.SetValueWithoutNotify(progress);

            DisplayCurrentPrice();
        }

        private void DisplayCurrentPrice()
        {
            var localization = Localization.Instance.GetCurrentLanguage();
            
            if (!Application.isMobilePlatform)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;  
            }
            
            Debug.Log("Display");

            priceText.text = localization switch
            {
                English => $"Price: {TextUtility.FormatPrice(currentPrice)}",
                Turkish => $"Fiyat: {TextUtility.FormatPrice(currentPrice)}",
                Russian => $"Цена: {TextUtility.FormatPrice(currentPrice)}",
                _ => $"Price: {TextUtility.FormatPrice(currentPrice)}"
            };

            // priceText.text = $"Price: {TextUtility.FormatPrice(currentPrice)}";

            if (currentPrice > itemData.CostData.SellCostMaxRecommendation)
            {
                priceText.color = redColor;
            }
            else
            {
                priceText.color = _recomendationColor;
            }
            
            if (currentPrice >= itemData.CostData.BuyCost)
            {
                var profit = currentPrice - itemData.CostData.BuyCost;
                
                profitText.text = localization switch
                {
                    English => $"Profit: {profit}",
                    Turkish => $"Kâr: {profit}",
                    Russian => $"Выгода: {profit}",
                    _ => $"Profit: {profit}"
                };
                
                // profitText.text = $"Profit: {profit}";
                profitText.color = greenColor;
            }
            else
            {
                var antiProfit = itemData.CostData.BuyCost - currentPrice;
                
                profitText.text = localization switch
                {
                    English => $"Profit: {antiProfit}",
                    Turkish => $"Kâr: {antiProfit}",
                    Russian => $"Выгода: {antiProfit}",
                    _ => $"Profit: {antiProfit}"
                };
                // profitText.text = $"Profit: -{antiProfit}";
                profitText.color = redColor;
            }
        }

        private void OnAcceptClick()
        {
            if (!Application.isMobilePlatform)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
            acceptAction?.Invoke(itemType, currentPrice);
            Debug.Log("ТИП " + itemType);
            gui.ShowMainScreen();
        }
    }
}