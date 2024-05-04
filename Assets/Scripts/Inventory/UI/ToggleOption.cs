using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToggleOption : MonoBehaviour
{
    private Button _button;

    private Image _image;

    public Button ToggleBtn
    {
        get { return _button; }
    }

    public Image ToggleImage
    {
        get { return _image; }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }
}
