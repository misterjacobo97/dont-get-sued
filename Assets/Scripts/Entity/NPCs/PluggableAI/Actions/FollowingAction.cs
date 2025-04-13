using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FollowingAction")]
public class FollowingAction : PluggableAction {

    [SerializeField] float stoppingDistance = 0.1f;


    public override void Act(StateController controller) {
        Follow(controller as NPCStateController);
    }

    private void Follow(NPCStateController controller) {
        //if (!controller.shoppingList.Any(i => i.collected == false)) return;

        if (controller.agentFollowTarget == null && controller.npcDatabase.IsFollowerAssigned(controller)) {

            controller.agent.stoppingDistance = stoppingDistance;

            controller.agentFollowTarget = controller.npcDatabase.GetFollowerTarget(controller);
                    
            controller.agent.isStopped = false;
        }

        if (controller.agentFollowTarget != null) {
            controller.agent.SetDestination(controller.agentFollowTarget.position);

        }

        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending) {
            controller.agent.isStopped = true;
        }
        else controller.agent.isStopped = false;
    }
}
