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

    public void AddItemToInventory(GarbageManager.GarbageType m_garbageType, int m_count)
    {
        InventoryItemData inventoryItemData = new InventoryItemData()
        {
            garbageType = m_garbageType,
            count = m_count
        };
        AddItem(inventoryItemData);
        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(itemsData));
    }

}
