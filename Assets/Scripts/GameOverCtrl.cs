using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCtrl : MonoBehaviour
{
    public Button restartBtn;
    public Button menuBtn;
    public Button quitBtn;

    private void Start()
    {
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        menuBtn.onClick.AddListener(OnClickMenuBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    private void OnClickRestartBtn()
    {
        GameManager.Instance.PlayClickAudio();
        SceneManager.LoadScene("01Main");
    }

    private void OnClickMenuBtn()
    {
        GameManager.Instance.PlayClickAudio();
        DestroyImmediate(DataManager.Instance.gameObject);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("00Start");
    }

    private void OnClickQuitBtn()
    {
        GameManager.Instance.PlayClickAudio();
        Application.Quit();
    }
}
