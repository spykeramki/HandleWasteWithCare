using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameProfilesListUiCtrl : MonoBehaviour
{
    [Serializable]
    public struct UiData
    {
        public List<LoadGameProfileUiCtrl.UiData> LoadGameProfilesData;
    }

    public LoadGameProfileUiCtrl[] loadGameProfiles;

    public LoadingScreenCtrl loadingScreenCtrl;

    public void SetDataInUi(UiData uiData)
    {
        for (int i = 0; i < uiData.LoadGameProfilesData.Count; i++)
        {
            loadGameProfiles[i].SetDataInUi(uiData.LoadGameProfilesData[i], OnClickThisProfile);
        }
    }

    private void OnClickThisProfile(int m_id)
    {
        DataManager.Instance.SetCurrentPlayerIndex(m_id);
        loadingScreenCtrl.ShowLoadingScreen("01Main");
    }
}
