using System;
using LocalizationContent;
using TheSTAR.Utility;
using TheSTAR.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApparatSlot : MonoBehaviour
{
    private const string English = "en";
    private const string Russian = "ru";
    private const string Turkish = "tr";
    
    [SerializeField] private GameObject _soonPanel;
    [SerializeField] private Image iconImg;
    [SerializeField] private PointerButton buyButton;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject buyGroup;
    [SerializeField] private GameObject ownedGroup;

    [Space]
    [SerializeField] private GameObject availableContainer;
    [SerializeField] private TextMeshProUGUI neededLevelText;

    private int index;
    private Action<int> onBuyClickAction;

    public int Index => index;

    public void Init(
        int index, 
        string nameText, 
        Sprite icon, 
        DollarValue cost, 
        Action<int> onBuyClickAction, 
        ScrollRect scrollRect)
    {
        this.index = index;
        this.nameText.text = nameText;
        this.iconImg.sprite = icon;
        costText.text = TextUtility.FormatPrice(cost);
        this.onBuyClickAction = onBuyClickAction;

        buyButton.Init(OnBuyButtonClick);
        buyButton.SetScrollable(scrollRect);
    }

    public void SetVisual(bool owned)
    {
        buyGroup.SetActive(!owned);
        ownedGroup.SetActive(owned);
    }

    private void OnBuyButtonClick()
    {
        onBuyClickAction?.Invoke(index);
    }

    public void SetValueSoonPanel(bool value)
    {
        _soonPanel.SetActive(value);
    }

    public void SetLocked(int neededLevel)
    {
        availableContainer.SetActive(false);
        neededLevelText.gameObject.SetActive(true);
        
        var localization = Localization.Instance.GetCurrentLanguage();

        neededLevelText.text = localization switch
        {
            English => $"Level {neededLevel + 1} is required",
            Turkish => $"Seviye {neededLevel + 1} gereklidir",
            Russian => $"УРОВЕНЬ {neededLevel + 1} требуется",
            _ => $"Level {neededLevel + 1} is required"
        };
        
        // neededLevelText.text = $"Level {neededLevel + 1} is required";
    }

    public void SetUnlocked()
    {
        availableContainer.SetActive(true);
        neededLevelText.gameObject.SetActive(false);
    }
}