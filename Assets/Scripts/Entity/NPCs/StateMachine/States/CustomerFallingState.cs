using UnityEngine;

public class GrandmaFallingState : CustomerBaseState {

    public GrandmaFallingState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private Sprite _fallingSprite;
    [SerializeField] private Sprite _normalSprite;


    [SerializeField] private float _minStunDuration = 2f;
    [SerializeField] private float _maxStunDuration = 8f;

    private float _stunDuration;

    public override void EnterState() {
        base.EnterState();

        _stunDuration = Random.Range(_minStunDuration, _maxStunDuration);
        _context.currentTarget = null;

        _context.spriteRenderer.sprite = _fallingSprite;

        _context.rb.AddForce(_context.lastMovementDir * 700);
    }



    public override CustomerStateMachine.STATES GetNextState() {
        if (_context.spriteRenderer.sprite != _fallingSprite) {
            _context.spriteRenderer.sprite = _fallingSprite;

        }

        if (Time.time > _context.timeOfLastStateChange + _stunDuration) {
            return CustomerStateMachine.STATES.IDLE;
        }

        return StateKey;
    }

    public override void ExitState() {
        _context.spriteRenderer.sprite = _normalSprite;
        base.ExitState();
    }

}
