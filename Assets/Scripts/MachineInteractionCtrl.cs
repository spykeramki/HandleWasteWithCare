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

    private bool isPlayerEnteredFirstTime = false;

    private string instructionText = "Use 'Left Mouse Button' to Interact with UI";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.SetDataAndActivenessOfGeneralIntructUi(instructionText, true);
            OpenInventoryUi();

            if(!isPlayerEnteredFirstTime && machineUiCtrl.MachineRecycleType== GarbageManager.GarbageType.RADIOACTIVE &&
                GameManager.Instance.CurrentGameState == GameManager.GameState.BASE_INTRO)
            {
                isPlayerEnteredFirstTime = true;
                GameManager.Instance.SetGameStateInGame(GameManager.GameState.DISPOSE_RADIOACTIVE_WASTE);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SetDataAndActivenessOfGeneralIntructUi(string.Empty, false);
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
