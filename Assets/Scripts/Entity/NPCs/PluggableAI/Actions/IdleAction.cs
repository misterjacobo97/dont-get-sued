using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/IdleAction")]
public class IdleAction : PluggableAction {
    public override void Act(StateController controller) {
        Idle(controller as NPCStateController);
    }

    private void Idle(NPCStateController controller) {
        controller.agent.isStopped = true;
    }
}
