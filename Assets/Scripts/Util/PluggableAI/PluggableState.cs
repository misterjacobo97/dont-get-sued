using System;
using UnityEngine;

namespace PluggableAI {

    public class PluggableState : MonoBehaviour {
        public BaseStateSO stateKey;
        public PluggableAction[] actions;
        public PluggableTransition[] transitions;

        public Action<BaseStateSO> ChangeBaseState;
        private StateController parentController;

        [Header("Debug")]
        public Color sceneGizmosColor = Color.green;

        public void UpdateState(StateController controller) {

            DoActions(controller);
            MakeDecisions(controller);
        }

        public void Init(StateController newController) {
            parentController = newController;

            foreach (PluggableTransition transition in transitions) {
                transition.decision.OnDecisionMade += (controller) => {
                    if (controller == parentController) ChangeBaseState.Invoke(transition.nextState);
                };
            }
        }

        private void DoActions(StateController controller) {
            for (int i = 0; i < actions.Length; i++) {
                actions[i]?.Act(parentController);
            }
        }

        private void MakeDecisions(StateController controller) {
            foreach (PluggableTransition transition in transitions) {
                transition.decision.Decide(parentController);
            }
        }

        
    }
}