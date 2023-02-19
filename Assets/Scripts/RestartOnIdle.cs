using UnityEngine;

public class RestartOnIdle : MonoBehaviour {
    [SerializeField] private float idleTime = 4f;
    [SerializeField] private float velocityThreshold = 3f;
    [SerializeField] private GameObject promptSprite;
    private Rigidbody2D _playerRb;
    float _idleTimer;

    void Start() {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Debug.Log(_playerRb.velocity.magnitude);
        if (_playerRb.velocity.magnitude < velocityThreshold) {
            _idleTimer += Time.deltaTime;
            if (_idleTimer > idleTime) {
                promptSprite.SetActive(true);
            }
        } else {
            _idleTimer = 0;
            promptSprite.SetActive(false);
        }

        if (Input.GetButton("Restart")) {
            FindObjectOfType<SceneLoader>().ReloadScene(0);
        }
    }
}