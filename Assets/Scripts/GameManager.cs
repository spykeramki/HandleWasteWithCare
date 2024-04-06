using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public UnityEvent<bool> SetPlayerStateToUiMode = new UnityEvent<bool>();

    public GameOverCtrl gameOverCtrl;

    public PauseMenuCtrl pauseMenuCtrl;

    public PlayerStatsUiCtrl playerStatsUiCtrl;

    public RecyclerInventorySystem bioHazardRecyclerInventory;

    public RecyclerInventorySystem radiationRecyclerInventory;

    public List<GarbageCtrl> garbageList;

    private bool _isGameOver;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !_isGameOver)
        {
            pauseMenuCtrl.OnClickResumeBtn();
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance!= null && Instance != this) { 
            Destroy(this);
        }
    }

    private void Start()
    {
        PlayerCtrl.LocalInstance.SetPlayerGameData();
        SetMachinesData();
        SetGarbageData();
    }

    public void SetGameOver()
    {
        _isGameOver = true;
        SetPlayerStateToUiMode?.Invoke(true);
        Utilities.Instance.SetSettingsForUi(true);
        gameOverCtrl.gameObject.SetActive(true);
    }

    public DataManager.MachinesData GetMachinesData()
    {
        DataManager.MachinesData machinesData = new DataManager.MachinesData();
        machinesData.bioHazardWaste = bioHazardRecyclerInventory.GetInventoryItemsData().Count;
        machinesData.radiationWaste = radiationRecyclerInventory.GetInventoryItemsData().Count;
        return machinesData;
    }

    private void SetMachinesData()
    {
        DataManager.UserGameData m_userGameData = DataManager.Instance.GetCurrentUserData();
        if (!m_userGameData.Equals(null))
        {
            DataManager.MachinesData machinesData = DataManager.Instance.GetCurrentUserData().machinesData;
            if(machinesData.bioHazardWaste != 0)
            {
                InventorySystem.InventoryItemData bioHazardInventoryItemData = new InventorySystem.InventoryItemData()
                {
                    count = machinesData.bioHazardWaste,
                    garbageType = GarbageManager.GarbageType.BIO_HAZARD
                };
                bioHazardRecyclerInventory.AddItem(bioHazardInventoryItemData);
                bioHazardRecyclerInventory.machineInteractionCtrl.UpdateMachineUi();
            }

            if (machinesData.radiationWaste != 0)
            {
                InventorySystem.InventoryItemData radiationInventoryItemData = new InventorySystem.InventoryItemData()
                {
                    count = machinesData.radiationWaste,
                    garbageType = GarbageManager.GarbageType.RADIOACTIVE
                };
                radiationRecyclerInventory.AddItem(radiationInventoryItemData);
                radiationRecyclerInventory.machineInteractionCtrl.UpdateMachineUi();
            }
        }
    }

    private void SetGarbageData()
    {
        DataManager.UserGameData m_userGameData = DataManager.Instance.GetCurrentUserData();
        if (!m_userGameData.Equals(null))
        {
            for (int i=0; i< garbageList.Count; i++)
            {
                for (int j = 0; j < m_userGameData.garbageDetails.Count; j++)
                {
                    DataManager.GarbageDetails garbageDetails = m_userGameData.garbageDetails[j];
                    if (garbageList[i].Id == garbageDetails.id)
                    {
                        GarbageCtrl.GarbageState garbageStatus = (GarbageCtrl.GarbageState)Enum.Parse(typeof(GarbageCtrl.GarbageState), garbageDetails.garbageStatus);
                        garbageList[i].SetGarbageState(m_garbageState: garbageStatus);
                    }
                }
            }
        }
    }

    public List<DataManager.GarbageDetails> GetGarbageDetails()
    {
        List<DataManager.GarbageDetails> playerInventoryGarbage = new List<DataManager.GarbageDetails>();
        foreach (GarbageCtrl item in garbageList)
        {
            DataManager.GarbageDetails garbageDetails = new DataManager.GarbageDetails();
            garbageDetails.garbageType = item.GarbageType.ToString();
            garbageDetails.id = item.Id;
            garbageDetails.garbageStatus = item.CurrentGarbageState.ToString();
            playerInventoryGarbage.Add(garbageDetails);
        }
        return playerInventoryGarbage;
    }
}
