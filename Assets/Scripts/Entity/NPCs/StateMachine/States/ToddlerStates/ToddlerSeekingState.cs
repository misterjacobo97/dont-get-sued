using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToddlerSeekingState : CustomerBaseState {

    public ToddlerSeekingState(CustomerStateMachine.STATES key, ToddlerStateContext_SO context) : base(key, context) { }

    private bool _initialStepsTaken = false;

    public override void EnterState() {
        base.EnterState();

        FindNextTarget();

        if (_context.currentTarget != null) {
            _context.agent.SetDestination(_context.currentTarget.position);
        }

        _initialStepsTaken = true;
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_initialStepsTaken == false) return StateKey;

        //if (_context.shoppingList.All(item => item.collected == true)) {
        //    return CustomerStateMachine.STATES.EXIT;
        //}

        if (_context.currentTarget == null || (_context.currentTarget.GetComponentInParent<BaseShelf>()).HasItem() == false) {

            return CustomerStateMachine.STATES.IDLE;
        }

        return StateKey;
    }

    public override void ExitState() {
        base.ExitState();

        _initialStepsTaken = false;

    }

    private void FindNextTarget() {

        List<TaskInfo> tasks = TaskManager.Instance.GetTaskList.Where(t => t.assignedNPC == null && t.state != (TaskObject.TASK_STATE.COMPLETED | TaskObject.TASK_STATE.FAILED) && t.taskHolder != null).ToList();

        if (tasks.Count > 0) {
            // only bleach
            tasks = tasks.FindAll(t => t.taskHolder != null && _context.itemHolder.IsItemAccepted(t.task.holdableItem_SO));

            TaskInfo task = (tasks[Random.Range(0, tasks.Count - 1)]);
            task.ChangeAssignedNPC(_context.stateMachine.GetComponentInParent<NPCController>());

            BaseShelf shelf = (task.taskHolder as BaseShelf);
            if (shelf != null && shelf.GetCustomerTarget() != null) {
                _context.currentTarget = shelf.GetCustomerTarget();

            }
        }
    }

}
