using UnityEngine;

public class CustomerSeekingState : CustomerBaseState {

    public CustomerSeekingState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _changeInterval = 2f;
    private bool _initialStepsTaken = false;

    public override void EnterState() {
        base.EnterState();

        if (_context.currentTask == null) {
            _context.stateMachine.ForceTransitionState(CustomerStateMachine.STATES.IDLE);
        }
        else {
            _context.target = (_context.currentTask.GetParentHolder() as BaseShelf).GetCustomerTarget();
            _context.agent.SetDestination(_context.target.position);
        }

        _initialStepsTaken = true;


    }

    public override void UpdateState() {
        base.UpdateState();


    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_initialStepsTaken == false) return StateKey;

        if (
            _context.currentTask == null ||
            _context.possibleTasks.Contains(_context.currentTask) == false
            ) {
            return CustomerStateMachine.STATES.IDLE;
        }

        if (_context.agent.remainingDistance < 0.1) {
            return CustomerStateMachine.STATES.IDLE;
        }


        return StateKey;
    }
    public override void ExitState() {
        base.ExitState();

        _context.target = null;
        _context.currentTask = null;

        _initialStepsTaken = false;

    }

}
