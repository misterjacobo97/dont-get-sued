public class CustomerStateMachine : BaseStateManager<CustomerStateMachine.STATES> {
    public enum STATES {
        IDLE,
        SEEKING,
        FALLING,
        EXIT
    }

    protected override void Start() {
        foreach (CustomerBaseState child in GetComponentsInChildren<CustomerBaseState>()) {
            (_context as CustomerStateContext_SO).stateMachine = this;
            child.Init(_context);
            States.Add(child);
        }

        base.Start();
    }

    public void ForceTransitionState(STATES newState) => TransitionToState(newState);
}

