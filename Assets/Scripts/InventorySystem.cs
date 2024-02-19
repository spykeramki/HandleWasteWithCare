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

    protected Dictionary<string, InventoryItemData> itemsData;

    public virtual void AddItem(InventoryItemData itemData)
    {
        if (itemData.garbageCtrl != null)
        {
            string enumString = itemData.garbageCtrl.GarbageType.ToString();

            ManageItemAdditionToInventory(enumString, itemData);
        }
        else if(itemData.containerCtrl != null)
        {
            ManageItemAdditionToInventory("CONTAINER", itemData);
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
            ManageItemRemovalToInventory("CONTAINER", itemData);
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


    protected Dictionary<string, InventoryItemData> GetInventoryItemsData()
    {
        return itemsData;
    }
}
