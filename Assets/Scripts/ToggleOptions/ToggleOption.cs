using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ToggleOption : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onSelect;

    [SerializeField]
    private UnityEvent onUnSelect;

    private void OnEnable()
    {
        if(OnSelectingThisOption != null)
        {
            OnSelectingThisOption.Invoke();
        }
    }

    private void OnDisable()
    {
        if (OnUnSelectingThisOption != null)
        {
            OnUnSelectingThisOption.Invoke();
        }
    }

    public UnityEvent OnSelectingThisOption
    {
        get { return onSelect; }
        set { onSelect = value; }
    }

    public UnityEvent OnUnSelectingThisOption
    {
        get { return onUnSelect; }
        set { onUnSelect = value; }
    }
}
