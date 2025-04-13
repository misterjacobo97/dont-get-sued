using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/FollowerExitDecision")]
public class FollowerExitDecision : PluggableDecision {
    public override void Decide(StateController controller) {
        if (ShouldExit(controller as NPCStateController)) {
            OnDecisionMade.Invoke(controller);
        }
    }

    private bool ShouldExit(NPCStateController controller) {
        if (controller.agentFollowTarget == null) {
            return true;
        }

        return false;
    }
}
