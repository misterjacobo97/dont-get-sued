using UnityEngine;


/// <summary>
/// a event bus for general game events such as change of game state and such
/// </summary>
[CreateAssetMenu(menuName = "Systems/GameStateEventChannel")]
public class GameStateEventChannel : ScriptableObject {
    
    public GameEventSO gamePaused;
    public GameEventSO gameUnpaused;

    public GameEventSO gameStarted;
    public GameEventSO gameFinished;
    

}
