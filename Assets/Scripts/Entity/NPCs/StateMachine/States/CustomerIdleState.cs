using UnityEngine;

public class GrandmaIdleState : CustomerBaseState {
    public GrandmaIdleState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _minIdleDuration = 2f;
    [SerializeField] private float _maxIdleDuration = 8f;

    [SerializeField] private CustomerStateMachine.STATES _nextState;

    private float _waitingDuration;
    private bool _stateInitialised = false;

    public override void EnterState() {
        base.EnterState();

        _waitingDuration = Random.Range(_minIdleDuration, _maxIdleDuration);
        _context.currentTarget = null;

        _stateInitialised = true;
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (Time.time > _context.timeOfLastStateChange + _waitingDuration) {
            return _nextState;
        }

        return StateKey;
    }

    public override void ExitState() {
        _stateInitialised = false;
        
        base.ExitState();
    }

}
