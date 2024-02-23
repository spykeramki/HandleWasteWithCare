using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCtrl : MonoBehaviour
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
        if (GameManager.Instance.PlayerCtrl != null)
        {
            GameManager.Instance.PlayerCtrl.StopInfectLevelCoroutines(garbageType);
        }
    }
}
