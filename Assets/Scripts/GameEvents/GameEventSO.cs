using System.Collections.Generic;
using UnityEngine;




public class GameEventSO<T> : ScriptableObject {
    // list of listeners that this event will notify if it is invoked
    List<GameEventListener<T>> listeners = new List<GameEventListener<T>>();

    public void RegisterListener(GameEventListener<T> listener) => listeners.Add(listener);
    public void UnRegisteristener(GameEventListener<T> listener) => listeners.Remove(listener);

    public void RaiseEvent(T data) {
        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnEventRaised(data);
        }
    }
}

// for parameterless events, use DefaultUnit as type
[CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvents/GameEvent")]
public class GameEventSO : GameEventSO<DefaultUnit> {
    public void RaiseEvent() => RaiseEvent(DefaultUnit.Default);
}

public struct DefaultUnit {
    public static DefaultUnit Default => default;
}

