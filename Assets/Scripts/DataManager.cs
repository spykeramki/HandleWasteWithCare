using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;


    [Serializable]
    public struct PlayerData
    {
        public string name;
    }

    [Serializable]
    public struct SingleSaveData
    {
        public PlayerData playerData;
    }

    [Serializable]
    public struct SaveData
    {
        public List<SingleSaveData> playerDataList;
    }

    [SerializeField, HideInInspector]
    private SaveData savedData;

    private string savePath = "";

    string saveDataJsonString = string.Empty;

    private int currentPlayerDataIndex = -1;

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
        savedData = new SaveData();
    }

    private void Start()
    {
        savePath = Application.persistentDataPath + @"\hwcSaveData.txt";
        GetSavedData();
    }

    public void SetPlayerData(PlayerData m_playerData)
    {
        currentPlayerDataIndex = savedData.playerDataList.FindIndex(each => each.playerData.name == m_playerData.name);
        SingleSaveData playerSave;
        if (currentPlayerDataIndex < 0)
        {
            playerSave = new SingleSaveData() { playerData = m_playerData };
            savedData.playerDataList.Add(playerSave);
        }
        else
        {
            playerSave = savedData.playerDataList[currentPlayerDataIndex];
            playerSave.playerData = m_playerData;
            savedData.playerDataList[currentPlayerDataIndex] = playerSave;
        }
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
        int playerIndex = savedData.playerDataList.FindIndex(each => each.playerData.name == m_name);
        return playerIndex >= 0;
    }

}
