using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameProfilesListUiCtrl : MonoBehaviour
{
    [Serializable]
    public struct UiData
    {
        public List<LoadGameProfileUiCtrl.UiData> LoadGameProfilesData;
    }

    public LoadGameProfileUiCtrl[] loadGameProfiles;

    public void SetDataInUi(UiData uiData)
    {
        for (int i = 0; i < uiData.LoadGameProfilesData.Count; i++)
        {
            loadGameProfiles[i].SetDataInUi(uiData.LoadGameProfilesData[i]);
        }
    }
}
