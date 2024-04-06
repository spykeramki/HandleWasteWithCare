
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

    [Serializable]
    public struct EquipmentSprites
    {
        public Sprite radiationSuit;
        public Sprite bioSuit;
        public Sprite dryWasteHandler;
        public Sprite wetWasteHandler;
        public Sprite bioHazardWasteHandler;
        public Sprite radiationWasteHandler;
        public Sprite scanner;

    }

    public EquipmentSprites equipmentSprites;

    [Serializable]
    public struct TimeToRecycleWastages
    {
        public float plastic;
        public float glass;
        public float oil;
        public float organic;
        public float radioactive;

    }

    public TimeToRecycleWastages recycleTimes;

    public InventoryUiContainerCtrl.UiData PrepareDataForInventoryUi(Dictionary<string, InventorySystem.InventoryItemData> inventoryItemsData)
    {
        InventoryUiContainerCtrl.UiData uiData = new InventoryUiContainerCtrl.UiData();

        List<InventorySlotUiCtrl.UiData> inventorySlotsUiData = new List<InventorySlotUiCtrl.UiData>();

        foreach (KeyValuePair<string, InventorySystem.InventoryItemData> inventoryItem in inventoryItemsData)
        {
                InventorySlotUiCtrl.UiData inventorySlotUiCtrlData = new InventorySlotUiCtrl.UiData();
                GarbageManager.GarbageType garbageType = inventoryItem.Value.garbageType;
                InventoryItemUiCtrl.UiData inventoryItemUiData = new InventoryItemUiCtrl.UiData();
                inventoryItemUiData.itemImage = GetSpriteFromGarbageType(garbageType);
                inventoryItemUiData.count = inventoryItem.Value.count;
                inventorySlotUiCtrlData.inventoryItemUiData = inventoryItemUiData;
                inventorySlotUiCtrlData.garbageType = garbageType;
                inventorySlotsUiData.Add(inventorySlotUiCtrlData);

            /*Debug.Log(inventoryItemUiData.itemImage.name + "image");
            Debug.Log(inventoryItemUiData.count + "count");*/
        }

        uiData.inventorySlotsUiData = inventorySlotsUiData;

        return uiData;
    }

    public InventoryUiContainerCtrl.UiData PrepareDataForInventoryUi(Dictionary<string, InventorySystem.InventoryItemData> inventoryItemsData, GarbageManager.GarbageType m_garbageType)
    {
        InventoryUiContainerCtrl.UiData uiData = new InventoryUiContainerCtrl.UiData();

        List<InventorySlotUiCtrl.UiData> inventorySlotsUiData = new List<InventorySlotUiCtrl.UiData>();

        foreach (KeyValuePair<string, InventorySystem.InventoryItemData> inventoryItem in inventoryItemsData)
        {
            GarbageManager.GarbageType garbageType = inventoryItem.Value.garbageType;
            if(m_garbageType == garbageType){
                InventorySlotUiCtrl.UiData inventorySlotUiCtrlData = new InventorySlotUiCtrl.UiData();
                InventoryItemUiCtrl.UiData inventoryItemUiData = new InventoryItemUiCtrl.UiData();
                inventoryItemUiData.itemImage = GetSpriteFromGarbageType(garbageType);
                inventoryItemUiData.count = inventoryItem.Value.count;
                inventorySlotUiCtrlData.inventoryItemUiData = inventoryItemUiData;
                inventorySlotUiCtrlData.garbageType = garbageType;
                inventorySlotsUiData.Add(inventorySlotUiCtrlData);
            }

            /*Debug.Log(inventoryItemUiData.itemImage.name + "image");
            Debug.Log(inventoryItemUiData.count + "count");*/
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
            case GarbageManager.GarbageType.BIO_HAZARD:
                return garbageSprites.organic;
            case GarbageManager.GarbageType.RADIOACTIVE:
                return garbageSprites.radioactive;
        }
        return null;
    }

    public Sprite GetSuitSpriteFromSuitType(EquipStationCtrl.PlayerProtectionSuitType suitType)
    {
        switch (suitType)
        {
            case EquipStationCtrl.PlayerProtectionSuitType.RADIATION:
                return equipmentSprites.radiationSuit;
            case EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD:
                return equipmentSprites.bioSuit;
        }
        return null;
    }


    public float GetRecyclingTimeAsPerGarbageType(GarbageManager.GarbageType garbageType)
    {
        float time = 0f;
        switch (garbageType)
        {
            case GarbageManager.GarbageType.PLASTIC:
                time = recycleTimes.plastic;
                break;
            case GarbageManager.GarbageType.GLASS:
                time = recycleTimes.glass;
                break;
            case GarbageManager.GarbageType.BIO_HAZARD:
                time = recycleTimes.organic;
                break;
            case GarbageManager.GarbageType.OIL:
                time = recycleTimes.oil;
                break;
            case GarbageManager.GarbageType.RADIOACTIVE:
                time = recycleTimes.radioactive;
                break;
            default:
                time = 0f;
                break;
        }
        return time;
    }

    public void SetSettingsForUi(bool isUiActive)
    {
        if(isUiActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
