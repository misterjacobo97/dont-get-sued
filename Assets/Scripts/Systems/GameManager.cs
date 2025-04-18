using System;
using R3;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSignleton<GameManager> {

    public enum GAME_STATE {
        PRE_GAME,
        START_SCREEN,
        MAIN_GAME,
        LOADING,
        SUED,
        PAUSED,
        END_GAME
    }

    [NonSerialized] public UnityEvent<GAME_STATE> GameStateChanged = new();
    private ReactiveProperty<GAME_STATE> _currentGameState = new ReactiveProperty<GAME_STATE>(GAME_STATE.PRE_GAME);
    public ReadOnlyReactiveProperty<GAME_STATE> GetGameState => _currentGameState;

    [Header("UI Refs")]

    [Header("refs")]
    [SerializeField] private GameStatsSO _gameState;


    [Header("Game Params")]
    [SerializeField] private int _secondsInRound = 90;
    [SerializeField] private int _startingHealth = 3;

    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;
    [SerializeField] private Cheats_SO _cheats_SO;
    public Cheats_SO GetCheatsSO => _cheats_SO;

    private void Start() {
        _gameState.suedStatus.GetReactiveValue.AsObservable().Subscribe(status => {
            Debug.Log(status);
        }).AddTo(this);

        _gameState.managementSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
            if (num <= 0) _gameState.suedStatus.SetReactiveValue(true);
        }).AddTo(this);

        _gameState.customerSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
            if (num <= 0) _gameState.suedStatus.SetReactiveValue(true);
        }).AddTo(this);

        _gameState.gameTimeLeft.GetReactiveValue?.AsObservable().Subscribe(time => {
            if (time <= 0){
                ChangeGameState(GAME_STATE.END_GAME);
            }
        }).AddTo(this);



        LevelManager.Instance.LevelLoadingStarted.AddListener(() => ChangeGameState(GAME_STATE.LOADING));

        PreGameActions();

        LoadStartScreen();


    }

    private void Update() {
        ControlTimer();



        if (GetGameState.CurrentValue != GAME_STATE.END_GAME && _gameState.gameTimeLeft.GetReactiveValue.Value <= 0) {
            EndGameActions();
        }

    }

    private void ControlTimer() {
        if (GetGameState.CurrentValue != GAME_STATE.MAIN_GAME) return;
            _gameState?.gameTimeLeft?.AddToReactiveValue(-Time.deltaTime);
    }



    #region Game State Actions
    public void ChangeGameState(GAME_STATE newState) {
        _currentGameState.Value = newState;
        GameStateChanged.Invoke(_currentGameState.Value);

        _logger.Log("Game state changed to: " + GetGameState, this, _showDebugLogs);
    }

    private void PreGameActions() {
        ChangeGameState(GAME_STATE.PRE_GAME);
    }

    public async void LoadStartScreen() {
        await LevelManager.Instance.LoadLevel("StartScreen");

        ChangeGameState(GAME_STATE.START_SCREEN);
    }

    private void EndGameActions() {
        ChangeGameState(GAME_STATE.END_GAME);
    }

    private void SuedActions() {  }

    public async void GameStartActions() {
        await LevelManager.Instance.LoadLevel("TestLevel");

        ChangeGameState(GAME_STATE.MAIN_GAME);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    #endregion
}


