using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public UnityEvent<bool> SetPlayerStateToUiMode = new UnityEvent<bool>();

    public GameOverCtrl gameOverCtrl;

    public PauseMenuCtrl pauseMenuCtrl;

    private bool _isGameOver;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !_isGameOver)
        {
            pauseMenuCtrl.OnClickResumeBtn();
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance!= null && Instance != this) { 
            Destroy(this);
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
        SetPlayerStateToUiMode?.Invoke(true);
        Utilities.Instance.SetSettingsForUi(true);
        gameOverCtrl.gameObject.SetActive(true);
    }
}
