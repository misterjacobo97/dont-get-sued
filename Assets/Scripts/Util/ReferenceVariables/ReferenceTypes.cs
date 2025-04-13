using System;
using Unity.VisualScripting;

[Serializable]
public class FloatReference {

    public bool useConstant = true;
    public float constantValue;
    public FloatVariable variable;

    public float Value {
        get {
            return useConstant
                ? constantValue
                : variable.Value;
        }
        set {
            variable.Value = value;
        }
    }
}

[Serializable]
public class IntReference {

    public bool useConstant = true;
    public float constantValue;
    public IntVariable variable;

    public float Value {
        get {
            return useConstant
                ? constantValue
                : variable.Value;
        }
        set {
            variable.Value = value;
        }
    }
}

[Serializable]
public class BoolReference {

    
    public bool useDefault = false;
    public bool defaultValue;
    public BoolVariable variable;

    public bool Value {
        get {
            return useDefault
                ? defaultValue
                : variable.Value;
        }
        set {
            variable.Value = value;
        }
    }
}