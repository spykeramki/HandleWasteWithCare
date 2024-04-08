using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    public void ChangeParentToWake()
    {
        PlayerCtrl.LocalInstance.ChangeCamToWake();
    }
}
