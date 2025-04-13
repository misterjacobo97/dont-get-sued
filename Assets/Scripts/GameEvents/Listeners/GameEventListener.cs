using UnityEngine;
using UnityEngine.Events;

public interface IGameEventListener<T> {
    void OnEventRaised(T data);
}

public class GameEventListener<T> : MonoBehaviour, IGameEventListener<T> {

    // game event to listen to
    [SerializeField] GameEventSO<T> GameEvent;

    // response when game event is raised/fired
    [SerializeField] UnityEvent<T> Response;

    public void OnEnable() => GameEvent.RegisterListener(this);
    public void OnDisable() => GameEvent.UnRegisteristener(this);

    public void OnEventRaised(T data) => Response.Invoke(data);
    
}

public class GameEventListener : GameEventListener<DefaultUnit> { }