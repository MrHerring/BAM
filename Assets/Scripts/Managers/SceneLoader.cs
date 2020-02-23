using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField]
    private Slider _loadingProgressSlider;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1;
        PrefabPoolingSystem.Reset();
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void BackToHome()
    {
        Time.timeScale = 1;
        GameScore.LvlCompleted = false;
        PrefabPoolingSystem.Reset();
        StartCoroutine(LoadAsynchronously(0));
    }
        
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        _loadingProgressSlider.value = 0;
        _loadingScreen.SetActive(true);

        yield return new WaitForSeconds(2);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            _loadingProgressSlider.value = progress;

            yield return null;
        }

        _loadingScreen.SetActive(false);
        _loadingProgressSlider.value = 0;
    }
}
