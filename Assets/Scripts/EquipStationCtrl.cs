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
        SetPlayerEquipment( new EquipData()
        {
            playerProtectionSuitType = PlayerProtectionSuitType.BIO_HAZARD,
            leftHandGunType = GunType.SCANNER,
            rightHandGunType = GunType.DRY_WASTE
        });
        SetDataInUi(GetDataFromOptionsAndSetData);
    }

    public void SetPlayerEquipment(EquipData data)
    {
        equipData = data;
        if (PlayerCtrl.Instance!=null)
        {
            PlayerCtrl.Instance.PlayerEquipment.SetPlayerEquipment(equipData);
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

    public void SetDataInUi(Action action = null)
    {
        ToggleOption.EquipmentType suitData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = equipData.playerProtectionSuitType,
            playerGunType = GunType.NONE
        };
        toggleSystems.suit.SetDataInUi(suitData, action);

        ToggleOption.EquipmentType leftHandData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = PlayerProtectionSuitType.NONE,
            playerGunType = equipData.leftHandGunType
        };

        toggleSystems.leftHand.SetDataInUi(leftHandData, action);

        ToggleOption.EquipmentType rightHandData = new ToggleOption.EquipmentType()
        {
            playerProtectionSuitType = PlayerProtectionSuitType.NONE,
            playerGunType = equipData.rightHandGunType
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
