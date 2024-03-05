using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MachineUiCtrl : MonoBehaviour
{
    [SerializeField]
    private InventoryUiContainerCtrl playerInventoryUiCtrl;

    [SerializeField]
    private InventoryUiContainerCtrl machineInventoryUiCtrl;

    [SerializeField]
    private InventorySystem inventorySystem;

    [SerializeField]
    private Button playerTransferButton;

    [SerializeField]
    private Button machineTransferButton;


    [SerializeField]
    private Button recycleButton;

    public bool isRecycler = false;

    private float _currentRecycleTime = 0;

    private InventorySlotUiCtrl.UiData _currentRecyclingSlotData;

    private void Start()
    {
        playerTransferButton.onClick.AddListener(OnClickTransferToMachineInventoryButton);
        machineTransferButton.onClick.AddListener(OnClickTransferFromMachineInventoryButton);
        recycleButton.onClick.AddListener(SetRecyclingProcess);
        recycleButton?.gameObject.SetActive( isRecycler ? true : false );

    }

    public void UpdateTotalUi()
    {
        playerInventoryUiCtrl.RemoveAllItemsFromSlots();
        machineInventoryUiCtrl.RemoveAllItemsFromSlots();
        Dictionary<string, InventorySystem.InventoryItemData> playerItemsData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData();
        Dictionary<string, InventorySystem.InventoryItemData> machineItemsData = inventorySystem.GetInventoryItemsData();

        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(playerItemsData));
        machineInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(machineItemsData));

    }

    public void UpdateDataInUi()
    {
        Dictionary<string, InventorySystem.InventoryItemData> playerItemsData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData();
        Dictionary<string, InventorySystem.InventoryItemData> machineItemsData = inventorySystem.GetInventoryItemsData();

        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(playerItemsData));
        machineInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(machineItemsData));
    }

    public void OnClickTransferToMachineInventoryButton()
    {
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = playerInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData playerItemData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //inventorySlotUiCtrl.RemoveItemFromSlot();

            PlayerCtrl.LocalInstance.PlayerInventory.RemoveItem(playerItemData);
            inventorySystem.AddItem(playerItemData);
        }

        if (inventorySlotUiCtrls.Count != 0) 
        {
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }

    public void OnClickTransferFromMachineInventoryButton()
    {
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = machineInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData machineItemData = inventorySystem.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //inventorySlotUiCtrl.RemoveItemFromSlot();

            inventorySystem.RemoveItem(machineItemData);
            PlayerCtrl.LocalInstance.PlayerInventory.AddItem(machineItemData);
        }

        if (inventorySlotUiCtrls.Count != 0)
        {
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }



    private void SetRecyclingProcess()
    {
        Debug.Log("recycle started");
        StopAllCoroutines();
        _currentRecyclingSlotData = machineInventoryUiCtrl.InventorySlotList[0].CurrentSlotData;
        if (_currentRecycleTime <= 0f)
        {
            _currentRecycleTime = Utilities.Instance.GetRecyclingTimeAsPerGarbageType(_currentRecyclingSlotData.garbageType);
        }
        Debug.Log(_currentRecycleTime + "_currentRecycleTime");
        StartCoroutine("StartRecycling");
    }

    private IEnumerator StartRecycling()
    {
        while (_currentRecycleTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentRecycleTime -= 1f;
        }
        if (_currentRecycleTime <= 0)
        {
            _currentRecyclingSlotData.inventoryItemUiData.count -= 1;
            
            inventorySystem.RemoveSingleItemOfType(_currentRecyclingSlotData.garbageType);
            if (_currentRecyclingSlotData.inventoryItemUiData.count <= 0)
            {
                UpdateTotalUi();
            }
            else
            {
                UpdateDataInUi();
            }
            StopCoroutine("StartRecycling");
            ContinueRecycling();
        }
    }

    private void ContinueRecycling()
    {
        if (inventorySystem.GetInventoryItemsData().Count > 0)
        {
            SetRecyclingProcess();
        }
        else
        {
            Debug.Log(inventorySystem.GetInventoryItemsData().Count + " inventory count");
        }
    }
}
