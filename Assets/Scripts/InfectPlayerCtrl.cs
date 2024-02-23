using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InfectPlayerCtrl : MonoBehaviour
{
    private GarbageManager.GarbageType _garbageType;

    private void Start()
    {
        _garbageType =GetComponentInParent<GarbageCtrl>().GarbageType;
    }

    public GarbageManager.GarbageType InfectType { get { return _garbageType; } }

    
}
