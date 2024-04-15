using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteractionCtrl : MonoBehaviour
{

    [SerializeField]
    private GameObject machineInventoryCanvas;
    [SerializeField]
    private GameObject machineInventorySecondaryCanvas;

    public MachineUiCtrl machineUiCtrl;

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
        UpdateMachineUi();
        machineInventoryCanvas.SetActive(true);
        machineInventorySecondaryCanvas.SetActive(true);
    }

    private void CloseInventoryUi()
    {
        machineInventoryCanvas.SetActive(false);
        machineInventorySecondaryCanvas.SetActive(false);
    }

    public void UpdateMachineUi()
    {
        machineUiCtrl.UpdateTotalUi();
    }
}
