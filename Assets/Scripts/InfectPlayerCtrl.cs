using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When player enter premises of hazardous material this gives material hazard type
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
