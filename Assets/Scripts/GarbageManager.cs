using System;
using Unity.Netcode;
using UnityEngine;

public class GarbageManager : NetworkBehaviour
{
    public enum GarbageType
    {
        PLASTIC,
        GLASS,
        OIL,
        BIO_HAZARD,
        RADIOACTIVE,
        NONE
    }

    
}
