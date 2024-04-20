using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySystem : InventorySystem
{
    [SerializeField]
    private InventoryUiContainerCtrl playerInventoryUiCtrl;

    private bool _isFirstRadioActiveItemCollected = false;
    private bool IsFirstBioHazardItem = false;

    public void UpdateDataInInvetoryUi()
    {
        playerInventoryUiCtrl.RemoveAllItemsFromSlots();
        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(itemsData));
    }

    public void AddItemToInventory(GarbageManager.GarbageType m_garbageType, int m_count)
    {
        if(!_isFirstRadioActiveItemCollected  && m_garbageType == GarbageManager.GarbageType.RADIOACTIVE && 
            GameManager.Instance.CurrentGameState== GameManager.GameState.COLLECT_RADIOACTIVE_WASTE)
        {
            _isFirstRadioActiveItemCollected = true;
            GameManager.Instance.SetGameStateInGame(GameManager.GameState.DECONTAMINATION);
        }
        InventoryItemData inventoryItemData = new InventoryItemData()
        {
            garbageType = m_garbageType,
            count = m_count
        };
        AddItem(inventoryItemData);
        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(itemsData));
    }

}
