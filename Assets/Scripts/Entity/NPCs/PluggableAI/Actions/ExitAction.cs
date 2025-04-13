using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/ExitAction")]
public class ExitAction : PluggableAction {
    public override void Act(StateController controller) {
        Exit(controller as NPCStateController);
    }

    private void Exit(NPCStateController controller) {


        if (!controller.npcDatabase.ExitExists(controller.nextDestinationTarget)) {
            controller.nextDestinationTarget = controller.npcDatabase.GetRandomExit();
        }

        controller.agent.SetDestination(controller.nextDestinationTarget.position);
        controller.agent.isStopped = false;
        if (controller.exitCollider != null) controller.exitCollider.enabled = true;
    }




}
