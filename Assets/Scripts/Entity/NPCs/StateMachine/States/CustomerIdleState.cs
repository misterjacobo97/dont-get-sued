using UnityEngine;

public class CustomerIdleState : CustomerBaseState {

    public CustomerIdleState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) {}

    [SerializeField] private float _changeInterval = 2f;

    public override void EnterState() {
        base.EnterState();

        _context.target = null;
    }

    public override void UpdateState() {
        base.UpdateState();

        //if (Time.time >= _context.timeOfLastStateChange + _changeInterval) {
            if (_context.possibleTasks.Count > 0) {
                _context.currentTask = _context.possibleTasks[0];
            }
            //else if (_context.possibleTasks.Count == 0 && _context.currentTask != null) {
            //    _context.currentTask = null;
            //    _changeInterval += 2;
            //}
        //}
    }

    public override CustomerStateMachine.STATES GetNextState() {

        if (_context.currentTask != null) {
            return CustomerStateMachine.STATES.SEEKING;
        }

        return StateKey;
    }
}
