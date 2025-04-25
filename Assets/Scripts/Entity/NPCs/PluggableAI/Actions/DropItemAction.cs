using System;
using System.Collections.Generic;
using System.Linq;
using PluggableAI;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/DropItemAction")]
public class DropItemAction : PluggableAction {

    [Serializable]
    private struct DropDecision {
        public bool willDropItem;
        public Transform npc;

        public DropDecision (bool decision, Transform _npc) {
            this.willDropItem = decision;
            npc = _npc;
        }
    }

    [SerializeField] float stoppingDistance = 0.1f;
    [SerializeField] private List<DropDecision> _activeNpcs;
    [SerializeField] private float _randomNum = 0;

    private void OnEnable() {
        _activeNpcs = new();
    }

    public override void Act(StateController controller) {

        _randomNum = UnityEngine.Random.Range(0f,1f);

        Seek(controller as NPCStateController);

        

        if (_activeNpcs.Any(i => i.npc == null)) {
            DropDecision[] decisions = _activeNpcs.FindAll(i => i.npc == null).ToArray();

            for (int i = decisions.Length - 1; i >= 0; i--) {
                _activeNpcs.Remove(decisions[i]);
            }
        }
    }

    private void Seek(NPCStateController controller) {

        if (_activeNpcs.Any(a => a.npc == controller.transform)) return;
        

        DropDecision decision =new DropDecision(
            _randomNum > 0.5 ? true : false,
            controller.transform
        );

        _activeNpcs.Add(decision);

        if (decision.willDropItem == false) return;

        Awaitable.WaitForSecondsAsync(20).GetAwaiter().OnCompleted(() => {
            ShoppingItem item = controller.shoppingList.Find(i => i.collected == true);
            
            if (item != null){
                item.sceneItem.DropItem();

                item.sceneItem = null;
                item.collected = false;
            }

            if (_activeNpcs.Contains(decision)) _activeNpcs.Remove(decision);
        });
    }
}
