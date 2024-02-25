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

    public void SetPlayerEquipment(EquipStationCtrl.EquipData equipData )
    {
        _playerEquipData = equipData;
        setDataInUi(equipData);
        //SetMaterialsAndPlayerObjects();
    }

    private void setDataInUi(EquipStationCtrl.EquipData equipData)
    {
        Utilities utilities = Utilities.Instance;
        equipImages.suit.sprite = utilities.GetSuitSpriteFromSuitType(equipData.playerProtectionSuitType);
        equipImages.leftHand.sprite = utilities.GetHandlerSpriteFromSuitType(equipData.leftHandGunType);
        equipImages.rightHand.sprite = utilities.GetHandlerSpriteFromSuitType(equipData.rightHandGunType);
    }
}
