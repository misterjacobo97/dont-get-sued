using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    [Header("refs")]
    [SerializeField] private GameStatsSO _gameState;
    [SerializeField] private GameStateEventChannel _gameEventChannel;
    [SerializeField] private AudioClipListReference _announcementClipsList;
    [SerializeField] private AudioMixerGroup _annoucementMixer;


    [Header("params")]
    [SerializeField] private float _annoucementInterval = 30f;
    [SerializeField] private SoundClipReference _currentAnnouncement = new();
    private List<AudioClip> _usedAnnouncements = new();

    [Header("debug")]
    [SerializeField] private bool _LoadToStartScreen = false;
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;
    [SerializeField] private Cheats_SO _cheats_SO;
    public Cheats_SO GetCheatsSO => _cheats_SO;

    private void Start() {
        _gameState.suedStatus.GetReactiveValue.AsObservable().Subscribe(status => {
            EndGameActions();
        }).AddTo(this);

        _gameState.managementSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
            if (num <= 0) _gameState.suedStatus.SetReactiveValue(true);
        }).AddTo(this);

        _gameState.customerSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
            if (num <= 0) _gameState.suedStatus.SetReactiveValue(true);
        }).AddTo(this);

        _gameState.gameTimeLeft.GetReactiveValue?.AsObservable().Subscribe(time => {
            if (time <= 0){
                EndGameActions();
            }
        }).AddTo(this);

        _gameState.pauseStatus.GetReactiveValue?.AsObservable().Subscribe(status =>{
            switch (status){
                case true:
                    _gameEventChannel.gamePaused.RaiseEvent();
                    OnGamePauseActions();
                    break;
                case false:
                    _gameEventChannel.gameUnpaused.RaiseEvent();
                    OnGamePauseActions();
                    break;
            }
        }).AddTo(this);


        if (_LoadToStartScreen == true){
            LevelManager.Instance.LevelLoadingStarted.AddListener(() => ChangeGameState(GAME_STATE.LOADING));

            PreGameActions();

            LoadStartScreen();
        }
        else {
            ChangeGameState(GAME_STATE.MAIN_GAME);
            GameStartActions("TestLevel");
            LevelManager.instance.LevelLoaded.Invoke(SceneManager.GetActiveScene().name);
        }
    }

    private void Update() {
        if (GetGameState.CurrentValue == GAME_STATE.MAIN_GAME){
            ControlTimer();
        }
    }

    private void ControlTimer() {
        if (GetGameState.CurrentValue != GAME_STATE.MAIN_GAME) return;
            _gameState?.gameTimeLeft?.AddToReactiveValue(-Time.deltaTime);
    }

    # region game special events

    private void PlayAnnoucement(){
        if (GetGameState.CurrentValue != GAME_STATE.MAIN_GAME) return;

        if (_usedAnnouncements.Count >= _announcementClipsList.GetList().Count) return;

        Awaitable.WaitForSecondsAsync(_annoucementInterval).GetAwaiter().OnCompleted(() => {
            

            List<AudioClip> unplayedClips = _announcementClipsList.GetList()
            .Where(clip => !_usedAnnouncements.Contains(clip)).ToList();

            AudioClip selected = unplayedClips[UnityEngine.Random.Range(0, unplayedClips.Count - 1)];

            _currentAnnouncement._soundClip = selected;
            _usedAnnouncements.Add(selected);

            _currentAnnouncement.Play();

            PlayAnnoucement();
        });
    }

    #endregion

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
        if (_currentGameState.Value == GAME_STATE.MAIN_GAME && _gameState.pauseStatus.GetReactiveValue.Value == true) _gameEventChannel.gameUnpaused.RaiseEvent();

        await LevelManager.Instance.LoadLevel("StartScreen");

        ChangeGameState(GAME_STATE.START_SCREEN);
    }

    private void EndGameActions() {
        ChangeGameState(GAME_STATE.END_GAME);

        _gameEventChannel.gameFinished.RaiseEvent();
    }

    public async void GameStartActions(string levelName) {
        if (GetGameState.CurrentValue == (GAME_STATE.LOADING | GAME_STATE.PRE_GAME)) return;

        if (_currentGameState.Value == GAME_STATE.MAIN_GAME && _gameState.pauseStatus.GetReactiveValue.Value == true) _gameEventChannel.gameUnpaused.RaiseEvent();

        _gameState.gameTimeLeft.Reset();
        _gameState.customerSatisfaction.Reset();
        _gameState.managementSatisfaction.Reset();
        _gameState.levelScore.Reset();
        _gameState.suedStatus.SetReactiveValue(false);

        await LevelManager.Instance.LoadLevel(levelName);

        ChangeGameState(GAME_STATE.MAIN_GAME);
        _gameEventChannel.gameStarted.RaiseEvent();
        
        PlayAnnoucement();
    }

    public void OnGamePauseActions(){
        if (_currentGameState.Value != GAME_STATE.MAIN_GAME) return;

        Time.timeScale = _gameState.pauseStatus?.GetReactiveValue.Value == true ? 0 : 1;
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    #endregion
}


