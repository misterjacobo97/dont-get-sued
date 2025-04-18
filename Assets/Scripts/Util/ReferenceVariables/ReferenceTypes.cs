using System;
using R3;

[Serializable]
public class FloatReference {

    public bool useConstant = false;
    public float constantValue;
    public FloatVariable variable;

    public void Reset() {
        switch (variable.resetType) {
            case FloatVariable.RESET_TYPE.MIN:
                variable.reactiveValue.Value = variable.minValue;
                return;
            case FloatVariable.RESET_TYPE.MAX:
                variable.reactiveValue.Value = variable.maxValue;
                return;
        }
    }

    public ReactiveProperty<float> GetReactiveValue => variable.reactiveValue;
    public void AddToReactiveValue(float newVal){
        if (newVal > 0 && variable.maxValue != -1 && variable.reactiveValue.Value + newVal > variable.maxValue && variable.reactiveValue.Value != variable.maxValue ) {
            variable.reactiveValue.Value = variable.maxValue;
        }
        else if (newVal < 0 && variable.reactiveValue.Value + newVal < variable.minValue && variable.reactiveValue.Value != variable.minValue) {
            variable.reactiveValue.Value = variable.minValue;
        }
        else variable.reactiveValue.Value += newVal;
    }
    public void SetReactiveValue(float newVal){
        variable.reactiveValue.Value = newVal;
    }
}

[Serializable]
public class IntReference {

    public bool useConstant = false;
    public float constantValue;
    public IntVariable variable;

        public void Reset() {
        switch (variable.resetType) {
            case IntVariable.RESET_TYPE.MIN:
                variable.reactiveValue.Value = variable.minValue;
                return;
            case IntVariable.RESET_TYPE.MAX:
                variable.reactiveValue.Value = variable.maxValue;
                return;
        }
    }

    public ReactiveProperty<int> GetReactiveValue => variable.reactiveValue;
    public void AddToReactiveValue(int newVal){
        if (newVal > 0 && variable.maxValue != -1 && variable.reactiveValue.Value + newVal > variable.maxValue && variable.reactiveValue.Value != variable.maxValue ) {
            variable.reactiveValue.Value = variable.maxValue;
        }
        else if (newVal < 0 && variable.reactiveValue.Value + newVal < variable.minValue && variable.reactiveValue.Value != variable.minValue) {
            variable.reactiveValue.Value = variable.minValue;
        }
        else variable.reactiveValue.Value += newVal;
    }
    public void SetReactiveValue(int newVal){
        variable.reactiveValue.Value = newVal;
    }

    
}

[Serializable]
public class BoolReference {

    public BoolVariable variable;

    public ReactiveProperty<bool> GetReactiveValue => variable.Value;
    public void SetReactiveValue(bool newValue) {
        variable.Value.Value = newValue;
    }
}