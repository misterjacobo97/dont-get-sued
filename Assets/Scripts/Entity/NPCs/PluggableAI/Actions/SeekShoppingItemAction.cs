using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SeekShoppingItemAction")]
public class SeekShoppingItemAction : PluggableAction {

    [SerializeField] float stoppingDistance = 0.1f;


    public override void Act(StateController controller) {
        Seek(controller as NPCStateController);
    }

    private void Seek(NPCStateController controller) {
        if (!controller.shoppingList.Any(i => i.collected == false)) return;

        if (controller.nextDestinationTarget == null && !controller.npcDatabase.IsNPCAssignedToShelf(controller)) {

            controller.agent.stoppingDistance = stoppingDistance;

            controller.npcDatabase.AssignNPCToShelf(controller);

            controller.nextDestinationTarget = controller.npcDatabase.TryGetNPCShelfTarget(controller).GetComponent<BaseShelf>().GetCustomerTarget();
        }

        controller.agent.SetDestination(controller.nextDestinationTarget.position);
        controller.agent.isStopped = false;

        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending) {
            controller.agent.isStopped = true;
        }
    }
}
