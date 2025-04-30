using UnityEngine;
using UnityEngine.Events;

public abstract class IConditionalUnityEvent<T> : MonoBehaviour where T : Object {

    public UnityEvent<T> EventTriggered = new();

    public abstract void Invoke();

}
