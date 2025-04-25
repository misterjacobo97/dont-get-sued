using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Refs")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _playerInteract;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Collider2D _slapCollider;
    [SerializeField] private LayerMask _hitboxLayer;
    [SerializeField] private Animator _animator;


    // [Header("Sprites")]
    // [SerializeField] private Sprite _sideSprite;
    // [SerializeField] private Sprite _backSprite;

    [Header("Params")]
    [SerializeField] private SoundClipReference _walkingSound;

    // movement
    [SerializeField] private float _movementAccel = 100f;
    [SerializeField] private float _maxMovementSpeed = 20f;
    [SerializeField] private float _linearDamping = 1f;
    private Vector2 _lastMovementDir = Vector2.zero;
    // dashing
    [SerializeField] private float _dashMaxSpeed = 200f;
    [SerializeField] private float _dashDuration = 0.3f;
    private bool _currentlyDashing = false;
    private float _timeOfLastDash = 0f;
    
    [Header("context")]
    [SerializeField] private GameStatsSO _gameStatsDB;
    [SerializeField] private UserInputChannelSO _userInputChannel;


    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs;

    private bool _walkingAnimActive = false;

    private void FixedUpdate() {
        if (_gameStatsDB?.pauseStatus.GetReactiveValue.Value == true) return;

        MovePlayer();
    }

    #region Movement
    private void MovePlayer() {
        if (GameManager.Instance.GetGameState.CurrentValue != GameManager.GAME_STATE.MAIN_GAME) {
            _rb.linearDamping = _linearDamping;
            return;
        }

        Vector2 movement = _userInputChannel.moveInput.GetReactiveValue.Value;

        if (movement != Vector2.zero && _userInputChannel.freezeInput.GetReactiveValue.Value == false) {
            // check dif between current and requested velocity direction
            double dirDiff = Math.Round(Vector2.Distance(_rb.linearVelocity.normalized, movement), 2);
            // then create a dynamic float to add to the movementAccel if current vel is not in the requested direction
            float directionSwitchMult = _movementAccel * (float)dirDiff;

            // calculate movement force
            Vector2 newForce = movement * (_movementAccel + directionSwitchMult) * Time.fixedDeltaTime;
            _logger.Log(newForce, this, _showDebugLogs);

            // change so it doesnt feel sluggish when moving
            _rb.linearDamping = 0.2f;

            _rb.AddForce(newForce);

            ControlSprite(movement);
            ControlAnimations();
            //_slapCollider.

            _lastMovementDir = movement;
        }
        else if (InputManager.Instance.PlayerInteractIsHeld) {
            _rb.linearVelocity = Vector2.zero;
        }
        else {
            // if no movement input
            _rb.linearDamping = _linearDamping;

        }

        LimitPlayerVelocity();
    }



    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == _hitboxLayer) {
            _logger.Log("Slapped", this, _showDebugLogs);
        }
    }

    private void LimitPlayerVelocity() {
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxMovementSpeed);
    }

    private void ControlSprite(Vector2 dir) {
        if (dir.y > 0) _animator.SetBool("facingFront", true);
        else if (dir.y <= 0) _animator.SetBool("facingFront", false);

        if (dir.x < 0 && _sprite.flipX == false) {
            _sprite.flipX = true;
        }

        else if (dir.x > 0 && _sprite.flipX == true) {
            _sprite.flipX = false;
        }
    }

    private async void ControlAnimations() {
        if (_walkingAnimActive == true) return;

        _walkingSound?.Play();

        _walkingAnimActive = true;

        await _sprite.transform.DOLocalMoveY(0.15f, 0.1f).SetEase(Ease.OutCirc).AsyncWaitForCompletion();
        await _sprite.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InCirc).AsyncWaitForCompletion();

        _walkingAnimActive = false;
    }


    #endregion

}
