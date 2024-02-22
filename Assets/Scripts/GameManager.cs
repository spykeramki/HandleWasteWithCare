using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum PlayerLocation
    {
        WORLD,
        BASE,
        CLEANSER
    }

    public PlayerLocation playerLocation = PlayerLocation.BASE;

    [SerializeField]
    private PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl { get { return playerCtrl; } }

    [SerializeField]
    private BaseMachinesInventoryCtrl baseMachinesInventoryCtrl;

    public BaseMachinesInventoryCtrl BaseMachinesInventoryCtrl { get => baseMachinesInventoryCtrl; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance!= null && Instance != this) { 
            Destroy(this);
        }
    }
}
