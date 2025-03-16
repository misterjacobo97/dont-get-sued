using System;
using UnityEngine;

/// <summary>
/// An abstract State class to derive states from and to be used with state manager derived from the BaseStateManager class.
/// </summary>
/// <typeparam name="EState"></typeparam>
public abstract class BaseState<EState> : MonoBehaviour where EState : Enum {
    /// <summary>
    /// Use if instantiating state from class derived from BaseStateManager via script.
    /// </summary>
    /// <param name="key"></param>
    public BaseState(EState key) {
        StateKey = key;
    }

    public EState StateKey;

    /// <summary>
    /// Use if attaching BaseState derived script to a gameObject as a child of the state manager.
    /// </summary>
    /// <param name="context"></param>
    public abstract void Init(ScriptableObject context);
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider collider);
    public abstract void OnTriggerStay(Collider collider);
    public abstract void OnTriggerExit(Collider collider);
}
