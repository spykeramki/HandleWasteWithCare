using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraUiCtrl : MonoBehaviour
{
    public RectTransform playerHead;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = PlayerCtrl.LocalInstance.GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        playerHead.localRotation =  Quaternion.Euler(0f, 0f, 360-playerTransform.eulerAngles.y);
    }
}
