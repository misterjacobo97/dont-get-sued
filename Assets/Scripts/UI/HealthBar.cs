using R3;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [Header ("refs")]
    [SerializeField] private FloatReference _meterRef;
    private Image _image;

    private void Awake(){
        _image = GetComponentInChildren<Image>();
    }

    private void Start(){
        _meterRef.variable?.reactiveValue.AsObservable()?.Subscribe(num => {
            _image.fillAmount = num > 0 ? num / _meterRef.variable.maxValue : 0;
        }).AddTo(this);
    }
}
