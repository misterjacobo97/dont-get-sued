using R3;
using TMPro;
using UnityEngine;

public class UICounter : MonoBehaviour {

    private TextMeshProUGUI _text;

    [Header("context")]
    [SerializeField] private FloatReference _counterToChange;

    private void Awake(){
        if (TryGetComponent(out TextMeshProUGUI text)){
            _text = text;
        }
        else _text = GetComponentInChildren<TextMeshProUGUI>();

        _counterToChange.GetReactiveValue?.AsObservable().Subscribe(val => {
            _text.text = val.ToString("0.0");
        }).AddTo(this);
    }
}
