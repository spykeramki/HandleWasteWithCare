using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecontaminatorCtrl : MonoBehaviour
{
    private float _radiationLevel = 0f;

    private float _bioHazardLevel = 0f;

    public ParticleSystem[] sprayers;

    private PlayerCtrl playerCtrl;

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
                Invoke("StartDecontamination", 1f);
            }
        }
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

    private void StartDecontamination()
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
        }
        if (_radiationLevel <= 0)
        {
            _radiationLevel =0;
            playerCtrl.RadiationLevel = _radiationLevel;
            if (_bioHazardLevel <= 0)
            {
                StopSprayers();
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
        }
        if (_bioHazardLevel <= 0)
        {
            _bioHazardLevel=0;
            playerCtrl.BioHazardLevel = _bioHazardLevel;
            if (_radiationLevel <= 0)
            {
                StopSprayers();
            }
            StopCoroutine("ReduceBioHazardLevelSlowly");
        }
    }

    private void StopSprayers()
    {
        foreach (ParticleSystem spray in sprayers)
        {
            spray.Stop();
        }
    }

    private void StartSprayers()
    {
        foreach (ParticleSystem spray in sprayers)
        {
            spray.gameObject.SetActive(true);
            spray.Play();
        }
    }
}
