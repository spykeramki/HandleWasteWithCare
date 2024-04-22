using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayInstructionsCtrl : MonoBehaviour
{
    public TextMeshProUGUI instructText;

    public void SetDataAndActivenessInUi(string m_text, bool isActive){
        gameObject.SetActive(isActive);
        if(isActive){
            instructText.text = m_text;
        }
        else{
            instructText.text = string.Empty;
        }
    }
}
