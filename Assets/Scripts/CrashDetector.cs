using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour {
    [SerializeField] private ParticleSystem crashEffect;

    [SerializeField] private float reloadDelay = 1f;
    [SerializeField] private float slowMotionFactor = 0.5f;

    bool _hasCrashed;

    private void OnCollisionEnter2D(Collision2D col) {
        if (!_hasCrashed) {
            FindObjectOfType<PlayerController>().DisableControl();
            Time.timeScale = slowMotionFactor;
            FindObjectOfType<SceneLoader>().ReloadScene(reloadDelay);
        }

        crashEffect.Play();
        _hasCrashed = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        crashEffect.Stop();
    }
}