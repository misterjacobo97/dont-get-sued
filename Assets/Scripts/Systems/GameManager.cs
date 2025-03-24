using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSignleton<GameManager>{

    public enum GAME_STATE {
        START_SCREEN,
        MAIN_GAME,
        SUED,
        PAUSED,
        END_GAME
    }

    [NonSerialized] public UnityEvent<GAME_STATE> GameStateChanged = new();
    private GAME_STATE _currentGameState = GAME_STATE.MAIN_GAME;
    public GAME_STATE GetGameState => _currentGameState;

    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Game Params")]
    [SerializeField] private int _secondsInRound = 90;

    private int _totalScore = 0;
    private float _timeLeft  = 0;

    private void Start() {
        _timeLeft = _secondsInRound;
        UpdateUI();
    }

    private void Update() {
        if (_timeLeft > 0) {
            _timeLeft -= Time.deltaTime;
        }

        else if (GetGameState != GAME_STATE.END_GAME && _timeLeft <= 0) {
            EndGameActions();
        }
            UpdateUI();
    }

    /// <summary>
    /// Use to change the game state and invoke an event including the new state.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeGameState(GAME_STATE newState) {
        _currentGameState = newState;
        GameStateChanged.Invoke(_currentGameState);
    }

    /// <summary>
    /// This can be both positive or negative scores
    /// </summary>
    /// <param name="amount"></param>
    public void AddToScore(int amount) {
        _totalScore += amount;

        UpdateUI();
    }

    private void UpdateUI() {
        _scoreText.text = _totalScore.ToString();

        if (_timeLeft > 0) {
            _timerText.text = _timeLeft.ToString("0.00");
        }
        else _timerText.text = "0.00";
    }

    private void EndGameActions() {
        ChangeGameState(GAME_STATE.END_GAME);
        Debug.Log("Game ended");
    }

}
