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
        SceneManager.LoadScene("01Main");
    }

    private void OnClickMenuBtn()
    {
        DestroyImmediate(DataManager.Instance.gameObject);
        SceneManager.LoadScene("00Start");
    }

    private void OnClickQuitBtn()
    {
        Application.Quit();
    }
}
