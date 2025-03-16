using UnityEngine;

public class CustomerSeekingState : CustomerBaseState {

    public CustomerSeekingState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _changeInterval = 2f;

    public override void EnterState() {
        base.EnterState();

        if (_context.currentTask == null) {
            _context.stateMachine.ForceTransitionState(CustomerStateMachine.STATES.IDLE);
        }
        else {
            _context.target = (_context.currentTask.GetParentHolder() as BaseShelf).GetCustomerTarget();
            _context.agent.SetDestination(_context.target.position);
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_context.currentTask == null) {
            return CustomerStateMachine.STATES.IDLE;
        }

        if (_context.possibleTasks.Contains(_context.currentTask) == false) {
            return CustomerStateMachine.STATES.IDLE;
        }
        //if (_context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete) {
        //    return CustomerStateMachine.STATES.IDLE;
        //}

        return StateKey;
    }
    public override void ExitState() {
        base.ExitState();

        _context.target = null;
        _context.currentTask = null;
    }

}
