using UnityEngine;

//public class ToddlerStateMachine : BaseStateManager<ToddlerStateMachine.STATES> {
//    public enum STATES {
//        IDLE,
//        SEEKING,
//        SLAPPED,
//        FOLLOWING
//    }

//    protected override void Start() {
//        foreach (CustomerBaseState child in GetComponentsInChildren<CustomerBaseState>()) {
//            (_context as CustomerStateContext_SO).stateMachine = this;
//            child.Init(_context);
//            States.Add(child);
//        }

//        base.Start();
//    }

//    public void ForceTransitionState(STATES newState) => TransitionToState(newState);
//}
