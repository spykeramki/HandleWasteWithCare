using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipmentCtrl : MonoBehaviour
{

    private EquipStationCtrl.EquipData _playerEquipData;

    public EquipStationCtrl.EquipData PlayerEquipData
    {
        get { return _playerEquipData; }
    }

    [Serializable]
    private struct EquipmentImages
    {
        public Image suit;
        public Image leftHand;
        public Image rightHand;
    }

    [SerializeField] private EquipmentImages equipImages;

    private List<GarbageManager.GarbageType> itemsThatCanBeAddedToInventory = new List<GarbageManager.GarbageType>();
    public List<GarbageManager.GarbageType> GarbageThatCanBeAddedToInventory{
        get { return itemsThatCanBeAddedToInventory;}
    }

    public void SetPlayerEquipment(EquipStationCtrl.EquipData equipData )
    {
        _playerEquipData = equipData;
        setDataInUi(equipData);
        itemsThatCanBeAddedToInventory = CanAddItemToInventoryBasedOnPlayerToolType(equipData);
        //SetMaterialsAndPlayerObjects();
    }

    private void setDataInUi(EquipStationCtrl.EquipData equipData)
    {
        Utilities utilities = Utilities.Instance;
        equipImages.suit.sprite = utilities.GetSuitSpriteFromSuitType(equipData.playerProtectionSuitType);
        equipImages.leftHand.sprite = utilities.GetHandlerSpriteFromSuitType(equipData.leftHandGunType);
        equipImages.rightHand.sprite = utilities.GetHandlerSpriteFromSuitType(equipData.rightHandGunType);
    }

    
    private List<GarbageManager.GarbageType> CanAddItemToInventoryBasedOnPlayerToolType(EquipStationCtrl.EquipData equipData ){
        EquipStationCtrl.GunType leftHandType = _playerEquipData.leftHandGunType;
        EquipStationCtrl.GunType rightHandType = _playerEquipData.rightHandGunType;

        List<GarbageManager.GarbageType> itemsThatCanBeAdded = new List<GarbageManager.GarbageType>();

        if(leftHandType == EquipStationCtrl.GunType.DRY_WASTE || rightHandType == EquipStationCtrl.GunType.DRY_WASTE){
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.PLASTIC);
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.GLASS);
        }
        if(leftHandType == EquipStationCtrl.GunType.FLUID_WASTE || rightHandType == EquipStationCtrl.GunType.FLUID_WASTE){
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.OIL);
        }
        if(leftHandType == EquipStationCtrl.GunType.ORGANIC_WASTE || rightHandType == EquipStationCtrl.GunType.ORGANIC_WASTE){
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.ORGANIC);
        }
        if(leftHandType == EquipStationCtrl.GunType.RADIATION || rightHandType == EquipStationCtrl.GunType.RADIATION){
            itemsThatCanBeAdded.Add(GarbageManager.GarbageType.RADIOACTIVE);
        }

        return itemsThatCanBeAdded;
    }
}
