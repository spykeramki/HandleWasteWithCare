using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public void ChangeParentToWake()
    {
        Debug.Log(" executed 1");
        GameManager gameManager = GameManager.Instance;
        PlayerCtrl.LocalInstance.ChangeCamToWake();
        gameManager.SetActivenessOfPlayerHudUi(true);
        gameManager.SetGameStateInGame(GameManager.GameState.COLLECT_RADIOACTIVE_WASTE);
        Debug.Log(" executed 2");
    }
}
