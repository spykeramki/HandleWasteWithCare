using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecontaminatorCtrl : MonoBehaviour
{
    private float _radiationLevel = 0f;

    private float _bioHazardLevel = 0f;

    public ParticleSystem[] sprayers;
    public AudioSource[] sprayersAudio;
    
    public AudioSource scanAudio;

    public Animator scanningMeshAnim;
    
    private PlayerCtrl playerCtrl;

    public ContaminationUiCtrl contaminationUiCtrl;

    public void SetRadiationLevel(float level)
    {
        _radiationLevel = level;
    }

    public void SetBioHazardLevel(float level)
    {
        _bioHazardLevel = level;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerCtrl = other.GetComponent<PlayerCtrl>();
            SetRadiationLevel(playerCtrl.RadiationLevel);
            SetBioHazardLevel(playerCtrl.BioHazardLevel);
            if (_bioHazardLevel > 0 || _radiationLevel > 0f)
            {
                Invoke("ScanPlayerForContamination", 1f);
            }
        }
    }

    private void ScanPlayerForContamination()
    {
        scanningMeshAnim.SetTrigger("Scan");
        scanAudio.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        StopSprayers();
        if (other.tag == "Player")
        {
            StopCoroutine("ReduceRadiationLevelSlowly");
            StopCoroutine("ReduceBioHazardLevelSlowly");
        }
    }

    public void ShowScannedDataForDecontamination()
    {
        contaminationUiCtrl.ParentGo.SetActive(true);
        SetDataInContaminationUi();
        Invoke("StartDecontamination", 1f);
    }

    public void StartDecontamination()
    {
        StartSprayers();
        if (_radiationLevel > 0f)
        {
            ReduceLevel(isRadiationLevel : true);
        }
        if( _bioHazardLevel > 0f)
        {
            ReduceLevel(isRadiationLevel : false);
        }
    }

    public void ReduceLevel(bool isRadiationLevel)
    {
        if (isRadiationLevel)
        {
            StartCoroutine("ReduceRadiationLevelSlowly");
        }
        else
        {
            StartCoroutine("ReduceBioHazardLevelSlowly");
        }
    }

    private IEnumerator ReduceRadiationLevelSlowly()
    {
        while (_radiationLevel > 0)
        {
            yield return new WaitForEndOfFrame();
            _radiationLevel-= Time.deltaTime;
            playerCtrl.RadiationLevel = _radiationLevel;
            GameManager.Instance.playerStatsUiCtrl.SetRadiationInUi(_radiationLevel);
            SetDataInContaminationUi();
        }
        if (_radiationLevel <= 0)
        {
            _radiationLevel =0;
            playerCtrl.RadiationLevel = _radiationLevel;
            GameManager.Instance.playerStatsUiCtrl.SetRadiationInUi(_radiationLevel);
            SetDataInContaminationUi();
            if (_bioHazardLevel <= 0)
            {
                StopSprayers();
                Invoke("DeactivateContaminationUi", 2f);
            }
            StopCoroutine("ReduceRadiationLevelSlowly");
        }
    }

    private IEnumerator ReduceBioHazardLevelSlowly()
    {
        while (_bioHazardLevel > 0)
        {
            yield return new WaitForEndOfFrame();
            _bioHazardLevel -= Time.deltaTime;
            playerCtrl.BioHazardLevel = _bioHazardLevel;
            GameManager.Instance.playerStatsUiCtrl.SetBioHazardInUi(_bioHazardLevel);
            SetDataInContaminationUi();
        }
        if (_bioHazardLevel <= 0)
        {
            _bioHazardLevel=0;
            playerCtrl.BioHazardLevel = _bioHazardLevel;
            GameManager.Instance.playerStatsUiCtrl.SetBioHazardInUi(_bioHazardLevel);
            SetDataInContaminationUi();
            if (_radiationLevel <= 0)
            {
                StopSprayers();
                Invoke("DeactivateContaminationUi", 2f);
            }
            StopCoroutine("ReduceBioHazardLevelSlowly");
        }
    }

    private void SetDataInContaminationUi()
    {
        contaminationUiCtrl.SetContamination(new ContaminationUiCtrl.UiData()
        {
            biohazard = _bioHazardLevel,
            radiation = _radiationLevel
        });
    }

    private void DeactivateContaminationUi()
    {
        contaminationUiCtrl.ParentGo.SetActive(false);
    }

    private void StopSprayers()
    {
        foreach (ParticleSystem spray in sprayers)
        {
            spray.Stop();
        }
        foreach(AudioSource a_source in sprayersAudio){
            a_source.Stop();
        }
    }

    private void StartSprayers()
    {
        foreach (ParticleSystem spray in sprayers)
        {
            spray.gameObject.SetActive(true);
            spray.Play();
        }
        foreach(AudioSource a_source in sprayersAudio){
            a_source.Play();
        }
    }
}
