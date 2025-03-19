using UnityEngine;

public class CustomerExitState : CustomerBaseState {

    public CustomerExitState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    private bool _finished = false;
    private bool _stateInitialsed = false;


    public override void EnterState() {
        base.EnterState();
        _context.currentTarget = null;

        _context.currentTarget = NPCManager.Instance.exitArea.transform;
        _context.agent.SetDestination(_context.currentTarget.position);

        _stateInitialsed = true;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (_stateInitialsed == false) return;

        if (_context.agent.remainingDistance < 0.1f && _finished == false) {
            _finished = true;
            //(_context.agent.gameObject);
            (_context.itemHolder as NPCItemHolder).CompleteItems();

            Destroy(_context.agent.transform.gameObject);
        }
    }

    public override CustomerStateMachine.STATES GetNextState() {


        return StateKey;
    }
}
