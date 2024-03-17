using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GarbageCtrl : NetworkBehaviour
{
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

    public void HideObject()
    {
        gameObject.SetActive(false);
    }
}
