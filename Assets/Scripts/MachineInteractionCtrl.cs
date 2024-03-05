using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteractionCtrl : MonoBehaviour
{

    [SerializeField]
    private GameObject machineInventoryCanvas;

    private MachineUiCtrl machineUiCtrl;

    private void Awake()
    {
        machineUiCtrl = machineInventoryCanvas.GetComponent<MachineUiCtrl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OpenInventoryUi();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CloseInventoryUi();
        }
    }

    private void OpenInventoryUi()
    {
        machineUiCtrl.UpdateTotalUi();
        machineInventoryCanvas.SetActive(true);
    }

    private void CloseInventoryUi()
    {
        machineInventoryCanvas.SetActive(false);
    }
}
