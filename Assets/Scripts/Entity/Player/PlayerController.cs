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

    [Header("Sprites")]
    [SerializeField] private Sprite _sideSprite;
    [SerializeField] private Sprite _backSprite;

    [Header("Params")]
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


    [Header("Logging")]
    [SerializeField] private Logger _logger;

    // movement related
    private Vector2 _movement;
    private bool _walkingAnimActive = false;

    private void FixedUpdate() {
        MovePlayer();


    }

    #region Movement
    private void MovePlayer() {
        _movement = InputManager.Instance.GetPlayerMovement();

        //if (InputManager.Instance.PlayerDashWasPressed && Time.time >= _timeOfLastDash + _dashDuration) {
        //    _timeOfLastDash = Time.time;
        //    _currentlyDashing = true;
        //}
        //if (_currentlyDashing == true) {
        //    PerformDashAction();
        //}

        if (_movement != Vector2.zero && InputManager.Instance.PlayerInteractIsHeld == false) {
            // check dif between current and requested velocity direction
            double dirDiff = Math.Round(Vector2.Distance(_rb.linearVelocity.normalized, _movement), 2);
            // then create a dynamic float to add to the movementAccel if current vel is not in the requested direction
            float directionSwitchMult = _movementAccel * (float)dirDiff;

            // calculate movement force
            Vector2 newForce = _movement * (_movementAccel + directionSwitchMult) * Time.fixedDeltaTime;
            Log(newForce);

            // change so it doesnt feel sluggish when moving
            _rb.linearDamping = 0.2f;

            _rb.AddForce(newForce);

            ControlSprite(_movement);
            ControlAnimations();

            _lastMovementDir = _movement;
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
            Debug.Log("Slapped");
        }
    }

    private void LimitPlayerVelocity() {
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxMovementSpeed);
    }

    private void ControlSprite(Vector2 dir) {
        if (dir.y > 0) _sprite.sprite = _backSprite;
        else if (dir.y < 0) _sprite.sprite = _sideSprite;

        if (dir.x < 0 && _sprite.flipX == false) {
            _sprite.flipX = true;
        }

        else if (dir.x > 0 && _sprite.flipX == true) {
            _sprite.flipX = false;
        }
    }

    private async void ControlAnimations() {
        if (_walkingAnimActive == true) return;

        _walkingAnimActive = true;

        await _sprite.transform.DOLocalMoveY(0.15f, 0.1f).SetEase(Ease.OutCirc).AsyncWaitForCompletion();
        await _sprite.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InCirc).AsyncWaitForCompletion();

        _walkingAnimActive = false;
    }

    private void PerformDashAction() {
        if (Time.time >= _timeOfLastDash + _dashDuration) {
            _currentlyDashing = false;
            return;
        }

        Debug.Log("dashed");
        _rb.linearDamping = 0.2f;

        _rb.linearVelocity = _lastMovementDir * _dashMaxSpeed * Time.fixedDeltaTime;
    }

    #endregion

    private void Log(object message) {
        if (_logger) {
            _logger.Log(message, this);
        }
    }
}
