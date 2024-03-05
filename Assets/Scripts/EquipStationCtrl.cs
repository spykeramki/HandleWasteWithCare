using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        NONE,
        BIO_HAZARD,
        RADIATION
    }

    public enum GunType
    {
        NONE,
        DRY_WASTE,
        FLUID_WASTE,
        ORGANIC_WASTE,
        RADIATION,
        SCANNER
    }

    private EquipData equipData;

    [Serializable]
    private struct ToggleSystems
    {
        public ToggleOptionsCtrl suit;
        public ToggleOptionsCtrl rightHand;
        public ToggleOptionsCtrl leftHand;
    }

    [SerializeField] private ToggleSystems toggleSystems;

    public GameObject uiCanvas;

    private void Start()
    {
        PlayerCtrl.SetEquipmentData += OnPlayerSpawn;
    }

    private void OnPlayerSpawn(EquipData data)
    {
        equipData = data;
        SetDataInUi(equipData, GetDataFromOptionsAndSetData);
    }

    public void SetPlayerEquipment(EquipData data)
    {
        equipData = data;
        if (PlayerCtrl.LocalInstance!=null)
        {
            PlayerCtrl.LocalInstance.PlayerEquipment.SetPlayerEquipment(equipData);
        }
    }

    public void GetDataFromOptionsAndSetData()
    {
        EquipData equipData = new EquipData()
        {
            playerProtectionSuitType = toggleSystems.suit.GetActiveEquipmentType().playerProtectionSuitType,
            leftHandGunType = toggleSystems.leftHand.GetActiveEquipmentType().playerGunType,
            rightHandGunType = toggleSystems.rightHand.GetActiveEquipmentType().playerGunType
        };
        SetPlayerEquipment(equipData) ;
    }

    public void SetDataInUi(EquipData m_equipData, Action action = null)
    {
        ToggleOption.EquipmentType suitData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = m_equipData.playerProtectionSuitType,
            playerGunType = GunType.NONE
        };
        toggleSystems.suit.SetDataInUi(suitData, action);

        ToggleOption.EquipmentType leftHandData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = PlayerProtectionSuitType.NONE,
            playerGunType = m_equipData.leftHandGunType
        };

        toggleSystems.leftHand.SetDataInUi(leftHandData, action);

        ToggleOption.EquipmentType rightHandData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = PlayerProtectionSuitType.NONE,
            playerGunType = m_equipData.rightHandGunType
        };

        toggleSystems.rightHand.SetDataInUi(rightHandData, action);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            uiCanvas.SetActive(false);
        }
    }
}
