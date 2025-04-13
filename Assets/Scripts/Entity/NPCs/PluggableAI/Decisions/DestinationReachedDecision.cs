using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/DestinationReachedDecision")]
public class DestinationReachedDecision : PluggableDecision {
    public override void Decide(StateController controller) {
        if (HasReachedDest(controller as NPCStateController)) {
            OnDecisionMade.Invoke(controller);
        }
    }

    private bool HasReachedDest(NPCStateController controller) {

        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending && controller.nextDestinationTarget != null) {
            return true;
        }

        return false;
    }
}
