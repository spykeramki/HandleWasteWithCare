using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCameraCtrl : MonoBehaviour
{
    Transform playerTransform;

    private void Start()
    {
        playerTransform = PlayerCtrl.LocalInstance.GetComponent<Transform>();
        RenderPipelineManager.beginCameraRendering += BeginOfThisCamRender;

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= BeginOfThisCamRender;
    }

    private void BeginOfThisCamRender(ScriptableRenderContext context, Camera cam)
    {
        if(cam.CompareTag("MapCamera"))
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
        }
    }
}
