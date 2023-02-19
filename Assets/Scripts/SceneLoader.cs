using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    void Awake() {
        if (FindObjectsOfType<SceneLoader>().Length > 1) {
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ReloadScene(float delay) {
        StartCoroutine(LoadSceneDelay(SceneManager.GetActiveScene().buildIndex, delay));
    }

    public void LoadNextScene(float delay) {
        StartCoroutine(LoadSceneDelay(SceneManager.GetActiveScene().buildIndex + 1, delay));
    }

    IEnumerator LoadSceneDelay(int sceneIdx, float delay) {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(sceneIdx);
    }
}