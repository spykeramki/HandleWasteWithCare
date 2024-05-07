using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Sets player material as per the suit type
public class PlayerEquipmentCtrl : MonoBehaviour
{

    private EquipStationCtrl.PlayerProtectionSuitType _playerSuit;

    public EquipStationCtrl.PlayerProtectionSuitType PlayerSuit
    {
        get { return _playerSuit; }
    }
    public Image suit;

    private List<GarbageManager.GarbageType> itemsThatCanBeAddedToInventory = new List<GarbageManager.GarbageType>();
    public List<GarbageManager.GarbageType> GarbageThatCanBeAddedToInventory{
        get { return itemsThatCanBeAddedToInventory;}
    }

    public void SetPlayerEquipment(EquipStationCtrl.PlayerProtectionSuitType suitType )
    {
        _playerSuit = suitType;
        setDataInUi(suitType);
        itemsThatCanBeAddedToInventory = CanAddItemToInventoryBasedOnPlayerToolType(suitType);
        PlayerCtrl.LocalInstance.ChangePlayerSuit(suitType == EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD ? true : false);
    }

    private void setDataInUi(EquipStationCtrl.PlayerProtectionSuitType suitType)
    {
        suit.sprite = Utilities.Instance.GetSuitSpriteFromSuitType(suitType);
    }

    
    private List<GarbageManager.GarbageType> CanAddItemToInventoryBasedOnPlayerToolType(EquipStationCtrl.PlayerProtectionSuitType m_suitType)
    {

        List<GarbageManager.GarbageType> itemsThatCanBeAdded = new List<GarbageManager.GarbageType>();

        if(m_suitType == EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD)
        {
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.BIO_HAZARD);
        }
        if(m_suitType == EquipStationCtrl.PlayerProtectionSuitType.RADIATION)
        {
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.RADIOACTIVE);
        }

        return itemsThatCanBeAdded;
    }
}
