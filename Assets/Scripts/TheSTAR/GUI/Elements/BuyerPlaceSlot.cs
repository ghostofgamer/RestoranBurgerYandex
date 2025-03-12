using System;
using LocalizationContent;
using TheSTAR.Utility;
using TheSTAR.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyerPlaceSlot : MonoBehaviour
{
    private const string English = "en";
    private const string Russian = "ru";
    private const string Turkish = "tr";

    [SerializeField] private Image iconImg;
    [SerializeField] private PointerButton buyButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject buyGroup;
    [SerializeField] private GameObject ownedGroup;
    [SerializeField] private GameObject _hand;

    [Space] [SerializeField] private GameObject availableByExpandContainer;
    [SerializeField] private GameObject availableByLevelContainer;
    [SerializeField] private TextMeshProUGUI neededLevelText;
    [SerializeField] private TextMeshProUGUI neededExpandText;

    [Space] [SerializeField] private TextMeshProUGUI placesText;

    private int index;
    private BuyerPlaceType buyerPlaceType;
    private Action<BuyerPlaceType, int> onBuyClickAction;

    public int Index => index;
    public BuyerPlaceType BuyerPlaceType => buyerPlaceType;

    public void Init(
        int index,
        BuyerPlaceType buyerPlaceType,
        string nameText,
        Sprite icon,
        DollarValue cost,
        Action<BuyerPlaceType, int> onBuyClickAction,
        ScrollRect scrollRect,
        int placesCount)
    {
        this.index = index;
        this.buyerPlaceType = buyerPlaceType;
        this.nameText.text = nameText;
        this.iconImg.sprite = icon;
        costText.text = TextUtility.FormatPrice(cost);
        this.onBuyClickAction = onBuyClickAction;
        placesText.text = placesCount.ToString();

        buyButton.Init(OnBuyButtonClick);
        buyButton.SetScrollable(scrollRect);
    }

    public void SetVisual(bool owned)
    {
        buyGroup.SetActive(!owned);
        ownedGroup.SetActive(owned);
    }

    public void SetActiveHandValue(bool value)
    {
        _hand.SetActive(value);
    }

    private void OnBuyButtonClick()
    {
        onBuyClickAction?.Invoke(buyerPlaceType, index);
    }

    public void SetLockedByLevel(int neededLevelIndex)
    {
        availableByLevelContainer.SetActive(false);
        neededLevelText.gameObject.SetActive(true);

        var localization = Localization.Instance.GetCurrentLanguage();

        neededLevelText.text = localization switch
        {
            English => $"Level {neededLevelIndex + 1} is required",
            Turkish => $"Seviye {neededLevelIndex + 1} gereklidir",
            Russian => $"УРОВЕНЬ {neededLevelIndex + 1} требуется",
            _ => $"Level {neededLevelIndex + 1} is required"
        };
    }

    public void SetUnlockedByLevel()
    {
        availableByLevelContainer.SetActive(true);
        neededLevelText.gameObject.SetActive(false);
    }

    public void SetLockedByExpand(int neededExpandCount)
    {
        availableByExpandContainer.SetActive(false);
        neededExpandText.gameObject.SetActive(true);
        
        var localization = Localization.Instance.GetCurrentLanguage();

        neededLevelText.text = localization switch
        {
            English => $"Zone {neededExpandCount} is required",
            Turkish => $"Alan {neededExpandCount} gereklidir",
            Russian => $"Зона {neededExpandCount} требуется",
            _ => $"Zone {neededExpandCount} is required"
        };
        
        // neededExpandText.text = $"Zone {neededExpandCount} is required";
    }

    public void SetUnlockedByExpand()
    {
        availableByExpandContainer.SetActive(true);
        neededExpandText.gameObject.SetActive(false);
    }
}