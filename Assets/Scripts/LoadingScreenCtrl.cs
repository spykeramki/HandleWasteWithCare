using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenCtrl : MonoBehaviour
{
    public Slider loadSlider;

    public Image screenBg;

    [SerializeField]
    private Sprite[] screenSprites;

    private int _currentIndex = 1;

    public void ShowLoadingScreen(string sceneName)
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadSceneAndChangeImages());
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    //Changing images in the loading screen
    private IEnumerator LoadSceneAndChangeImages()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            screenBg.sprite = screenSprites[_currentIndex];
            _currentIndex++;

            if(_currentIndex>= screenSprites.Length)
            {
                _currentIndex = 0;
            }
        }
    }

    //Controlling load effect of slider as per scene load progress
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(2f);
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!sceneLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
            loadSlider.value = Mathf.Clamp(sceneLoad.progress, 0f, 1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
