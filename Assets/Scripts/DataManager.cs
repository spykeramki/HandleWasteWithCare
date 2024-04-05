using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private string _sceneName;


    [Serializable]
    public struct PlayerDetails
    {
        public string name;
    }

    [Serializable]
    public struct GarbageDetails
    {
        public string garbageType;
        public int count;
    }

    [Serializable]
    public struct PlayerGameData
    {
        public Vector3 position;
        public float health;
        public float radiationLevel;
        public float bioHazardLevel;
        public List<GarbageDetails> inventoryGarbage;
        public string playerEquipmentType;
    }

    [Serializable]
    public struct MachinesData
    {
        public int bioHazardWaste;
        public int radiationWaste;
    }

    [Serializable]
    public struct UserGameData
    {
        public PlayerDetails playerDetails;
        public PlayerGameData playerGameData;
        public MachinesData machinesData;
        public int id;
    }

    [Serializable]
    public struct SaveData
    {
        public List<UserGameData> playerDataList;
    }

    [SerializeField, HideInInspector]
    private SaveData savedData;

    private string savePath = "";

    string saveDataJsonString = string.Empty;

    private int currentPlayerDataId = -1;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
        savedData = new SaveData() { playerDataList = new List<UserGameData>() };
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        savePath = Application.persistentDataPath + @"\hwcSaveData.txt";
        Debug.Log(savePath);
        GetSavedData();
    }

    public void SetNewPlayerData(PlayerDetails m_playerData)
    {
        UserGameData userGameData  = new UserGameData() { playerDetails = m_playerData, id = GetPlayerDataList().Count };
        savedData.playerDataList.Add(userGameData);

        SetCurrentPlayerIndex(userGameData.id);
        SetSaveData();
    }

    public void SetSaveData()
    {
        saveDataJsonString = JsonUtility.ToJson(savedData);
        File.WriteAllText(savePath, saveDataJsonString);
    }

    private void GetSavedData()
    {
        if(!File.Exists(savePath))
        {
            return;
        }
        saveDataJsonString = File.ReadAllText(savePath);

        savedData = JsonUtility.FromJson<SaveData>(saveDataJsonString);
    } 

    public bool PlayerNameAlreadyExists(string m_name)
    {
        int playerIndex = -1;
        playerIndex = GetPlayerDataList().FindIndex(each => each.playerDetails.name == m_name);
        return playerIndex >= 0;
    }

    public UserGameData GetCurrentUserData()
    {
        return GetPlayerDataList().Find(each => each.id == currentPlayerDataId);
    }

    public void SetCurrentPlayerIndex(int id)
    {
        currentPlayerDataId = id;
    }

    private List<UserGameData> GetPlayerDataList()
    {
        if(savedData.playerDataList == null)
        {
            return new List<UserGameData>();
        }
        else
        {
            return savedData.playerDataList;
        }
    }


    public LoadGameProfilesListUiCtrl.UiData PrepareDataForLoadGameProfiles()
    {

        List<LoadGameProfileUiCtrl.UiData> uiData = new List<LoadGameProfileUiCtrl.UiData>();
        
            for (int i = GetPlayerDataList().Count - 1; i >= 0; i--)
            {
                LoadGameProfileUiCtrl.UiData data = new LoadGameProfileUiCtrl.UiData();
                data.name = savedData.playerDataList[i].playerDetails.name;
                data.id = savedData.playerDataList[i].id;
                uiData.Add(data);
            }

        return new LoadGameProfilesListUiCtrl.UiData() { LoadGameProfilesData = uiData };
    }

    private void OnSceneLoaded(Scene m_scene, LoadSceneMode m_loadSceneMode)
    {
        _sceneName = m_scene.name;
    }

    public void SaveDataOfCurrentUser()
    {
        UserGameData userGameData = GetPlayerDataList()[currentPlayerDataId];
        userGameData.playerGameData = PlayerCtrl.LocalInstance.GetPlayerGameData();
        userGameData.machinesData = GameManager.Instance.GetMachinesData();

        savedData.playerDataList[currentPlayerDataId] = userGameData;
        SetSaveData();
    }
}
