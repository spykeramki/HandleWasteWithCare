using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
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

    public PlayerInventorySystem PlayerInventory
    {
        get { return playerInventorySystem; }
    }

    private float _healthToReduceForEachLevelOfRadiation = 0.1f;
    private float _healthToReduceForEachLevelOfBioHazardEffect = 0.2f;

    private bool isHealthDecreasing = false;

    private EquipStationCtrl.EquipData _playerEquipData;

    public float BioHazardLevel
    {
        get { return bioHazardLevel; }
    }

    public float RadiationLevel
    {
        get { return radiationLevel; }
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
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 5f, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 5f))
            {
                if (hit.collider.tag == "Garbage")
                {
                    GarbageCtrl garbageCtrl = hit.collider.GetComponent<GarbageCtrl>();
                    playerInventorySystem.AddItemToInventory(garbageCtrl);
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
            StartCoroutine("ReduceHealthSlowly");
            isHealthDecreasing=false;
        }
    }

    public void SetPlayerEquipment(EquipStationCtrl.EquipData equipData )
    {
        _playerEquipData = equipData;
        //set player equip
    }
}
