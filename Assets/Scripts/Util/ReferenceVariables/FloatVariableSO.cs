using R3;
using UnityEngine;

[CreateAssetMenu (fileName ="FloatVariable", menuName ="Primitives/FloatVariable")]
public class FloatVariable : ScriptableObject {
    public enum RESET_TYPE {
        NONE,
        MIN,
        MAX,
        MID
    }
    public RESET_TYPE resetType = RESET_TYPE.NONE;

    public float Value;
    public float maxValue;
    public float minValue;

    public SerializableReactiveProperty<float> reactiveValue;

    private void OnEnable () {
        reactiveValue = new SerializableReactiveProperty<float>();

        switch (resetType) {
            case RESET_TYPE.MIN:
                reactiveValue.Value = minValue;
                break;
            case RESET_TYPE.MAX:
                reactiveValue.Value = maxValue;
                break;
            case RESET_TYPE.MID:
                reactiveValue.Value = maxValue / 2;
                break;
        }

        
    }

    private void OnDisable () {
        reactiveValue?.Dispose();
    }

}
