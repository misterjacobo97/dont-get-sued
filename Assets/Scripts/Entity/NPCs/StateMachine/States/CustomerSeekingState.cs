using System.Linq;
using UnityEngine;

public class CustomerSeekingState : CustomerBaseState {

    public CustomerSeekingState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _changeInterval = 2f;
    private bool _initialStepsTaken = false;

    public override void EnterState() {
        base.EnterState();

        _context.agent.SetDestination(_context.currentTarget.position);

        _initialStepsTaken = true;
    }

    public override void UpdateState() {
        base.UpdateState();


        if (_context.agent.remainingDistance < 0.1 && _context.currentTarget != null) {

            BaseShelf shelf = (_context.currentTarget.GetComponentInParent<BaseShelf>());
            Debug.Log(shelf.GetHeldItem());

            _context.shoppingList.Find(item => !item.collected && item.item == shelf.GetHeldItem().holdableItem_SO).collected = true;


            shelf.GetHeldItem().ChangeParent(_context.itemHolder);
            
            _context.currentTarget = null;
        
        }
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_initialStepsTaken == false) return StateKey;

        if (_context.shoppingList.All(item => item.collected == true)) {
            return CustomerStateMachine.STATES.EXIT;
        }

        if (_context.currentTarget == null || (_context.currentTarget.GetComponentInParent<BaseShelf>()).HasItem() == false) {

            return CustomerStateMachine.STATES.IDLE;
        }


        return StateKey;
    }
    public override void ExitState() {
        base.ExitState();

        _initialStepsTaken = false;

    }

}
