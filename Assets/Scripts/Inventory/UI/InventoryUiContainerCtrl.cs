using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUiContainerCtrl : MonoBehaviour
{

    public struct UiData
    {
        public List<InventorySlotUiCtrl.UiData> inventorySlotsUiData;
    }

    [SerializeField]
    private List<InventorySlotUiCtrl> inventorySlotList;

    public void SetDataInUi(UiData uiData)
    {
        List<InventorySlotUiCtrl.UiData> inventoryData = uiData.inventorySlotsUiData;
        //Debug.Log(inventoryData.Count + "inventoryData.Count");
        for (int i = 0; i < inventoryData.Count; i++)
        {
            //Debug.Log(inventoryData[i].inventoryItemUiData.itemImage.name + "image");
            //Debug.Log(inventoryData[i].inventoryItemUiData.count + "count");
            inventorySlotList[i].SetDataInUi(inventoryData[i]);
        }
    }
}
