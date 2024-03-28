using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameProfileUiCtrl : MonoBehaviour
{
    [Serializable]
    public struct UiData
    {
        public string name;
        public int id;
    }

    [SerializeField]
    private TextMeshProUGUI profileName;

    [SerializeField]
    private Button loadGameBtn;

    private int id;

    private void Start()
    {
        
    }

    public void SetDataInUi(UiData m_uiData, Action<int> OnClickThisProfile)
    {
        loadGameBtn.onClick.AddListener(() => { OnClickThisProfile.Invoke(id); });
        profileName.text = m_uiData.name;
        id = m_uiData.id;
        loadGameBtn.interactable = true;
    }


}
