using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipStationCtrl : MonoBehaviour
{
    [Serializable]
    public struct EquipData
    {
        public PlayerProtectionSuitType playerProtectionSuitType;
        public GunType leftHandGunType;
        public GunType rightHandGunType;
    }

    public enum PlayerProtectionSuitType
    {
        BIO_HAZARD,
        RADIATION
    }

    public enum GunType
    {
        DRY_WASTE,
        FLUID_WASTE,
        ORGANIC_WASTE,
        RADIATION,
        SCANNER
    }

    [SerializeField] private EquipData equipData;

    public void SetPlayerEquipment(EquipData data)
    {
        equipData = data;
        //SetUiData();
    }
}