using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/SlappedAction")]
public class GetSlappedAction : PluggableAction {
    public override void Act(StateController controller) {
        Slapped(controller as NPCStateController);
    }

    private void Slapped(NPCStateController controller) {
        
    }
}
