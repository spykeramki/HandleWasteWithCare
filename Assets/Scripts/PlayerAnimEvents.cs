using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These are the Animation events called in a specific state of animation
public class PlayerAnimEvents : MonoBehaviour
{
    public void ChangeParentToWake()
    {
        GameManager gameManager = GameManager.Instance;
        PlayerCtrl.LocalInstance.ChangeCamToWake();
        gameManager.SetActivenessOfPlayerHudUi(true);
        if(gameManager.CurrentGameState == GameManager.GameState.NEW_ARRIVAL)
        {
            gameManager.SetGameStateInGame(GameManager.GameState.COLLECT_RADIOACTIVE_WASTE);
        }
    }

    public void OnMovingOneStep(){
        PlayerCtrl.LocalInstance.PlayFootStepsAudio(Utilities.Instance.GetRandomFootStep(), shouldLoop: false, m_volume: 0.7f);
    }
}
