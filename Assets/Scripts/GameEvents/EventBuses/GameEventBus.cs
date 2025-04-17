using UnityEngine;


/// <summary>
/// a event bus for general game events such as change of game state and such
/// </summary>
[CreateAssetMenu(menuName = "GameEvents/GameEventBus")]
public class GameEventBus : ScriptableObject {
    public GameEventSO changedGameState;
}
