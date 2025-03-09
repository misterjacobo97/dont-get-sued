using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : PersistentSignleton<InputManager> {

    [NonSerialized] public UnityEvent PlayerInteractPressedEvent = new();

    public InputSystem_Actions _playerActions;

    [NonSerialized] public bool PlayerIntereactWasPressed = false;
    [NonSerialized] public bool PlayerIntereactWasReleased = false;
    [NonSerialized] public bool PlayerIntereactIsHeld = false;

    // internal
    private InputAction _interactAction;
    private InputAction _moveAction;


    private void Start() {
        // separating into its smaller chunks first
        _playerActions = new();
        _playerActions.Enable();

        _interactAction = _playerActions.Player.Interact;
        _moveAction = _playerActions.Player.Move;
    }

    private void Update() {
        PlayerIntereactWasPressed = _interactAction.WasPressedThisFrame();
        PlayerIntereactWasReleased = _interactAction.WasReleasedThisFrame();
        PlayerIntereactIsHeld = _interactAction.IsPressed();

        if (PlayerIntereactWasPressed == true) {
            PlayerInteractPressedEvent.Invoke();
        }
    }

    /// <summary>
    /// Returns the current input axis corresponding to the player controls - i.e. returns Vector2.right if user is pressing right/D
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPlayerMovement() {
        Vector2 input;

        // get initial value
        input = _moveAction.ReadValue<Vector2>();

        return input;
    }

}
