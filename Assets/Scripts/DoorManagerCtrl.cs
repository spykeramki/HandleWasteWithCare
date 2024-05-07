using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Controls door when triggered
public class DoorManagerCtrl : MonoBehaviour
{

    public Animator anim;

    private bool isDoorOpened = false;
    private string instructionText = "Press 'E' to open the Door";

    public bool isMainDoor = false;
    public bool isSuitDoor = false;
    public bool isBaseDoor = false;

    public AudioSource mainAudioSourceRef;

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
                if(isBaseDoor){
                    mainAudioSourceRef.PlayOneShot(Utilities.Instance.gameAudioClips.swingDoorClose);
                }
                else{
                    mainAudioSourceRef.PlayOneShot(Utilities.Instance.gameAudioClips.slidingDoorClose);
                }
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
                if(isBaseDoor){
                    mainAudioSourceRef.PlayOneShot(Utilities.Instance.gameAudioClips.swingDoorOpen);
                }
                else{
                    mainAudioSourceRef.PlayOneShot(Utilities.Instance.gameAudioClips.slidingDoorOpen);
                }
            }

        }
    }

    //Method to set the game status when the doors were opened first time
    private void ShowIntro()
    {
        GameManager gameManager = GameManager.Instance;
        if (!_isFirstTimeOpened)
        {
            if (isMainDoor && gameManager.CurrentGameState == GameManager.GameState.AFTER_EFFECTS)
            {
                _isFirstTimeOpened = true;
                gameManager.SetGameStateInGame(GameManager.GameState.DECONTAMINATION_UNIT);
            }
            else if (isSuitDoor && gameManager.CurrentGameState == GameManager.GameState.DECONTAMINATION_UNIT)
            {
                _isFirstTimeOpened = true;
                gameManager.SetGameStateInGame(GameManager.GameState.CHANGE_SUIT);
            }
            else if(isBaseDoor && gameManager.CurrentGameState == GameManager.GameState.CHANGE_SUIT)
            {
                _isFirstTimeOpened = true;
                gameManager.SetGameStateInGame(GameManager.GameState.BASE_INTRO);
            }
        }
    }
}
