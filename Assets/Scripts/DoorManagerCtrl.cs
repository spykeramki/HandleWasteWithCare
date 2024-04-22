using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorManagerCtrl : MonoBehaviour
{

    public Animator anim;

    private bool isDoorOpened = false;
    private string instructionText = "Press 'E' to open the Door";

    public bool isMainDoor = false;
    public bool isSuitDoor = false;
    public bool isBaseDoor = false;

    private bool _isFirstTimeOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.SetDataAndActivenessOfGeneralIntructUi(instructionText, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SetDataAndActivenessOfGeneralIntructUi(string.Empty, false);
            if(isDoorOpened){
                isDoorOpened = false;
                //anim.speed = -1;
                anim.SetTrigger("CloseDoor");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isDoorOpened)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isDoorOpened = true;
                //anim.speed = -1;
                anim.SetTrigger("OperateDoor");
                Invoke("ShowIntro", 1f);
            }

        }
    }

    private void ShowIntro()
    {
        if (!_isFirstTimeOpened)
        {
            _isFirstTimeOpened = true;
            if (isMainDoor)
            {
                GameManager.Instance.SetGameStateInGame(GameManager.GameState.DECONTAMINATION_UNIT);
            }
            else if (isSuitDoor)
            {
                GameManager.Instance.SetGameStateInGame(GameManager.GameState.CHANGE_SUIT);
            }
        }
    }
}
