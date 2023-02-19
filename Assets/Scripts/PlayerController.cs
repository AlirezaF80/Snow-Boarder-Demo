using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] float torqueAmount = 3f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float boostSpeed = 45f;
    [SerializeField] ParticleSystem groundTouchEffect;
    [SerializeField] TrailRenderer boostEffect;

    [Tooltip("How long does it take to increase to target speed, in seconds. (Used in Mathf.Lerp as t)")]
    [SerializeField]
    float speedIncreaseTime = 0.2f;

    [Tooltip("How long does it take to decrease to target speed, in seconds. (Used in Mathf.Lerp as t)")]
    [SerializeField]
    float speedDecreaseTime = 1f;

    [Tooltip("Boost decrease per second.")] [SerializeField]
    float boostDecreaseRate = 0.4f;

    [Tooltip("How much the boost increases when not in air, per second.")] [SerializeField]
    float boostIncreaseRate = 0.05f;

    [Tooltip("How much the boost increases when in air, per seconds.")] [SerializeField]
    float airTimeBoostIncreaseRate = 0.1f; // per second

    [Tooltip("How much the boost increases, per flip.")] [SerializeField]
    float flipBoostIncreaseRate = 0.5f; // per flip

    [Range(0f, 360f)] [Tooltip("How many degrees the player should rotate, to flip. (in degrees)")] [SerializeField]
    float flipRotationAmount = 300f;

    private float _boostAmount;

    private bool _canMove = true;
    private bool _isTouchingGround;

    Rigidbody2D _myRigidBody;
    private readonly List<SurfaceEffector2D> _surfaceEffectors = new();
    private float _currentSpeed;

    private float _prevAbsZRotation;
    private float _curAbsZRotation;
    private float _prevZRotation;

    private BoostBar _boostBar;


    void Start() {
        Time.timeScale = 1f;
        _prevZRotation = transform.rotation.eulerAngles.z;
        _myRigidBody = GetComponent<Rigidbody2D>();
        _boostBar = FindObjectOfType<BoostBar>();

        _currentSpeed = baseSpeed;
        SurfaceEffector2D[] surfaceEffectors = FindObjectsOfType<SurfaceEffector2D>();
        foreach (SurfaceEffector2D surfaceEffector in surfaceEffectors) {
            _surfaceEffectors.Add(surfaceEffector);
            surfaceEffector.speed = baseSpeed;
        }
    }


    void Update() {
        if (_canMove) {
            RotatePlayer();
            SetBoostEffect();
            IncreaseBoost();
            DecreaseBoost();
            SetEffectorsSpeed();
            ShowBoostUI();
        }
    }

    void ShowBoostUI() {
        _boostBar.SetBoostAmount(_boostAmount);
    }

    private void LateUpdate() {
        if (_canMove) {
            CheckFlip();
        }
    }

    void CheckFlip() {
        float rotationDelta = (transform.rotation.eulerAngles.z - _prevZRotation);
        if (rotationDelta > 180) {
            rotationDelta -= 360;
        } else if (rotationDelta < -180) {
            rotationDelta += 360;
        }

        _prevZRotation = transform.rotation.eulerAngles.z;
        _curAbsZRotation += Mathf.Round(rotationDelta);


        if (_isTouchingGround) {
            _prevAbsZRotation = _curAbsZRotation;
            _curAbsZRotation = 360 - Mathf.Abs(_prevZRotation);

            return;
        }

        if (Mathf.Abs(_curAbsZRotation - _prevAbsZRotation) >= flipRotationAmount) {
            _boostAmount += flipBoostIncreaseRate;
            _prevAbsZRotation = _curAbsZRotation;
        }
    }


    void IncreaseBoost() {
        _boostAmount = Mathf.Clamp(_boostAmount, 0, 1);
        if (!_canMove) return;
        if (!_isTouchingGround) {
            _boostAmount += airTimeBoostIncreaseRate * Time.deltaTime;
        } else {
            _boostAmount += boostIncreaseRate * Time.deltaTime;
        }
    }


    void SetEffectorsSpeed() {
        var targetSpeed = baseSpeed;
        if (!_isTouchingGround) {
            targetSpeed = _myRigidBody.velocity.magnitude;
        } else {
            if (Input.GetButton("Jump") && _boostAmount > 0) {
                targetSpeed = boostSpeed;
            }

            var speedChangeTime = _currentSpeed < targetSpeed ? speedIncreaseTime : speedDecreaseTime;
            targetSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime / speedChangeTime);
        }

        foreach (SurfaceEffector2D surfaceEffector in _surfaceEffectors) {
            surfaceEffector.speed = targetSpeed;
        }

        _currentSpeed = targetSpeed;
    }

    void DecreaseBoost() {
        if (Input.GetButton("Jump") && _boostAmount > 0 && _isTouchingGround) {
            _boostAmount -= Time.deltaTime * boostDecreaseRate;
        }
    }

    void SetBoostEffect() {
        boostEffect.emitting = Input.GetButton("Jump") && _boostAmount > 0;
    }


    public void DisableControl() {
        _canMove = false;
    }

    private void RotatePlayer() {
        _myRigidBody.AddTorque(-Input.GetAxisRaw("Horizontal") * torqueAmount);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        _isTouchingGround = true;
        groundTouchEffect.Play();
    }

    private void OnCollisionExit2D(Collision2D other) {
        _isTouchingGround = false;
        groundTouchEffect.Stop();
    }
}