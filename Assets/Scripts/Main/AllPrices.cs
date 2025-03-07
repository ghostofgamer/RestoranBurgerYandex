using System.Collections.Generic;
using TheSTAR.Utility;
using Configs;
using Zenject;
using TheSTAR.Data;
using System;
using UnityEngine;

// должен просто хранить все цены и предоставлять логику для переназначения цен, обновление же на ценниках можно сделать через подписки на ивенты
public class AllPrices
{
    private ConfigHelper<ItemsConfig> itemsConfig = new();

    private Dictionary<ItemType, DollarValue> prices = new();

    private DataController data;

    private event Action<ItemType, DollarValue> OnSetPriceEvent;

    [Inject]
    private void Consruct(DataController data)
    {
        this.data = data;
    }

    public void Save()
    {
        data.gameData.levelData.prices = prices;
        data.Save(DataSectionType.Level);
    }

    public void Init()
    {
        // load
Debug.Log("1");
        var allItemTypes = EnumUtility.GetValues<ItemType>();
        
        Debug.Log("3");
        
        if (data.gameData.levelData.prices.Count == 0)
        {
            Debug.Log("5");
            foreach (var itemType in allItemTypes)
            {
                var itemData = itemsConfig.Get.Item(itemType);
                prices.Add(itemType, itemData.CostData.SaleCostMin);
            }

            Save();
        }
        else
        {
            Debug.Log("6");
            prices = data.gameData.levelData.prices;
        }

        // init update cost
        Debug.Log("7");
        /*foreach (var itemType in allItemTypes)
        {
            Debug.Log("8");
            SetPrice(itemType, prices[itemType]);
        }*/
        Debug.Log("10");
    }

    public void SetPrice(ItemType itemType, DollarValue price)
    {
        if (!prices.ContainsKey(itemType)) prices.Add(itemType, price);
        else prices[itemType] = price;

        OnSetPriceEvent?.Invoke(itemType, price);

        Save();
    }

    public DollarValue GetPrice(ItemType itemType)
    {
        var itemData = itemsConfig.Get.Item(itemType);
        if (prices.ContainsKey(itemType))
        {
            if (prices[itemType] < itemData.CostData.SaleCostMin || prices[itemType] > itemData.CostData.SaleCostMax) prices[itemType] = itemData.CostData.SaleCostMin;
            return prices[itemType];
        }
        else
        {
            prices.Add(itemType, itemData.CostData.SaleCostMin);
            return itemData.CostData.SaleCostMin;
        }
    }

    public void SubscribeToSetPrice(Action<ItemType, DollarValue> onSetPriceAction)
    {
        OnSetPriceEvent += onSetPriceAction;
        
        foreach (var element in prices)
        {
            onSetPriceAction?.Invoke(element.Key, GetPrice(element.Key));
        }
    }
}