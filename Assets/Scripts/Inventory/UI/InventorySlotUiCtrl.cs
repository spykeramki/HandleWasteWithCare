using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the parent of each inventory item.
public class InventorySlotUiCtrl : MonoBehaviour
{
    //class data
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
    public bool IsFilled
    {
        get { return _isFilled; }
    }

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

    //setter function
    public bool SetDataInUi(UiData uiData)
    {
        //if the slot is already filled with the same garbage type we just increase number
        //else we create new element with count.
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

    //Removes Item in Inventory UI slot
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

    //In Machine UI we can select the item with this method
    public void OnMouseDown()
    {
        if (_isFilled && _currentSlotData .garbageType!= GarbageManager.GarbageType.NONE)
        {
            SlotSelection(!_isSelected);
        }
    }

    //when slot is selected, it turns green. Else it will be default color
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
