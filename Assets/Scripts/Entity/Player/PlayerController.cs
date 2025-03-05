using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Refs")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Params")]
    [SerializeField] private float _movementAccel = 100f;
    [SerializeField] private float _maxMovementSpeed = 20f;
    [SerializeField] private float _linearDamping = 1f;

    [Header("Logging")]
    [SerializeField] private Logger _logger;

    private void FixedUpdate() {
        MovePlayer();
    }

    #region Movement
    private void MovePlayer() {
        Vector2 Movement = InputManager.Instance.GetPlayerMovement();

        if (Movement != Vector2.zero) {
            // check dif between current and requested velocity direction
            double dirDiff = Math.Round(Vector2.Distance(_rb.linearVelocity.normalized, Movement), 2);
            // then create a dynamic float to add to the movementAccel if current vel is not in the requested direction
            float directionSwitchMult = _movementAccel * (float)dirDiff;

            // calculate movement force
            Vector2 newForce = Movement * (_movementAccel + directionSwitchMult) * Time.fixedDeltaTime;
            Log(newForce);

            // change clamping
            _rb.linearDamping = 0f;

            _rb.AddForce(newForce);
        }
        else { 
            // if no movement input
            _rb.linearDamping = _linearDamping;

        }

        LimitPlayerVelocity();
    }

    private void LimitPlayerVelocity() {
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxMovementSpeed);
    }

    #endregion

    private void Log(object message) { 
        if (_logger) {
            _logger.Log(message, this);
        }
    }
}
