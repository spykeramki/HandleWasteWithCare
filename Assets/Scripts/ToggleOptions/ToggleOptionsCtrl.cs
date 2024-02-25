using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptionsCtrl : MonoBehaviour
{
    [SerializeField]
    private List<ToggleOption> optionsList;

    private ToggleOption ValueObject;

    private Button _button;

    private int _currentActiveIndex = 0;

    private Action updateUiAction;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        //DisableAllValues();
        _button.onClick.AddListener(ChangeValuesInARow);
        
    }

    private void ChangeValuesInARow()
    {
        _currentActiveIndex ++;
        if(_currentActiveIndex >= optionsList.Count)
        {
            _currentActiveIndex = 0;
        }
        DisableAllValues();
        ValueObject = optionsList[_currentActiveIndex];
        ValueObject.gameObject.SetActive(true);
        updateUiAction.Invoke();
    }

    public void SetDataInUi(ToggleOption.EquipmentType data, Action updateDataAction = null)
    {
        updateUiAction = updateDataAction;
        DisableAllValues();
        if(data.playerProtectionSuitType == EquipStationCtrl.PlayerProtectionSuitType.NONE && data.playerGunType != EquipStationCtrl.GunType.NONE)
        {
            for (int i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].ThisEquipmentType.playerGunType == data.playerGunType)
                {
                    ValueObject = optionsList[i];
                    _currentActiveIndex = i;
                    ValueObject.gameObject.SetActive(true);
                    return;
                }
            }
        }
        else if(data.playerProtectionSuitType != EquipStationCtrl.PlayerProtectionSuitType.NONE && data.playerGunType == EquipStationCtrl.GunType.NONE)
        {
            for (int i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].ThisEquipmentType.playerProtectionSuitType == data.playerProtectionSuitType)
                {
                    ValueObject = optionsList[i];
                    _currentActiveIndex = i;
                    ValueObject.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    private void DisableAllValues()
    {
        for (int i = 0; i < optionsList.Count; i++)
        {
            optionsList[i].gameObject.SetActive(false);
        }
    }

    public ToggleOption.EquipmentType GetActiveEquipmentType()
    {
        return ValueObject.ThisEquipmentType;
    }
}
