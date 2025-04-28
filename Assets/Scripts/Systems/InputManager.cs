using System;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : PersistentSignleton<InputManager> {

    [Header("UI")]
    [SerializeField] private GameEventSO _PausedButtonPressedEvent;
    [SerializeField] private Vector2Reference _currentMousePos;
    [SerializeField] private Vector2Reference _lastMouseDir;
    [SerializeField] private Vector2Reference _currentMouseVelocity;


    [Header("Player")]
    [SerializeField] private UserInputChannelSO _userInputChannel;

    [NonSerialized] public UnityEvent PlayerInteractPressedEvent = new();
    [NonSerialized] public UnityEvent PlayerInteractHeldReleasedEvent = new();
    [NonSerialized] public UnityEvent PlayerInteractHeldEvent = new();

    public InputSystem_Actions _playerActions;
    
    // Interact
    [NonSerialized] public bool PlayerInteractWasPressed = false;
    [NonSerialized] public bool PlayerInteractWasReleased = false;
    [NonSerialized] public bool PlayerInteractIsHeld = false;
    private float _minInteractHeldDuration = 0.2f;
    private float _interactHeldDuration = 0;
    private bool _isHoldingInteract = false;

    // Dash
    [NonSerialized] public bool PlayerDashWasPressed = false;
    [NonSerialized] public bool PlayerDashtWasReleased = false;
    [NonSerialized] public bool PlayerDashIsHeld = false;

    // Slap
    [NonSerialized] public bool PlayerSlapWasPressed = false;
    [NonSerialized] public bool PlayerSlaptWasReleased = false;
    [NonSerialized] public bool PlayerSlapIsHeld = false;

    // movement
    [NonSerialized] public UnityEvent<Vector2> MovementInputEvent = new();
    private Vector2 _lastMovementInput = Vector2.zero;

    // internal
    private InputAction _interactAction;
    private InputAction _moveAction;
    private InputAction _dashAction;
    private InputAction _slapAction;

    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;

    private void Start() {
        // separating into its smaller chunks first
        _playerActions = new();
        _playerActions.Enable();

        _interactAction = _playerActions.Player.Interact;
        _moveAction = _playerActions.Player.Move;
        _dashAction = _playerActions.Player.Dash;
        _slapAction = _playerActions.Player.Slap;

        HandleUIEvent();
        HandleMouseInput();

        // player freeze input
        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.Player.Freeze.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
           _userInputChannel.freezeInput.GetReactiveValue.Value = x;
        }).AddTo(this);

        // player interact input
        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.Player.Interact.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
           _userInputChannel.InteractInput.GetReactiveValue.Value = x;
        }).AddTo(this);

        // player move input
        Observable.EveryValueChanged(this, x => {
            return x._playerActions.Player.Move.ReadValue<Vector2>();
        }).Subscribe(val => {
            // change player dir
            if (val != Vector2.zero && val.normalized != _userInputChannel.lastMoveDir.GetReactiveValue.Value) {
                _userInputChannel.lastMoveDir.GetReactiveValue.Value = val.normalized;
            }

            _userInputChannel.moveInput.SetReactiveValue(val.normalized);
        }).AddTo(this);


    }

    private void Update() {
        PlayerInteractWasPressed = _interactAction.WasPressedThisFrame();
        PlayerInteractWasReleased = _interactAction.WasReleasedThisFrame();
        PlayerInteractIsHeld = _interactAction.IsPressed();

        PlayerDashWasPressed = _dashAction.WasPressedThisFrame();
        PlayerDashtWasReleased = _dashAction.WasReleasedThisFrame();
        PlayerDashIsHeld = _dashAction.IsPressed();

        PlayerSlapIsHeld = _slapAction.IsPressed();
        PlayerSlaptWasReleased = _slapAction.WasReleasedThisFrame();
        PlayerSlapWasPressed = _slapAction.WasPressedThisFrame();

        HandleInteractEvents();
    }

    private void HandleUIEvent(){
        // pause input
        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.UI.Pause.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
            if (x == false) return;
            _PausedButtonPressedEvent.RaiseEvent();
        }).AddTo(this);

    }

    private void HandleMouseInput(){
        Observable.EveryUpdate().Subscribe(x => {
            Vector2 newPos = (Vector2)_playerActions.UI.Point.ReadValue<Vector2>();

            // calculate the change velocity prior to changing the pos
            float distanceTravelled = Vector2.Distance(_currentMousePos.GetReactiveValue.Value, newPos);
            float speed = distanceTravelled / Time.deltaTime;
            _currentMouseVelocity.SetReactiveValue(speed * (newPos - _currentMousePos.GetReactiveValue.Value).normalized);

            _currentMousePos.SetReactiveValue(newPos);

            if (_currentMouseVelocity.GetReactiveValue.Value != Vector2.zero) {
                // calculate the new dir before setting the new current pos
                Vector2 calculatedNewDir = _currentMouseVelocity.GetReactiveValue.Value.normalized;

                _lastMouseDir.SetReactiveValue(calculatedNewDir);
            }


        }).AddTo(this);
    }

    /// <summary>
    /// Returns the current input axis corresponding to the player controls - i.e. returns Vector2.right if user is pressing right/D
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPlayerMovement() {
        Vector2 input;

        // get initial value
        input = _moveAction.ReadValue<Vector2>();

        // update last player movement
        if (_lastMovementInput != input && input != Vector2.zero) { 
            _lastMovementInput = input;
            MovementInputEvent.Invoke(_lastMovementInput);
        }

        return input;
    }

    
    public Vector2 GetLastPlayerMovment() {
        return _lastMovementInput;
    }

    public void HandleInteractEvents() {
        if (PlayerInteractIsHeld == true) {
            _interactHeldDuration += Time.deltaTime;
        }

        // normal press
        if (PlayerInteractWasReleased == true && (_interactHeldDuration < _minInteractHeldDuration) && _interactHeldDuration > 0) {
            _interactHeldDuration = 0f;
            PlayerInteractPressedEvent.Invoke();
        }

        // held started
        else if (PlayerInteractIsHeld == true && _interactHeldDuration > _minInteractHeldDuration && _isHoldingInteract == false) {
            _isHoldingInteract = true;
            PlayerInteractHeldEvent.Invoke();
        }

        // held released
        else if (PlayerInteractWasReleased == true && _isHoldingInteract == true) {
            _isHoldingInteract = false;
            _interactHeldDuration = 0f;
            PlayerInteractHeldReleasedEvent.Invoke();
        }
    }

    public Vector2 GetMousePosition(){
        return _currentMousePos.GetReactiveValue.Value;
    }

    public Vector2 GetLastMouseDirection(){
        return _lastMouseDir.GetReactiveValue.Value;
    }

}
