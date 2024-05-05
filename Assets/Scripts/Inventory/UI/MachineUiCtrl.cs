using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private float _currentRecycleTime = 0;

    private InventorySlotUiCtrl.UiData _currentRecyclingSlotData;

    private bool _isFirstTimeRecyclingDone = false;

    private void Start()
    {
        playerTransferButton.onClick.AddListener(OnClickTransferToMachineInventoryButton);
        machineTransferButton.onClick.AddListener(OnClickTransferFromMachineInventoryButton);
        recycleButton.onClick.AddListener(SetRecyclingProcess);
        recycleButton?.gameObject.SetActive( isRecycler ? true : false );
        recycleButton.interactable = false;
        recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0f);

    }

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
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = playerInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData playerItemData = PlayerCtrl.LocalInstance.PlayerInventory.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //inventorySlotUiCtrl.RemoveItemFromSlot();
            if(inventorySlotUiCtrl.CurrentSlotData.garbageType == GarbageManager.GarbageType.NONE || inventorySlotUiCtrl.CurrentSlotData.garbageType == machineRecycleType)
            {
                PlayerCtrl.LocalInstance.PlayerInventory.RemoveItem(playerItemData);
                inventorySystem.AddItem(playerItemData);
            }
        }

        if (inventorySlotUiCtrls.Count != 0) 
        {
            recycleButton.interactable = true;
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }

    public void OnClickTransferFromMachineInventoryButton()
    {
        GameManager.Instance.PlayMachineClickAudio();
        List<InventorySlotUiCtrl> inventorySlotUiCtrls = machineInventoryUiCtrl.InventorySlotList.FindAll(each => each.IsSelected);
        for (int i = 0; i < inventorySlotUiCtrls.Count; i++)
        {
            InventorySlotUiCtrl inventorySlotUiCtrl = inventorySlotUiCtrls[i];
            InventorySystem.InventoryItemData machineItemData = inventorySystem.GetInventoryItemsData()[inventorySlotUiCtrl.CurrentSlotData.garbageType.ToString()];
            //inventorySlotUiCtrl.RemoveItemFromSlot();

            inventorySystem.RemoveItem(machineItemData);
            PlayerCtrl.LocalInstance.PlayerInventory.AddItem(machineItemData);
        }

        if (inventorySlotUiCtrls.Count != 0)
        {
            UpdateTotalUi();
            PlayerCtrl.LocalInstance.PlayerInventory.UpdateDataInInvetoryUi();
        }
    }



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
            recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 1f);
            StartCoroutine("StartRecycling");
        }
    }

    private IEnumerator StartRecycling()
    {
        GameManager.Instance.PlayMachineClickAudio();
        machineAudioSource.clip = Utilities.Instance.gameAudioClips.machineRunning;
        machineAudioSource.Play();
        while (_currentRecycleTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentRecycleTime -= 1f;
        }
        if (_currentRecycleTime <= 0)
        {
            _currentRecyclingSlotData.inventoryItemUiData.count -= 1;
            
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
            recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0);
            recycleButton.interactable = false;
            machineAudioSource.clip = Utilities.Instance.gameAudioClips.machineIdle;
            machineAudioSource.Play();
            GameManager.Instance.CheckAndSetPlayerWin();
        }
    }

    private void OnDestroy()
    {
        recyclingMaterial.SetColor("_EmissionColor", recyclingColor * 0);
    }
}
