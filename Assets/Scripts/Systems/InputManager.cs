using System;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : PersistentSignleton<InputManager> {

    [Header("UI game events")]
    [SerializeField] private GameEventSO _PausedButtonPressedEvent;

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



    protected override void Awake(){
        base.Awake();

        // _playerFreezeInput = new SerializableReactiveProperty<bool>(false).AddTo(this);

    } 

    private void Start() {
        // separating into its smaller chunks first
        _playerActions = new();
        _playerActions.Enable();

        _interactAction = _playerActions.Player.Interact;
        _moveAction = _playerActions.Player.Move;
        _dashAction = _playerActions.Player.Dash;
        _slapAction = _playerActions.Player.Slap;


        HandleUIEvent();

        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.Player.Freeze.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
           _userInputChannel.freezeInput.GetReactiveValue.Value = x;
        }).AddTo(this);

        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.Player.Interact.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
           _userInputChannel.InteractInput.GetReactiveValue.Value = x;
        }).AddTo(this);

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
        Observable.EveryValueChanged(this, x => {
            return (int)x._playerActions.UI.Pause.ReadValue<float>() > 0 ? true : false;
        }).Subscribe(x => {
            if (x == false) return;
            _PausedButtonPressedEvent.RaiseEvent();
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
}
