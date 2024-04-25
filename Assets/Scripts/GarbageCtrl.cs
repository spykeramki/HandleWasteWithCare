using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GarbageCtrl : MonoBehaviour
{
    public enum GarbageState
    {
        YET_TO_COLLECT,
        COLLECTED
    }

    [SerializeField]
    private GarbageManager.GarbageType garbageType;

    public GarbageManager.GarbageType GarbageType
    {
        get { return garbageType; }
    }

    [SerializeField]
    private InfectPlayerCtrl infectPlayerCtrl;

    public InfectPlayerCtrl InfectPlayerCtrl
    {
        get { return infectPlayerCtrl; }
    }

    private string id;

    public string Id
    {
        get { return id; }
    }

    private GarbageState garbageState = GarbageState.YET_TO_COLLECT;
    public GarbageState CurrentGarbageState { get { return garbageState; } }

    private void Awake()
    {
        id = gameObject.name;
    }

    private void OnDisable()
    {
        if (PlayerCtrl.LocalInstance != null)
        {
            PlayerCtrl.LocalInstance.StopInfectLevelCoroutines(garbageType);
        }
    }

    private void Update()
    {
        if(transform.position.y < -50f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HideObjectServerRpc()
    {
        HideObjectClientRpc();
    }

    [ClientRpc]
    public void HideObjectClientRpc()
    {
        gameObject.SetActive(false);
    }

    public void SetActiveness(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetGarbageType(GarbageManager.GarbageType m_garbageType)
    {
        garbageType = m_garbageType;
    }

    public void SetGarbageState(GarbageState m_garbageState)
    {
        garbageState = m_garbageState;
        bool isActive = false;

        if (m_garbageState == GarbageState.YET_TO_COLLECT)
        {
            isActive = true;
        }
        SetActiveness(isActive);
    }
}
