using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptionsCtrl : MonoBehaviour
{

    public List<GameObject> _options;

    private GameObject ValueObject;

    private Button _button;

    private int _currentActiveIndex = 0;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        DisableAllValues();
        ValueObject = _options[_currentActiveIndex];
        ValueObject.SetActive(true);
        _button.onClick.AddListener(ChangeValuesInARow);
        
    }

    private void ChangeValuesInARow()
    {
        _currentActiveIndex ++;
        if(_currentActiveIndex >= _options.Count)
        {
            _currentActiveIndex = 0;
        }
        DisableAllValues();
        ValueObject = _options[_currentActiveIndex];
        ValueObject.SetActive(true);
    }

    private void DisableAllValues()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            _options[i].SetActive(false);
        }
    }
}
