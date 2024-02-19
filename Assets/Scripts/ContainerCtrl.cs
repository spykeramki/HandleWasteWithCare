using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCtrl : MonoBehaviour
{
    [SerializeField]
    private Material garbageLoadedMat;

    private GarbageManager.GarbageType _garbageAdded = GarbageManager.GarbageType.NONE;

    private Material _initialMat;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }


    public GarbageManager.GarbageType GarbageAdded
    {
        get { return _garbageAdded; }
    }

    public void SetOrRemoveGarbageInTheContainer(GarbageManager.GarbageType garbageAdded)
    {
        _garbageAdded = garbageAdded;
        ChangeMaterialOfTheContainer(garbageAdded);
    }

    private void ChangeMaterialOfTheContainer(GarbageManager.GarbageType garbageAdded)
    {
        Material material;
        if(garbageAdded == GarbageManager.GarbageType.NONE)
        {
            material = _initialMat;
        }
        else
        {
            material = garbageLoadedMat;
        }
        _renderer.material = material;
    }
}
