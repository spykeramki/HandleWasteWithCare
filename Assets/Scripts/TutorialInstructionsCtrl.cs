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
    public GameObject afterEffectsIntro;
    public GameObject decontaminationUnitIntroBg;
    public GameObject changeSuitIntroBg;

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
                afterEffectsIntro.SetActive(isActive);
                break;
            case GameManager.GameState.DECONTAMINATION_UNIT:
                decontaminationUnitIntroBg.SetActive(isActive);
                break;
            case GameManager.GameState.CHANGE_SUIT:
                changeSuitIntroBg.SetActive(isActive);
                break;
        }
    }

    public void InvokeGoToBaseIntroUiUi()
    {
        timeToRecycleIntro.SetActive(false);
        goToBaseIntroUi.SetActive(true);
        //Invoke("ActiveOfGoToBaseIntroUiUi", 2f);
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
