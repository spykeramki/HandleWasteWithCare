using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorManagerCtrl : MonoBehaviour
{

    public Animator anim;

    private bool isDoorOpened = false;
    private string instructionText = "Press 'E' to open the Door";

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
            }

        }
    }
}
