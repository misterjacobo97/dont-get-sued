using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class InputManager : PersistentSignleton<InputManager> {

    [SerializeField] InputActionAsset _actionAsset;
    private InputActionMap _playerActions;
    private InputActionMap _UIActions;

    private void Start() {
        // separating into its smaller chunks first
        _playerActions = _actionAsset.FindActionMap("Player");
        _UIActions = _actionAsset.FindActionMap("UI");
    }

    /// <summary>
    /// Returns the current input axis corresponding to the player controls - i.e. returns Vector2.right if user is pressing right/D
    /// </summary>
    /// <returns></returns>
    public Vector2 GetPlayerMovement() {
        Vector2 input;

        // get initial value
        input = _playerActions["Move"].ReadValue<Vector2>();

        return input;
    }





}
