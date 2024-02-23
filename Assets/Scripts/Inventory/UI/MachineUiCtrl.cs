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

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerTransferButton.onClick.AddListener(OnClickTransferToMachineInventoryButton);
    
    }

    public void OnPlayerConnectedToMachine()
    {
        gameManager = GameManager.Instance;
        playerInventoryUiCtrl.RemoveAllItemsFromSlots();
        machineInventoryUiCtrl.RemoveAllItemsFromSlots();
        Dictionary<string, InventorySystem.InventoryItemData> playerItemsData = gameManager.PlayerCtrl.PlayerInventory.GetInventoryItemsData();
        Dictionary<string, InventorySystem.InventoryItemData> machineItemsData = inventorySystem.GetInventoryItemsData();

        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(playerItemsData));
        machineInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(machineItemsData));

    }

    public void OnClickTransferToMachineInventoryButton()
    {
        Debug.Log("Button Cliked");
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = playerInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData playerItemData = gameManager.PlayerCtrl.PlayerInventory.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //inventorySlotUiCtrl.RemoveItemFromSlot();

            gameManager.PlayerCtrl.PlayerInventory.RemoveItem(playerItemData);
            inventorySystem.AddItem(playerItemData);
        }

        if (inventorySlotUiCtrls.Count != 0) 
        {
            OnPlayerConnectedToMachine();
            //gameManager.PlayerCtrl.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }
}
