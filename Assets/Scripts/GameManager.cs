using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public UnityEvent<bool> SetPlayerStateToUiMode = new UnityEvent<bool>();

    public enum PlayerLocation
    {
        WORLD,
        BASE,
        CLEANSER
    }

    public PlayerLocation playerLocation = PlayerLocation.BASE;

    [SerializeField]
    private PlayerCtrl playerCtrl;

    private FirstPersonController firstPersonController;

    [SerializeField] private GarbageManager garbageManager;

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

    private void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }
}
