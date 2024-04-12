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

    public Light[] stationLights;

    [Serializable]
    public struct StationColors
    {
        public Color radiation;
        public Color bioHazard;
    }

    public StationColors stationColors;

    public Image uiBg;

    public Renderer playerModelRenderer;

    public TextMeshProUGUI suitTypeText;

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
        Debug.Log("calling onclick");
        if(playerProtectionSuitType == PlayerProtectionSuitType.BIO_HAZARD)
        {
            playerProtectionSuitType = PlayerProtectionSuitType.RADIATION;
        }
        else
        {
            playerProtectionSuitType = PlayerProtectionSuitType.BIO_HAZARD;
        }
        AdjustStationBasedOnSuitType(playerProtectionSuitType);
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

    private void AdjustStationBasedOnSuitType(PlayerProtectionSuitType m_suitType)
    {
        Material[] materials = playerModelRenderer.materials;
        if (m_suitType== PlayerProtectionSuitType.BIO_HAZARD)
        {
            materials[0] = PlayerCtrl.LocalInstance.playerSuitMats.biohazardSuit;
            ChangeStationLightsColor(stationColors.bioHazard);
            uiBg.color = stationColors.bioHazard;
        }
        else
        {
            materials[0] = PlayerCtrl.LocalInstance.playerSuitMats.radiationSuit;
            ChangeStationLightsColor(stationColors.radiation);
            uiBg.color = stationColors.radiation;
        }
        playerModelRenderer.materials = materials;
    }

    private void ChangeStationLightsColor(Color m_color)
    {
        foreach(Light light in stationLights)
        {
            light.color = m_color;
        }
    }
}
