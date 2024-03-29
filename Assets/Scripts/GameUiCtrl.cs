using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiCtrl : MonoBehaviour
{
    public PauseMenuCtrl pauseMenuCtrl;


    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenuCtrl.OnClickResumeBtn();
        }
    }


}
