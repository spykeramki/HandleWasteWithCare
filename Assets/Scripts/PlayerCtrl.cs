using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerCtrl : NetworkBehaviour
{
    public static PlayerCtrl Instance;

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
    private PlayerEquipmentCtrl playerEquipmentCtrl;

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

    RaycastHit hit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
    }
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
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
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
                        garbageCtrl.gameObject.SetActive(false);
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
