using System.Collections;
using UnityEngine;

public class TutorialBox : MonoBehaviour {
    [SerializeField] private float timeScaleFactor = 0.05f;

    [Tooltip("How long is the tutorial in seconds.")] [SerializeField]
    private float delaySeconds = 3f;

    private bool _hasStarted = false;

    void Awake() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player") || _hasStarted) return;
        if (!_hasStarted) {
            _hasStarted = true;
        }

        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }

        Time.timeScale = timeScaleFactor;
        yield return new WaitForSecondsRealtime(delaySeconds);
        Time.timeScale = 1f;
    }
}