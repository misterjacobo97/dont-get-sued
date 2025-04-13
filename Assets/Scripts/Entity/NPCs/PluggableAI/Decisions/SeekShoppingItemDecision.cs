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
        if (controller.nextDestinationTarget == null && controller.shoppingList.Any(i => i.collected == false)) {

            return true;
        }

        return false;
    }
}
