using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuUICtrl : MonoBehaviour
{
    public Button PlayBtn;
    public Button YesBtnToQuit;

    void Start()
    {
        PlayBtn.onClick.AddListener(StartGame);
        YesBtnToQuit.onClick.AddListener(QuitGame);
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

    private void StartGame()
    {
        SceneManager.LoadScene("01main");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
