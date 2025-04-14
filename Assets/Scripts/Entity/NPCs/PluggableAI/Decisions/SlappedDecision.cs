using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/SlappedDecision")]
public class SlappedDecision : PluggableDecision {

    public override void Decide(StateController controller) {

        IsSlapped(controller as NPCStateController);
    }

    private void IsSlapped(NPCStateController controller) {
        //if (controller.isSlapped.Value == true) {
        //    OnDecisionMade.Invoke(controller);
        //}

    }


}
