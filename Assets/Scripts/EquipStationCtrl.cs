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

    public Renderer uiBg;

    public Renderer playerModelRenderer;


    public GameObject bioHazardModel;
    public GameObject radiationModel;

    public TextMeshProUGUI suitTypeText;

    public AudioSource uiAudioSource;

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
        AdjustStationBasedOnSuitType(suitType);
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
        PlayerCtrl.LocalInstance.PlayPlayerAudio(Utilities.Instance.GetRandomEquipClip(), shouldLoop: false, m_volume: 1.0f);
        uiAudioSource.Play();
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
        bool isBioHazard = m_suitType == PlayerProtectionSuitType.BIO_HAZARD;
        bioHazardModel.SetActive(isBioHazard);
        radiationModel.SetActive(!isBioHazard);
        if (isBioHazard)
        {
            ChangeStationLightsColor(stationColors.bioHazard);
            uiBg.material.color = stationColors.bioHazard;
            uiBg.material.SetColor("_EmissionColor", stationColors.bioHazard);
            suitTypeText.text = "Bio Hazard";
        }
        else
        {
            ChangeStationLightsColor(stationColors.radiation);
            uiBg.material.color = stationColors.radiation;
            uiBg.material.SetColor("_EmissionColor", stationColors.radiation);
            suitTypeText.text = "Radiation";
        }
    }

    private void ChangeStationLightsColor(Color m_color)
    {
        foreach(Light light in stationLights)
        {
            light.color = m_color;
        }
    }
}
