using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUiCtrl : MonoBehaviour
{
    public struct UiData{
        public InventoryItemUiCtrl.UiData inventoryItemUiData;
        public GarbageManager.GarbageType garbageType;
    }

    [SerializeField]
    private InventoryItemUiCtrl inventoryItemPrefab;

    private UiData _currentSlotData;
    public UiData CurrentSlotData {  get { return _currentSlotData; } }

    [SerializeField]
    private Image slotBg;

    private bool _isFilled = false;

    private InventoryItemUiCtrl _currentInventoryItemUiCtrl;

    private Color _currentColor;

    private bool _isSelected = false;
    public bool IsSelected
    {
        get => _isSelected;
    }


    private void Start()
    {
        _currentColor = new Color(199, 226, 236, 255);
        slotBg.color = _currentColor;
    }

    public bool SetDataInUi(UiData uiData)
    {
        if (!_isFilled){
            _currentSlotData = uiData;
            _currentInventoryItemUiCtrl= Instantiate(inventoryItemPrefab, transform);
            _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
            _isFilled = true;
            return true;
        }
        else{
            if (_currentSlotData.garbageType == uiData.garbageType)
            {
                if(_currentSlotData.inventoryItemUiData.count != uiData.inventoryItemUiData.count)
                {
                    _currentSlotData = uiData;
                    _currentInventoryItemUiCtrl.SetDataInUi(uiData.inventoryItemUiData);
                }
                return true;
            }
        }
        return false;
    }

    public void RemoveItemFromSlot()
    {
        _isFilled = false;
        if(_currentInventoryItemUiCtrl != null)
        {
            Destroy(_currentInventoryItemUiCtrl.gameObject);
        }
        _currentSlotData = new UiData() { garbageType = GarbageManager.GarbageType.NONE};
        SlotSelection(false);
    }

    public void OnMouseDown()
    {
        Debug.Log(_isFilled + "_isFilled");
        Debug.Log(_isFilled + "_isFilled");
        if (_isFilled && _currentSlotData .garbageType!= GarbageManager.GarbageType.NONE)
        {
            SlotSelection(!_isSelected);
        }
    }

    private void SlotSelection(bool isSelected) {
        _isSelected = isSelected;
        if (isSelected)
        {
            slotBg.color = Color.green;
        }
        else
        {
            slotBg.color = _currentColor;
        }
    }

}
