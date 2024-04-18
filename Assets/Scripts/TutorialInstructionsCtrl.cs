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
        }
    }
}
