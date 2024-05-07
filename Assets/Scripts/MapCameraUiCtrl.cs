using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraUiCtrl : MonoBehaviour
{
    public RectTransform playerHead;

    public RectTransform basePointer;

    public Transform baseTransform;

    private Transform playerTransform;

    private bool _showPointer = false;
    private GameObject basePointerGo;

    private void Start()
    {
        playerTransform = PlayerCtrl.LocalInstance.GetComponent<Transform>();
        basePointerGo = basePointer.gameObject;
        basePointerGo.SetActive(false);
    }

    private void Update()
    {
        ManageIcon(baseTransform, basePointer);
    }

    private void LateUpdate()
    {
        playerHead.localRotation =  Quaternion.Euler(0f, 0f, 360-playerTransform.eulerAngles.y);
    }

    //Controls the position of base icon with respect to player position
    private void ManageIcon(Transform objectTransform, RectTransform iconTransform)
    {
        Vector3 baseDirectionFromPlayer = objectTransform.position - playerTransform.position;
        if (baseDirectionFromPlayer.magnitude > 20f && !_showPointer)
        {
            _showPointer = true;
            iconTransform.gameObject.SetActive(true);
        }
        else if (baseDirectionFromPlayer.magnitude <= 20f && _showPointer)
        {
            _showPointer = false;
            iconTransform.gameObject.SetActive(false);
        }
        Vector3 basePointerDirection = new Vector3(baseDirectionFromPlayer.x, baseDirectionFromPlayer.z, 0f).normalized;
        iconTransform.localPosition = (playerHead.localPosition + basePointerDirection * 100f);
    }
}
