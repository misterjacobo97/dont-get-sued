using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : PersistentSignleton<InputManager> {

    [Header("UI game events")]
    [SerializeField] private GameEventSO _PausedButtonPressedEvent;


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

    // ui
    private InputAction _pauseAction;

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

        // ui
        _pauseAction = _playerActions.UI.Pause;

        
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

        HandleUIEvent();

        HandleInteractEvents();
    }

    private void HandleUIEvent(){
        if (_pauseAction.WasPressedThisFrame()) _PausedButtonPressedEvent.RaiseEvent();
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

}
