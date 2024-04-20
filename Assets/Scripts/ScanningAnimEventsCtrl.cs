using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanningAnimEventsCtrl : MonoBehaviour
{
    public DecontaminatorCtrl decontaminatorCtrl;
    public void OnCompleteScanning()
    {
        decontaminatorCtrl.ShowScannedDataForDecontamination();
    }
}
