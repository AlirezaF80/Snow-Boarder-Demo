using UnityEngine;

public class FinishLine : MonoBehaviour {
    [SerializeField] private float loadDelay = 2f;
    [SerializeField] private float timeScaleFactor = 0.5f;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private ParticleSystem confettiEffect;
    [SerializeField] private Color winParticleColor = Color.white;

    private bool _hasFinished;


    private void OnTriggerEnter2D(Collider2D col) {
        if (_hasFinished) return;
        _hasFinished = true;
        if (col.CompareTag("Player")) {
            ChangeParticleColor();
            confettiEffect.Play();
            Time.timeScale = timeScaleFactor;
            FindObjectOfType<SceneLoader>().LoadNextScene(loadDelay);
        }
    }

    void ChangeParticleColor() {
        var main = smokeEffect.main;
        main.startColor = winParticleColor;
    }
}