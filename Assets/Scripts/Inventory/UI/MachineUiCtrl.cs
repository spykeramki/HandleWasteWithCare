using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//controls total machine methods and UI
public class MachineUiCtrl : MonoBehaviour
{
    [SerializeField]
    private InventoryUiContainerCtrl playerInventoryUiCtrl;

    [SerializeField]
    private InventoryUiContainerCtrl machineInventoryUiCtrl;

    [SerializeField]
    private InventorySystem inventorySystem;

    [SerializeField]
    private Button playerTransferButton;

    [SerializeField]
    private Button machineTransferButton;

    [SerializeField]
    private GarbageManager.GarbageType machineRecycleType = GarbageManager.GarbageType.NONE;
    public GarbageManager.GarbageType MachineRecycleType
    {
        get { return machineRecycleType; }
    }

    public Color recyclingColor;

    public Material recyclingMaterial;

    [SerializeField]
    private Button recycleButton;

    public AudioSource machineAudioSource;

    public bool isRecycler = false;

    public GameObject timerTextCanvasGo;
    public TextMeshProUGUI timerText;

    private float _currentRecycleTime = 0;

    private InventorySlotUiCtrl.UiData _currentRecyclingSlotData;

    private bool _isFirstTimeRecyclingDone = false;

    private void Start()
    {
        //assigning events for each button click in machine UI
        playerTransferButton.onClick.AddListener(OnClickTransferToMachineInventoryButton);
        machineTransferButton.onClick.AddListener(OnClickTransferFromMachineInventoryButton);
        recycleButton.onClick.AddListener(SetRecyclingProcess);
        recycleButton?.gameObject.SetActive( isRecycler ? true : false );
        recycleButton.interactable = false;
        recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0f);

    }

    //When Item is transferred between player and machine, all the items in the UI are updated to avoid edge cases
    public void UpdateTotalUi()
    {
        playerInventoryUiCtrl.RemoveAllItemsFromSlots();
        machineInventoryUiCtrl.RemoveAllItemsFromSlots();
        Dictionary<string, InventorySystem.InventoryItemData> playerItemsData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData();
        Dictionary<string, InventorySystem.InventoryItemData> machineItemsData = inventorySystem.GetInventoryItemsData();

        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(playerItemsData, machineRecycleType));
        machineInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(machineItemsData, machineRecycleType));

    }

    public void UpdateDataInUi()
    {
        Dictionary<string, InventorySystem.InventoryItemData> playerItemsData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData();
        Dictionary<string, InventorySystem.InventoryItemData> machineItemsData = inventorySystem.GetInventoryItemsData();

        playerInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(playerItemsData, machineRecycleType));
        machineInventoryUiCtrl.SetDataInUi(Utilities.Instance.PrepareDataForInventoryUi(machineItemsData, machineRecycleType));
    }

    public void OnClickTransferToMachineInventoryButton()
    {
        GameManager.Instance.PlayMachineClickAudio();
        //Getting selected items
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = playerInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData playerItemData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //Removing Item in the inventory of player and adding it to machine inventory.
            if(inventorySlotUiCtrl.CurrentSlotData.garbageType == GarbageManager.GarbageType.NONE || inventorySlotUiCtrl.CurrentSlotData.garbageType == machineRecycleType)
            {
                PlayerCtrl.LocalInstance.PlayerInventory.RemoveItem(playerItemData);
                inventorySystem.AddItem(playerItemData);
            }
        }
    
        if (inventorySlotUiCtrls.Count != 0) 
        {
            recycleButton.interactable = true;
            //Updating UI of the inventory
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }

    public void OnClickTransferFromMachineInventoryButton()
    {
        GameManager.Instance.PlayMachineClickAudio();
        //getting selected items
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = machineInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData machineItemData = inventorySystem.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            
            //Removing Item in the inventory of machine and adding it to player inventory.
            inventorySystem.RemoveItem(machineItemData);
            PlayerCtrl.LocalInstance.PlayerInventory.AddItem(machineItemData);
        }

        if (inventorySlotUiCtrls.Count != 0)
        {
            //updating total UI
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }

    //Controls the recycling process and machine emission material
    private void SetRecyclingProcess()
    {
        StopAllCoroutines();
        _currentRecyclingSlotData = machineInventoryUiCtrl.InventorySlotList[0].CurrentSlotData;
        
        if (_currentRecycleTime <= 0f)
        {
            _currentRecycleTime = Utilities.Instance.GetRecyclingTimeAsPerGarbageType(_currentRecyclingSlotData.garbageType);
        }
        if (machineInventoryUiCtrl.InventorySlotList[0].IsFilled)
        {
            //Setting emission to material in URP
            recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 1f);
            StartCoroutine("StartRecycling");
        }
    }

    //Controls the recycling timer
    private IEnumerator StartRecycling()
    {
        timerTextCanvasGo.SetActive(true);
        GameManager.Instance.PlayMachineClickAudio();
        machineAudioSource.clip = Utilities.Instance.gameAudioClips.machineRunning;
        machineAudioSource.Play();
        while (_currentRecycleTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentRecycleTime -= 1f;
            timerText.text = _currentRecycleTime.ToString();
        }
        if (_currentRecycleTime <= 0)
        {
            _currentRecyclingSlotData.inventoryItemUiData.count -= 1;
            timerText.text = "0";
            inventorySystem.RemoveSingleItemOfType(_currentRecyclingSlotData.garbageType);
            if (_currentRecyclingSlotData.inventoryItemUiData.count <= 0)
            {
                UpdateTotalUi();
            }
            else
            {
                UpdateDataInUi();
            }
            StopCoroutine("StartRecycling");
            ContinueRecycling();
        }
    }

    //if we have more than one items in the machine, then the recycling continues untill the end
    private void ContinueRecycling()
    {
        if (!_isFirstTimeRecyclingDone && machineRecycleType == GarbageManager.GarbageType.RADIOACTIVE && 
            GameManager.Instance.CurrentGameState == GameManager.GameState.DISPOSE_RADIOACTIVE_WASTE)
        {
            GameManager.Instance.SetGameStateInGame(GameManager.GameState.FREE_ROAM);
        }
        if (inventorySystem.GetInventoryItemsData().Count > 0)
        {
            SetRecyclingProcess();
        }
        else
        {
            //This is end of Recycling process
            recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0);
            recycleButton.interactable = false;
            machineAudioSource.clip = Utilities.Instance.gameAudioClips.machineIdle;
            machineAudioSource.Play();
            timerTextCanvasGo.SetActive(false);
            GameManager.Instance.CheckAndSetPlayerWin();
        }
    }

    //If the game was stopped in between them the emission material will be reset for next game
    private void OnDestroy()
    {
        recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0);
    }
}
