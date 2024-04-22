using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInstructionsCtrl : MonoBehaviour
{
    public struct UiData
    {
        public GameManager.GameState gameState;
        public Action actionToBeExecutedAfterIntro;
    }

    public GameObject collectRadMatIntro;
    public GameObject timeToRecycleIntro;
    public GameObject goToBaseIntroUi;
    public GameObject decontaminationIntro;

    private UiData uiData;

    public void SetDataInUi(UiData m_uiData)
    {
        uiData = m_uiData;
        SetActivenessOfUi(true);
    }

    public void OnClickLetsGoBtn()
    {
        SetActivenessOfUi(false);
        uiData.actionToBeExecutedAfterIntro?.Invoke();
    }

    private void SetActivenessOfUi(bool isActive)
    {
        switch (uiData.gameState)
        {
            case GameManager.GameState.COLLECT_RADIOACTIVE_WASTE:
                collectRadMatIntro.SetActive(isActive);
                break;
            case GameManager.GameState.AFTER_EFFECTS:
                ResetDecontaminationIntro();
                decontaminationIntro.SetActive(isActive);
                break;
        }
    }

    public void InvokeGoToBaseIntroUiUi()
    {
        timeToRecycleIntro.SetActive(false);
        Invoke("ActiveOfGoToBaseIntroUiUi", 2f);
    }

    public void ActiveOfGoToBaseIntroUiUi()
    {
        goToBaseIntroUi.SetActive(true);
    }

    private void ResetDecontaminationIntro()
    {
        timeToRecycleIntro.SetActive(true);
        goToBaseIntroUi.SetActive(false);
    }
}
