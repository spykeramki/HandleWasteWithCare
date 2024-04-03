using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipStationCtrl : MonoBehaviour
{

    public enum PlayerProtectionSuitType
    {
        BIO_HAZARD,
        RADIATION
    }

    private PlayerProtectionSuitType playerProtectionSuitType;

    public ToggleOption suit;

    public GameObject uiCanvas;

    private void Awake()
    {
        PlayerCtrl.SetEquipmentData += OnPlayerSpawn;
    }

    private void Start()
    {
        suit.ToggleBtn.onClick.AddListener(OnClickEquipmentChangeBtn);
    }

    private void OnPlayerSpawn(PlayerProtectionSuitType suitType)
    {
        playerProtectionSuitType = suitType;
        SetDataInUi(suitType);
    }

    public void SetPlayerEquipment(PlayerProtectionSuitType suitType)
    {
        if (PlayerCtrl.LocalInstance!=null)
        {
            PlayerCtrl.LocalInstance.PlayerEquipment.SetPlayerEquipment(suitType);
        }
    }

    public void SetDataInUi(PlayerProtectionSuitType m_suitType)
    {
        suit.ToggleImage.sprite = Utilities.Instance.GetSuitSpriteFromSuitType(m_suitType);
    }

    private void OnClickEquipmentChangeBtn()
    {
        if(playerProtectionSuitType == PlayerProtectionSuitType.BIO_HAZARD)
        {
            playerProtectionSuitType = PlayerProtectionSuitType.RADIATION;
        }
        else
        {
            playerProtectionSuitType = PlayerProtectionSuitType.BIO_HAZARD;
        }
        SetPlayerEquipment(playerProtectionSuitType);

        SetDataInUi(playerProtectionSuitType);
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
