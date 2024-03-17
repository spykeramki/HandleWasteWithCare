using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using System;

public class PlayerCtrl : NetworkBehaviour
{
    public static Action<EquipStationCtrl.EquipData> SetEquipmentData;

    public static PlayerCtrl LocalInstance;

    [SerializeField]
    private float health = 100;
    private float stamina = 100;
    [SerializeField]
    private float bioHazardLevel = 0;
    [SerializeField]
    private float radiationLevel = 0;

    [SerializeField]
    private PlayerInventorySystem playerInventorySystem;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private AudioListener audioListener;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineVitualCam;

    [SerializeField]
    private FirstPersonController fpController;

    [SerializeField]
    private PlayerInput platerInput;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private PlayerEquipmentCtrl playerEquipmentCtrl;

    public GameObject playerInventoryGo;

    private bool _isPlayerUiActive = false;

    public PlayerEquipmentCtrl PlayerEquipment
    {
        get { return playerEquipmentCtrl; }
    }

    public PlayerInventorySystem PlayerInventory
    {
        get { return playerInventorySystem; }
    }

    private float _healthToReduceForEachLevelOfRadiation = 0.1f;
    private float _healthToReduceForEachLevelOfBioHazardEffect = 0.2f;

    private bool isHealthDecreasing = false;

    public float BioHazardLevel
    {
        get { return bioHazardLevel; }
        set { bioHazardLevel = value; }
    }

    public float RadiationLevel
    {
        get { return radiationLevel; }
        set { radiationLevel = value; }
    }

    public float Stamina
    {
        get { return stamina; }
    }

    public float Health
    {
        get { return health; }
    }

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            LocalInstance = this;
            characterController.enabled = true;
            fpController.enabled = true;
            platerInput.enabled = true;
            audioListener.enabled = true;
            cinemachineVitualCam.Priority = 1;
        }
        else
        {
            cinemachineVitualCam.Priority = 0;
        }
        SetPlayerInitialEquipmentData();
    }

    #region NON MULTIPLAYER
    private void Awake()
    {
        LocalInstance = this;
    }

    private void Start()
    {
        SetPlayerInitialEquipmentData();
    }
    #endregion

    private void Update()
    {
        if (!isHealthDecreasing && (radiationLevel > 0 || bioHazardLevel > 0))
        {
            ReduceHealthByRadiationOrBioHazardLevelIncrease();
            isHealthDecreasing = true;
        }
        if (isHealthDecreasing && (radiationLevel <= 0 && bioHazardLevel <= 0))
        {
            StopCoroutine("ReduceHealthSlowly");
            isHealthDecreasing = false;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isPlayerUiActive = !_isPlayerUiActive;
            playerInventoryGo.SetActive(_isPlayerUiActive);
        }
    }

    private void LateUpdate()
    {
        /*if (!IsOwner)
        {
            return;
        }*/
        RaycastHit hit;
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 5f, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 5f))
            {
                if (hit.collider.tag == "Garbage")
                {
                    GarbageCtrl garbageCtrl = hit.collider.GetComponent<GarbageCtrl>();
                    if(garbageCtrl!=null && playerEquipmentCtrl.GarbageThatCanBeAddedToInventory.Contains(garbageCtrl.GarbageType)){
                        playerInventorySystem.AddItemToInventory(garbageCtrl);
                        //garbageCtrl.HideObjectServerRpc();
                        garbageCtrl.HideObject();
                    }
                }
                if (hit.collider.tag == "Button")
                {
                    Button buttonCom = hit.collider.GetComponent<Button>();
                    buttonCom.onClick.Invoke();
                }
            }
        }
    }

    private void SetPlayerInitialEquipmentData()
    {
        EquipStationCtrl.EquipData equipData = new EquipStationCtrl.EquipData()
        {
            playerProtectionSuitType = EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD,
            leftHandGunType = EquipStationCtrl.GunType.SCANNER,
            rightHandGunType = EquipStationCtrl. GunType.RADIATION
        };
        playerEquipmentCtrl.SetPlayerEquipment(equipData);
        SetEquipmentData?.Invoke(equipData);
    }

    private void ReduceHealthByRadiationOrBioHazardLevelIncrease()
    {
        StartCoroutine("ReduceHealthSlowly");
    }

    private IEnumerator ReduceHealthSlowly()
    {
        while (health>0)
        {
            yield return new WaitForSeconds(1f);
            health -= (radiationLevel* _healthToReduceForEachLevelOfRadiation) + (bioHazardLevel * _healthToReduceForEachLevelOfBioHazardEffect);
        }

        if(health <= 0)
        {
            StopCoroutine("ReduceHealthSlowly");
            isHealthDecreasing=false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InfectPlayerCtrl infectPlayerCtrl = other.GetComponent<InfectPlayerCtrl>();
        if(infectPlayerCtrl != null)
        {
            if (playerEquipmentCtrl.PlayerEquipData.playerProtectionSuitType!= EquipStationCtrl.PlayerProtectionSuitType.RADIATION &&
                infectPlayerCtrl.InfectType == GarbageManager.GarbageType.RADIOACTIVE)
            {
                StartCoroutine("IncreaseRadiationValueSlowly");
            }
            else if (playerEquipmentCtrl.PlayerEquipData.playerProtectionSuitType != EquipStationCtrl.PlayerProtectionSuitType.BIO_HAZARD && 
                infectPlayerCtrl.InfectType == GarbageManager.GarbageType.ORGANIC)
            {
                StartCoroutine("IncreaseBioHazardValueSlowly");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InfectPlayerCtrl infectPlayerCtrl = other.GetComponent<InfectPlayerCtrl>();
        
        if(infectPlayerCtrl != null)
        {
            StopInfectLevelCoroutines(infectPlayerCtrl.InfectType);
        }
    }

    public IEnumerator IncreaseBioHazardValueSlowly()
    {
        while (bioHazardLevel < 100)
        {
            yield return new WaitForSeconds(1f);
            bioHazardLevel += 1f;
        }

        if (bioHazardLevel >= 100)
        {
            bioHazardLevel = 100;
            StopCoroutine("IncreaseBioHazardValueSlowly");
        }
    }

    public IEnumerator IncreaseRadiationValueSlowly()
    {
        while (radiationLevel < 100)
        {
            yield return new WaitForSeconds(1f);
            radiationLevel += 1f;
        }

        if (radiationLevel >= 100)
        {
            radiationLevel = 100;
            StopCoroutine("IncreaseRadiationValueSlowly");
        }
    }

    public void StopInfectLevelCoroutines(GarbageManager.GarbageType garbageType)
    {
        if (garbageType == GarbageManager.GarbageType.RADIOACTIVE)
        {
            StopCoroutine("IncreaseRadiationValueSlowly");
        }
        else if (garbageType == GarbageManager.GarbageType.ORGANIC)
        {
            StopCoroutine("IncreaseBioHazardValueSlowly");
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
