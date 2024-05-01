using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuUICtrl : MonoBehaviour
{
    public Button YesBtnToQuit;
    public Button loadGameBtn;

    public LoadGameProfilesListUiCtrl loadGameProfilesUiCtrl;

    public AudioSource clickAudioSource;

    void Start()
    {
        YesBtnToQuit.onClick.AddListener(QuitGame);
        loadGameBtn.onClick.AddListener(OnClickLoadBtn);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnClickLoadBtn()
    {
        LoadGameProfilesListUiCtrl.UiData loadProfilesUiData = DataManager.Instance.PrepareDataForLoadGameProfiles();

        loadGameProfilesUiCtrl.SetDataInUi(loadProfilesUiData);
        loadGameProfilesUiCtrl.gameObject.SetActive(true);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public void PlayClickAudio(){
        clickAudioSource.Play();
    }
}
