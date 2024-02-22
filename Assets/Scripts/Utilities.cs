
using System;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour 
{
    public static Utilities Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
    }

    [Serializable]
    public struct GarbageSprites
    {
        public Sprite emptyContainer;
        public Sprite plastic;
        public Sprite glass; 
        public Sprite oil;
        public Sprite organic;
        public Sprite radioactive;

    }

    public GarbageSprites garbageSprites;

    public InventoryUiContainerCtrl.UiData PrepareDataForInventoryUi(Dictionary<string, InventorySystem.InventoryItemData> inventoryItemsData)
    {
        InventoryUiContainerCtrl.UiData uiData = new InventoryUiContainerCtrl.UiData();

        List<InventorySlotUiCtrl.UiData> inventorySlotsUiData = new List<InventorySlotUiCtrl.UiData>();

        foreach (KeyValuePair<string, InventorySystem.InventoryItemData> inventoryItem in inventoryItemsData)
        {
            InventorySlotUiCtrl.UiData inventorySlotUiCtrlData = new InventorySlotUiCtrl.UiData();
            GarbageManager.GarbageType garbageType = inventoryItem.Value.garbageCtrl.GarbageType;
            InventoryItemUiCtrl.UiData inventoryItemUiData = new InventoryItemUiCtrl.UiData();
            inventoryItemUiData.itemImage = GetSpriteFromGarbageType(garbageType);
            inventoryItemUiData.count = inventoryItem.Value.count;
            inventorySlotUiCtrlData.inventoryItemUiData = inventoryItemUiData;
            inventorySlotUiCtrlData.garbageType = garbageType;
            inventorySlotsUiData.Add(inventorySlotUiCtrlData);

            Debug.Log(inventoryItemUiData.itemImage.name + "image");
            Debug.Log(inventoryItemUiData.count + "count");
        }

        uiData.inventorySlotsUiData = inventorySlotsUiData;

        return uiData;
    }

    public Sprite GetSpriteFromGarbageType(GarbageManager.GarbageType garbageType)
    {
        switch (garbageType)
        {
            case GarbageManager.GarbageType.NONE:
                return garbageSprites.emptyContainer;
            case GarbageManager.GarbageType.PLASTIC:
                return garbageSprites.plastic;
            case GarbageManager.GarbageType.GLASS:
                return garbageSprites.glass;
            case GarbageManager.GarbageType.OIL:
                return garbageSprites.oil;
            case GarbageManager.GarbageType.ORGANIC:
                return garbageSprites.organic;
            case GarbageManager.GarbageType.RADIOACTIVE:
                return garbageSprites.radioactive;
        }
        return null;
    }
}
