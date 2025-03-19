using UnityEngine;

public class CustomerFallingState : CustomerBaseState {

    public CustomerFallingState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _changeInterval = 2f;


    public override void EnterState() {
        base.EnterState();

        _context.currentTarget = GameObject.FindAnyObjectByType<PlayerController>().transform;
    }

    public override CustomerStateMachine.STATES GetNextState() {


        return StateKey;
    }
}
