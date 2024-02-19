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
