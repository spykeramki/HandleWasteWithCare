using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContaminationUiCtrl : MonoBehaviour
{
    [Serializable]
    public struct UiData
    {
        public float radiation;
        public float biohazard;
    }

    public TextMeshProUGUI radiationText;
    public TextMeshProUGUI bioHazardText;

    public GameObject contaminatedWarning;
    public GameObject decontaminatedAssurance;

    public GameObject ParentGo;


    public void SetContamination(UiData uiData)
    {
        radiationText.text = (uiData.radiation).ToString("0.00");
        bioHazardText.text = uiData.biohazard.ToString("0.00");

        if(uiData.radiation <= 0 && uiData.biohazard <= 0)
        {
            SetContaminationStatus(false);
        }
        else
        {
            SetContaminationStatus(true);
        }
    }

    private void SetContaminationStatus(bool isContaminated)
    {
        contaminatedWarning.SetActive(isContaminated);
        decontaminatedAssurance.SetActive(!isContaminated);
    }
}
