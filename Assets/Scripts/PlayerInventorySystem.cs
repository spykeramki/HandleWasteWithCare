using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySystem : InventorySystem
{
    [SerializeField]
    private InventoryUiContainerCtrl playerInventoryUiCtrl;

    public void UpdateDataInInvetoryUi()
    {
        playerInventoryUiCtrl.RemoveAllItemsFromSlots();
        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(itemsData));
    }

    public void AddItemToInventory(GarbageCtrl garbageItemCtrl)
    {
        InventoryItemData inventoryItemData = new InventoryItemData()
        {
            garbageCtrl = garbageItemCtrl,
            count = 1
        };
        AddItem(inventoryItemData);
        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(itemsData));
    }

}
