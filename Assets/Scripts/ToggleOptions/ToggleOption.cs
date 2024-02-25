using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ToggleOption : MonoBehaviour
{
    [Serializable]
    public struct EquipmentType
    {
        public EquipStationCtrl.PlayerProtectionSuitType playerProtectionSuitType;

        public EquipStationCtrl.GunType playerGunType;
    }

    [SerializeField]
    private EquipmentType thisEquipmentType;

    public EquipmentType ThisEquipmentType
    {
        get => thisEquipmentType;
    }

}
