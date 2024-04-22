using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuCtrl : MonoBehaviour
{
    public Button resumeBtn;
    public Button saveBtn;
    public Button mainMenuBtn;
    public Button quitBtn;


    public Button quitConfirmationBtn;
    public Button quitCancelBtn;

    public GameObject parentGo;
    public GameObject quitConfirmationGo;
    public GameObject savingUiGo;

    public GameObject opionsGo;
    public GameObject optionsPanelGo;


    private bool isPausing = false;

    private void Start()
    {
        resumeBtn.onClick.AddListener(OnClickResumeBtn);
        saveBtn.onClick.AddListener(OnClickSaveBtn);
        mainMenuBtn.onClick.AddListener(OnClickMainMenuBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
        quitConfirmationBtn.onClick.AddListener(OnConfirmationOfQuit);
        quitCancelBtn.onClick.AddListener(OnCancelOfQuit);
    }

    public void OnClickResumeBtn()
    {
        isPausing = !isPausing;
        GameManager.Instance.SetPlayerStateToUiMode?.Invoke(isPausing);
        parentGo.SetActive(isPausing);
        ResetPauseOptions();
    }

    private void OnClickSaveBtn()
    {
        StartCoroutine("ShowSavingData");
        DataManager.Instance.SaveDataOfCurrentUser();
    }

    private void OnClickMainMenuBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("00Start");
    }

    private void OnClickQuitBtn()
    {
        optionsPanelGo.SetActive(false);
        quitConfirmationGo.SetActive(true);
    }

    private IEnumerator ShowSavingData()
    {
        opionsGo.SetActive(false);
        savingUiGo.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        opionsGo.SetActive(true);
        savingUiGo.SetActive(false);
    }

    private void OnConfirmationOfQuit()
    {
        Application.Quit();
    }

    private void OnCancelOfQuit()
    {
        quitConfirmationGo.SetActive(false);
        optionsPanelGo.SetActive(true);
    }

    private void ResetPauseOptions()
    {
        quitConfirmationGo.SetActive(false);
        savingUiGo.SetActive(false);
        optionsPanelGo.SetActive(true);
        opionsGo.SetActive(true);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
