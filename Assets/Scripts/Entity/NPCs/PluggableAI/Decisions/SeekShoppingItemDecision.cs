using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/SeekShoppingItemDecision")]
public class SeekShoppingItemDecision : PluggableDecision {
    public override void Decide(StateController controller) {
        if (ShouldSeekItem(controller as NPCStateController)) {
            OnDecisionMade.Invoke(controller);
        }
    }

    private bool ShouldSeekItem(NPCStateController controller) {
        if (controller.nextDestinationTarget != null && controller.npcDatabase.IsNPCAssignedToShelf(controller)){
            Transform target = controller.npcDatabase.TryGetNPCShelfTarget(controller);

            if (target != null && target.GetComponent<BaseShelf>().GetHeldItem() == false) {
                return true;
            }
        }

        if (controller.nextDestinationTarget == null && controller.shoppingList.Any(i => i.collected == false)) {
            
            return true;
        }

        return false;
    }
}
