using UnityEngine;

public class CustomerSlappedState : CustomerBaseState {

    public CustomerSlappedState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private Sprite _slappedSprite;
    [SerializeField] private Sprite _normalSprite;


    [SerializeField] private float _minStunDuration = 2f;
    [SerializeField] private float _maxStunDuration = 8f;


    private float _stunDuration;
    //private bool _stateInitialised = false;

    public override void EnterState() {
        base.EnterState();

        _stunDuration = Random.Range(_minStunDuration, _maxStunDuration);
        _context.currentTarget = null;

        _context.spriteRenderer.sprite = _slappedSprite;
        //_stateInitialised = true;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (_context.spriteRenderer.sprite != _slappedSprite) { 
            _context.spriteRenderer.sprite = _slappedSprite;

        }
        //if (Time.time > _context.timeOfLastStateChange + _stunDuration) {

        //    _stunDuration += _minStunDuration;
        //}
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (Time.time > _context.timeOfLastStateChange + _stunDuration) {
            return CustomerStateMachine.STATES.IDLE;
        }

        return StateKey;
    }

    public override void ExitState() {
        //_stateInitialised = false;
        _context.spriteRenderer.sprite = _normalSprite;
        base.ExitState();
    }

}
