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

    private bool _isFilled = false;

    private InventoryItemUiCtrl _currentInventoryItemUiCtrl;

    public void SetDataInUi(UiData uiData)
    {
        currentGarbageType = uiData.garbageType;
        if(!_isFilled){
            _currentInventoryItemUiCtrl= Instantiate(inventoryItemPrefab, transform);
            _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
            _isFilled = true;
        }
        else{
            /*if (currentGarbageType == uiData.garbageType)
            {*/
                _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
            //}
        }
    }

    public void RemoveItemFromSlot()
    {
        _isFilled = false;
        if(_currentInventoryItemUiCtrl != null)
        {
            Destroy(_currentInventoryItemUiCtrl.gameObject);
        }
        currentGarbageType = GarbageManager.GarbageType.NONE;
    }

}
