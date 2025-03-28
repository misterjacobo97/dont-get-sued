using UnityEngine;

public class GrandmaExitState : CustomerBaseState {

    public GrandmaExitState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    private bool _finished = false;
    private bool _stateInitialsed = false;


    public override void EnterState() {
        base.EnterState();
        _context.currentTarget = null;

        _context.currentTarget = NPCManager.Instance.exitArea.transform;
        _context.agent.SetDestination(_context.currentTarget.position);

        _stateInitialsed = true;
    }

    public override void OnTriggerEnter2D(Collider2D collider) {
       if (_context.stateMachine.CurrentState != this) {
            return;
        }
        
        if (collider.transform.TryGetComponent<NPCExitArea>(out NPCExitArea area)) {
            _finished = true;
            (_context.itemHolder as NPCItemHolder).CompleteItems();
            
            Destroy(_context.agent.transform.gameObject);
        }
    }
    public override CustomerStateMachine.STATES GetNextState() {


        return StateKey;
    }
}
