using Configs;
using LocalizationContent;
using TheSTAR.Utility;
using TMPro;
using UnityEngine;
using Zenject;

public class OrderInProcessElement : MonoBehaviour
{
    private const string English = "en";
    private const string Russian = "ru";
    private const string Turkish = "tr";
    
    [SerializeField] private TextMeshProUGUI orderNumberText;
    [SerializeField] private OrderItemElement firstItem;
    [SerializeField] private OrderItemElement secondItem;

    private ConfigHelper<ItemsConfig> itemsConfig = new();

    public void Set(ActiveOrderData orderData)
    {
        if (orderData.OrderData.Items.Length == 1)
        {
            Set(
                orderData.Place.Index, 
                itemsConfig.Get.Item(orderData.OrderData.Items[0].ItemType).mainData.IconSprite, 
                orderData.OrderData.Items[0].Value);
        }
        else if (orderData.OrderData.Items.Length >= 2)
        {
            Set(
                orderData.Place.Index, 
                itemsConfig.Get.Item(orderData.OrderData.Items[0].ItemType).mainData.IconSprite, 
                orderData.OrderData.Items[0].Value,
                itemsConfig.Get.Item(orderData.OrderData.Items[1].ItemType).mainData.IconSprite, 
                orderData.OrderData.Items[1].Value);
        }
    }

    public void Set(int index, Sprite firstOrderIcon, int firstOrderValue)
    {
        var localization =   Localization.Instance.GetCurrentLanguage();

        orderNumberText.text = localization switch
        {
            English => $"ORDER {index + 1}:",
            Turkish => $"EMİR {index + 1}:",
            Russian => $"ЗАКАЗ {index + 1}:",
            _ => $"ORDER {index + 1}:"
        };

        // orderNumberText.text = $"ORDER {index + 1}:";
        firstItem.Set(firstOrderIcon, firstOrderValue);
        this.secondItem.gameObject.SetActive(false);
    }

    public void Set(int index, Sprite firstOrderIcon, int firstOrderValue, Sprite secondOrderIcon, int secondOrderValue)
    {
        var localization =   Localization.Instance.GetCurrentLanguage();

        orderNumberText.text = localization switch
        {
            English => $"ORDER {index + 1}:",
            Turkish => $"EMİR {index + 1}:",
            Russian => $"ЗАКАЗ {index + 1}:",
            _ => $"ORDER {index + 1}:"
        };

        // orderNumberText.text = $"ORDER {index + 1}:";
        firstItem.Set(firstOrderIcon, firstOrderValue);
        this.secondItem.gameObject.SetActive(true);
        secondItem.Set(secondOrderIcon, secondOrderValue);
    }
}