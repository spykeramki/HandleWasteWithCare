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
        loadGameBtn.onClick.AddListener(OnClickThisProfile);
    }

    public void SetDataInUi(UiData m_uiData)
    {
        profileName.text = m_uiData.name;
        id = m_uiData.id;
        loadGameBtn.interactable = true;
    }

    private void OnClickThisProfile()
    {
        DataManager.Instance.SetCurrentPlayerIndex(id);
        SceneManager.LoadScene("01Main");
        Debug.Log(DataManager.Instance.GetCurrentPlayerData().playerData.name + " current user name");
    }

}
