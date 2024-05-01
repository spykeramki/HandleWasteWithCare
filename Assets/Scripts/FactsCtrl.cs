using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FactsCtrl : MonoBehaviour
{
    public struct UiData
    {
        public List<FactsData.FactData> factsDataList;
        public Action onCompletionOfSettingData;
    }

    public TextMeshProUGUI factText;
    public AudioClip[] defaultLetterClips;
    private PlayerCtrl playerCtrl;

    private int totalStringCount;
    private int currentStringCount = 0;
    private UiData uiData;

    private int _currentAudioIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerCtrl = PlayerCtrl.LocalInstance;
    }

    public void SetFactsText(UiData m_uiData)
    {
        uiData = m_uiData;
        gameObject.SetActive(true);
        totalStringCount = m_uiData.factsDataList.Count;
        SetSpecificText();
    }

    private void SetSpecificText()
    {
        FactsData.FactData factData = uiData.factsDataList[currentStringCount];
        factText.text = factData.text;
        bool hasAudio = factData.audio != null;
        if (hasAudio)
        {
            PlayerCtrl.LocalInstance.PlayPlayerAudio(factData.audio, false, 1.0f);
        }
        StartCoroutine(TextTypeWritingEffect(factData.text, hasAudio));
    }

    private void ResetUi()
    {
        factText.text = string.Empty;
        gameObject.SetActive(false);
    }

    private IEnumerator TextTypeWritingEffect(string m_string, bool hasAudio)
    {
        factText.ForceMeshUpdate();
        int lettersCount = m_string.Length;
        int iterationCount = 0;
        while (iterationCount <lettersCount)
        {
            iterationCount++;
            factText.maxVisibleCharacters = iterationCount;
            if (!hasAudio)
            {
                PlayKeyboardClickSoundsRandom();
            }
            yield return new WaitForSeconds(0.12f);
        }

        currentStringCount++;
        if(currentStringCount< totalStringCount)
        {
            Invoke("SetSpecificText", 2f);
        }
        else
        {
            uiData.onCompletionOfSettingData?.Invoke();
            StopCoroutine("TextTypeWritingEffect");
            ResetUi();
        }
    }

    private void PlayKeyboardClickSoundsRandom(){
        int randomIndex = UnityEngine.Random.Range(0, 4);
        float randomVolume = UnityEngine.Random.Range(0.8f, 1.0f);
        PlayerCtrl.LocalInstance.PlayPlayerAudio(defaultLetterClips[randomIndex], shouldLoop: false, randomVolume);
    }
}
