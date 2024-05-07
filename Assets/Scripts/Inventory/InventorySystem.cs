using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent class of inventory systems in the game.
//All the machine and player inventory systems inherit this class.
public class InventorySystem : MonoBehaviour
{

    public struct InventoryItemData
    {
        public GarbageManager.GarbageType garbageType;
        public int count;
    }

    protected Dictionary<string, InventoryItemData> itemsData = new Dictionary<string, InventoryItemData>();

    //Adds Item to dictionary
    public virtual void AddItem(InventoryItemData itemData)
    {
            string enumString = itemData.garbageType.ToString();

            ManageItemAdditionToInventory(enumString, itemData);
    }

    private void ManageItemAdditionToInventory(string enumString, InventoryItemData itemData)
    {
        //If the inventory already has item, then the count will be increased.
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

    //removes item from the dictionary
    public virtual void RemoveItem(InventoryItemData itemData)
    {
        string enumString = itemData.garbageType.ToString();
        ManageItemRemovalToInventory(enumString, itemData);
    }

    private void ManageItemRemovalToInventory(string enumString, InventoryItemData itemData)
    {
        //if item count is greater than 1 then the count will be reduced, else item will be removed
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

    //method to remove 1 item of single type at once.
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

    //gets total items data from dictionary
    public Dictionary<string, InventoryItemData> GetInventoryItemsData()
    {
        return itemsData;
    }
}
