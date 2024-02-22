using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public struct InventoryItemData
    {
        public ContainerCtrl containerCtrl;
        public GarbageCtrl garbageCtrl;
        public int count;
    }

    protected Dictionary<string, InventoryItemData> itemsData = new Dictionary<string, InventoryItemData>();

    public virtual void AddItem(InventoryItemData itemData)
    {
        if (itemData.garbageCtrl != null)
        {
            string enumString = itemData.garbageCtrl.GarbageType.ToString();

            ManageItemAdditionToInventory(enumString, itemData);
        }
        else if(itemData.containerCtrl != null)
        {
            ManageItemAdditionToInventory(GarbageManager.GarbageType.NONE.ToString(), itemData);
        }
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
        if (itemData.garbageCtrl != null)
        {
            string enumString = itemData.garbageCtrl.GarbageType.ToString();
            ManageItemRemovalToInventory(enumString, itemData);
        }
        else if (itemData.containerCtrl != null)
        {
            ManageItemRemovalToInventory(GarbageManager.GarbageType.NONE.ToString(), itemData);
        }
    }

    private void ManageItemRemovalToInventory(string enumString, InventoryItemData itemData)
    {
        if (itemsData.ContainsKey(enumString))
        {
            InventoryItemData inventoryItemData = itemsData[enumString];
            inventoryItemData.count -= itemData.count;

            if (inventoryItemData.count <= 0)
            {
                itemsData.Remove(enumString);
            }
        }
    }


    public Dictionary<string, InventoryItemData> GetInventoryItemsData()
    {
        return itemsData;
    }
}
