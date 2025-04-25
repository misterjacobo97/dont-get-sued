using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/Actions/GrabShoppingItemAction")]
public class GrabShoppingItemAction : PluggableAction {

    public override void Act(StateController controller) {
        
        GrabItem(controller as NPCStateController);
    }

    private  void GrabItem(NPCStateController controller) {
        if (controller.nextDestinationTarget == null || !controller.npcDatabase.TryGetNPCShelfTarget(controller).GetComponent<BaseShelf>().HasItem()) return;

        HoldableItem item = controller.npcDatabase.TryGetNPCShelfTarget(controller).GetComponent<BaseShelf>().GetHeldItem();

        item.ChangeParent(controller.itemHolder);

        ShoppingItem shopItem = controller.shoppingList.First(i => i.item == item.holdableItem_SO && i.collected == false);

        shopItem.collected = true;
        shopItem.sceneItem = item;

        controller.npcDatabase.UnassignNPCToShelf(controller);

        controller.agent.isStopped = true;
        controller.nextDestinationTarget = null;
    }
}
