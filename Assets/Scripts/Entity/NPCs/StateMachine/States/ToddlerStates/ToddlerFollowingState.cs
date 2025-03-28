using UnityEngine;

public class ToddlerFollowingState : CustomerBaseState {

    public ToddlerFollowingState(CustomerStateMachine.STATES key, ToddlerStateContext_SO context) : base(key, context) { }

    private bool _initialStepsTaken = false;

    // random chance and time for baby to go for bleach 

    public override void EnterState() {
        base.EnterState();

        _context.currentTarget = (_context as ToddlerStateContext_SO).targetParent.transform;

        // to make toddler stop a few meters from parent
        _context.agent.stoppingDistance = 1;

        if (_context.currentTarget != null) {
            _context.agent.SetDestination(_context.currentTarget.position);
        }

        _initialStepsTaken = true;
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_initialStepsTaken == false) return StateKey;

        if (_context.currentTarget == null || (_context as ToddlerStateContext_SO).targetParent == null) {

            return CustomerStateMachine.STATES.EXIT;
        }

        return StateKey;
    }

    public override void ExitState() {
        base.ExitState();

        _initialStepsTaken = false;
        _context.agent.stoppingDistance = 0.1f;

    }

}
