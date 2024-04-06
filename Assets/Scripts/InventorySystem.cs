using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public struct InventoryItemData
    {
        public GarbageManager.GarbageType garbageType;
        public int count;
    }

    protected Dictionary<string, InventoryItemData> itemsData = new Dictionary<string, InventoryItemData>();

    public virtual void AddItem(InventoryItemData itemData)
    {
            string enumString = itemData.garbageType.ToString();

            ManageItemAdditionToInventory(enumString, itemData);
    }

    private void ManageItemAdditionToInventory(string enumString, InventoryItemData itemData)
    {
        if (!itemsData.ContainsKey(enumString))
        {
            itemsData[enumString] = itemData;
        }
        else
        {
            InventoryItemData inventoryItemData = itemsData[enumString];
            inventoryItemData.count += itemData.count;
            itemsData[enumString] = inventoryItemData;
        }
    }

    public virtual void RemoveItem(InventoryItemData itemData)
    {
        string enumString = itemData.garbageType.ToString();
        ManageItemRemovalToInventory(enumString, itemData);
    }

    private void ManageItemRemovalToInventory(string enumString, InventoryItemData itemData)
    {
        if (itemsData.ContainsKey(enumString))
        {
            InventoryItemData inventoryItemData = itemsData[enumString];
            inventoryItemData.count -= itemData.count;
            itemsData[enumString] = inventoryItemData;

            if (inventoryItemData.count <= 0)
            {
                itemsData.Remove(enumString);
            }
        }
    }

    public void RemoveSingleItemOfType(GarbageManager.GarbageType garbageType)
    {
        string garbageTypeString = garbageType.ToString();
        if (itemsData.ContainsKey(garbageTypeString))
        {
            InventoryItemData inventoryItemData = itemsData[garbageTypeString];
            inventoryItemData.count -= 1;
            itemsData[garbageTypeString] = inventoryItemData;

            if (inventoryItemData.count <= 0)
            {
                itemsData.Remove(garbageTypeString);
            }
        }
    }


    public Dictionary<string, InventoryItemData> GetInventoryItemsData()
    {
        return itemsData;
    }
}
