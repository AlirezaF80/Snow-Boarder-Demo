using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BoostBar : MonoBehaviour {
    private Slider _slider;
    private float _targetValue;

    [Tooltip("How long does it take to reach the new value, in seconds.")] [SerializeField]
    private float valueChangeRate = 0.2f;

    void Start() {
        _slider = GetComponent<Slider>();
    }

    void LateUpdate() {
        _slider.value = Mathf.Lerp(_slider.value, _targetValue, Time.deltaTime / valueChangeRate);
    }

    public void SetBoostAmount(float amount) {
        _targetValue = amount;
    }
}