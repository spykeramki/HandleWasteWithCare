using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the parent object with a set of inventory slots
public class InventoryUiContainerCtrl : MonoBehaviour
{
    //class data
    public struct UiData
    {
        public List<InventorySlotUiCtrl.UiData> inventorySlotsUiData;
    }

    [SerializeField]
    private List<InventorySlotUiCtrl> inventorySlotList;

    public List<InventorySlotUiCtrl> InventorySlotList
    {
        get { return inventorySlotList; }
    }

    //data setter
    public void SetDataInUi(UiData uiData)
    {
        List<InventorySlotUiCtrl.UiData> inventoryData = uiData.inventorySlotsUiData;
        for (int i = 0; i < inventoryData.Count; i++)
        {
            for(int j = 0; j < InventorySlotList.Count; j++)
            {
                bool isUiSet = inventorySlotList[j].SetDataInUi(inventoryData[i]);
                if (isUiSet)
                {
                    break;
                }
            }
        }
    }

    //removes all items from slots by iterating and removing each slot at a time
    public void RemoveAllItemsFromSlots()
    {
        for (int i = 0; i < InventorySlotList.Count; i++)
        {
            inventorySlotList[i].RemoveItemFromSlot();
        }
    }
}
