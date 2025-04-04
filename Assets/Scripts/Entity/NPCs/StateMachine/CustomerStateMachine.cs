using UnityEngine;

public class CustomerStateMachine : BaseStateManager<CustomerStateMachine.STATES> {
    public enum STATES {
        IDLE,
        SEEKING,
        SLAPPED,
        FALLING,
        EXIT, 
        FOLLOWING
    }

    protected override void Start() {
        foreach (CustomerBaseState child in GetComponentsInChildren<CustomerBaseState>()) {
            (_context as CustomerStateContext_SO).stateMachine = this;
            child.Init(_context);
            States.Add(child);
        }

        base.Start();
    }

    //private void OnCollisionEnter2D(UnityEngine.Collision2D collision) {
    //    Debug.Log(collision);
    //}

    public void ForceTransitionState(STATES newState) => TransitionToState(newState);
}

