using System;
using System.Threading.Tasks;
using TMPro;
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
    private GAME_STATE _currentGameState = GAME_STATE.PRE_GAME;
    public GAME_STATE GetGameState => _currentGameState;

    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Game Params")]
    [SerializeField] private int _secondsInRound = 90;
    [SerializeField] private int _startingHealth = 3;


    private int _totalScore = 0;
    private float _timeLeft = 0;
    private int _currentHealth;


    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;


    private void Start() {
        _timeLeft = _secondsInRound;
        _currentHealth = _startingHealth;

        UpdateUI();

        LevelManager.Instance.LevelLoadingStarted.AddListener(() => ChangeGameState(GAME_STATE.LOADING));

        PreGameActions();

        LoadStartScreen();


    }

    private void Update() {
        ControlTimer();


        if (GetGameState != GAME_STATE.END_GAME && _timeLeft <= 0) {
            EndGameActions();
        }

        UpdateUI();
    }

    private void ControlTimer() {
        if (GetGameState != GAME_STATE.MAIN_GAME) return;
        if (_timeLeft > 0) {
            _timeLeft -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Use to change the game state and invoke an event including the new state.
    /// </summary>
    /// <param name="newState"></param>
    private void UpdateUI() {
        _scoreText.text = _totalScore.ToString();

        UIManager.Instance.ChangeHealthUI(_currentHealth);


        if (_timeLeft > 0) {
            _timerText.text = _timeLeft.ToString("0.00");
        }
        else _timerText.text = "0.00";
    }

    /// <summary>
    /// This can be both positive or negative scores
    /// </summary>
    /// <param name="amount"></param>
    public void AddToScore(int amount) {
        _totalScore += amount;

        UpdateUI();
    }

    /// <summary>
    /// to change the health of the player, pass in a negative number to damage it
    /// </summary>
    /// <param name="value"></param>
    public void AddToHealth(int value) {
        _currentHealth += value;

        UpdateUI();
    }

    #region Game State Actions
    public void ChangeGameState(GAME_STATE newState) {
        _currentGameState = newState;
        GameStateChanged.Invoke(_currentGameState);

        _logger.Log("Game state changed to: " + GetGameState, this, _showDebugLogs);
    }

    private void PreGameActions() {
        ChangeGameState(GAME_STATE.PRE_GAME);

    }

    public async Task LoadStartScreen() {
        await LevelManager.Instance.LoadLevel("StartScreen");

        ChangeGameState(GAME_STATE.START_SCREEN);

    }

    private void EndGameActions() {
        ChangeGameState(GAME_STATE.END_GAME);
    }

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
