using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUiCtrl : MonoBehaviour
{
    public struct UiData{
        public InventoryItemUiCtrl.UiData inventoryItemUiData;
        public GarbageManager.GarbageType garbageType;
    }

    [SerializeField]
    private InventoryItemUiCtrl inventoryItemPrefab;

    [SerializeField]
    private GarbageManager.GarbageType currentGarbageType = GarbageManager.GarbageType.NONE;

    private InventoryItemUiCtrl _currentInventoryItemUiCtrl;

    public void SetDataInUi(UiData uiData)
    {
        if(currentGarbageType!=uiData.garbageType){
            _currentInventoryItemUiCtrl= Instantiate(inventoryItemPrefab, transform);
            _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
            currentGarbageType = uiData.garbageType;
        }
        else{
            _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
        }
    }

}
