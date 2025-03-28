using UnityEngine;

public class CustomerBaseState : BaseState<CustomerStateMachine.STATES> {

    protected CustomerStateContext_SO _context;

    public CustomerBaseState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key) {
        _context = context;
    }

    public override void Init(ScriptableObject context) {
        _context = (CustomerStateContext_SO)context;
    }
    public override void EnterState() {
        _context.timeOfLastStateChange = Time.time;
    }
    public override void ExitState() {
        _context.lastState = StateKey;
    }
    public override void UpdateState() { }
    public override void FixedUpdateState() { }
    public override CustomerStateMachine.STATES GetNextState() {
        return StateKey;
    }
    public override void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerEnter2D(Collider2D collider) { }

    public override void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerStay2D(Collider2D collider) { }
    public override void OnTriggerExit(Collider collider) { }
    public virtual void OnTriggerExit2D(Collider2D collider) { }


}
