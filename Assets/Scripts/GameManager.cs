using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        NEW_ARRIVAL,
        COLLECT_RADIOACTIVE_WASTE,
        AFTER_EFFECTS,
        DISPOSE_RADIOACTIVE_WASTE,
        CHANGE_SUIT,
        COLLECT_BIOHAZARD_WASTE,
        DISPOSE_BIOHAZARD_WASTE,
        FREE_ROAM
    }

    public static GameManager Instance;

    [HideInInspector]
    public UnityEvent<bool> SetPlayerStateToUiMode = new UnityEvent<bool>();

    public GameOverCtrl gameOverCtrl;

    public PauseMenuCtrl pauseMenuCtrl;

    public PlayerStatsUiCtrl playerStatsUiCtrl;

    public RecyclerInventorySystem bioHazardRecyclerInventory;

    public RecyclerInventorySystem radiationRecyclerInventory;

    public List<GarbageCtrl> garbageList;

    public FactsCtrl factsCtrl;
    public FactsCtrl introCtrl;

    public FactsData factsData;
    public GameObject[] playerHudUiElements;

    public TutorialInstructionsCtrl tutorialInstructionsCtrl;

    public PlayInstructionsCtrl playInstructionsCtrl;

    private bool _isGameOver;

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

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
        currentGameState = GameState.NEW_ARRIVAL;
    }

    private void Start()
    {
        PlayerCtrl.LocalInstance.SetPlayerGameData();
        SetMachinesData();
        SetGarbageData();
        SetActivenessOfPlayerHudUi(false);
        SetPlayerGameStateFromData();
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

    private void SetPlayerGameStateFromData()
    {
        DataManager.UserGameData m_userGameData = DataManager.Instance.GetCurrentUserData();
        GameState gameState = (GameState)Enum.Parse(typeof(GameState), m_userGameData.gameState);
        if (gameState!= GameState.NEW_ARRIVAL)
        {
            PlayerCtrl.LocalInstance.SetAnimation(PlayerCtrl.PlayerState.IS_IDLE);
            SetActivenessOfPlayerHudUi(true);
        }
        SetGameStateInGame(gameState);
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

    public void SetGameStateInGame(GameState m_gameState)
    {
        Debug.Log(m_gameState + " executed 3");
        currentGameState = m_gameState;
        switch (m_gameState)
        {
            case GameState.NEW_ARRIVAL:
                introCtrl.SetFactsText(new FactsCtrl.UiData()
                {
                    factsDataList = factsData.intro,
                    onCompletionOfSettingData = PlayerCtrl.LocalInstance.WakeUpPlayer
                });
                break;
            case GameState.COLLECT_RADIOACTIVE_WASTE:
                TutorialInstructionsCtrl.UiData uiData = new TutorialInstructionsCtrl.UiData()
                {
                    gameState = currentGameState,
                    actionToBeExecutedAfterIntro = () =>
                    {
                        SetPlayerStateToUiMode?.Invoke(false);
                        Utilities.Instance.SetSettingsForUi(false);
                        Invoke("SetDataForFactsInformationUi", 2f);
                    }
                };
                SetDataInTutorialInstructions(uiData);
                break;
            case GameState.AFTER_EFFECTS:
                TutorialInstructionsCtrl.UiData uiData2 = new TutorialInstructionsCtrl.UiData()
                {
                    gameState = currentGameState,
                    actionToBeExecutedAfterIntro = () =>
                    {
                        SetPlayerStateToUiMode?.Invoke(false);
                        Utilities.Instance.SetSettingsForUi(false);
                    }
                };
                SetDataInTutorialInstructions(uiData2);
                break;
        }
    }

    private void SetDataInTutorialInstructions(TutorialInstructionsCtrl.UiData m_uiData)
    {
        tutorialInstructionsCtrl.SetDataInUi(m_uiData);
        SetPlayerStateToUiMode?.Invoke(true);
        Utilities.Instance.SetSettingsForUi(true);
    }

    public void SetActivenessOfPlayerHudUi(bool isActive)
    {
        foreach (GameObject item in playerHudUiElements)
        {
            item.SetActive(isActive);
        }
    }

    private void SetDataForFactsInformationUi()
    {
        factsCtrl.SetFactsText(new FactsCtrl.UiData()
        {
            factsDataList = factsData.wastageFacts,
            onCompletionOfSettingData = null
        });
    }

    public void SetDataAndActivenessOfGeneralIntructUi(string m_text, bool isActive){
        playInstructionsCtrl.SetDataAndActivenessInUi(m_text, isActive);
    }
}
