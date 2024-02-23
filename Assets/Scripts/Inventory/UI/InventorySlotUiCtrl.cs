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

    private void Awake()
    {
        _currentColor = slotBg.color;
        _currentSlotData = new UiData() { garbageType = GarbageManager.GarbageType.NONE };
    }

    public void SetDataInUi(UiData uiData)
    {
        _currentSlotData = uiData;
        if (!_isFilled){
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
        _currentSlotData = new UiData() { garbageType = GarbageManager.GarbageType.NONE};
        SlotSelection(false);
    }

    public void OnMouseDown()
    {
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
