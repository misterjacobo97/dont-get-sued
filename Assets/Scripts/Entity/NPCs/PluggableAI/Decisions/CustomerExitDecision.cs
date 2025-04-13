using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/CustomerExitDecision")]
public class CustomerExitDecision : PluggableDecision {
    public override void Decide(StateController controller) {
        if (ShouldExit(controller as NPCStateController)) {
            OnDecisionMade.Invoke(controller);
        }
    }

    public bool ShouldExit(NPCStateController controller) { 
        if (controller.shoppingList.All(item => item.collected)) {
            Debug.Log("Should exit");
            return true;
        }

        return false;
    }
}
