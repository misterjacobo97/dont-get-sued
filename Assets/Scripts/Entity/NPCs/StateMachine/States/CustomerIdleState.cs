using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerIdleState : CustomerBaseState {
    public CustomerIdleState(CustomerStateMachine.STATES key, CustomerStateContext_SO context) : base(key, context) { }

    [SerializeField] private float _minIdleDuration = 2f;
    [SerializeField] private float _maxIdleDuration = 8f;
    
    private float _waitingDuration;
    private bool _stateInitialised = false;

    public override void EnterState() {
        base.EnterState();

        _waitingDuration = Random.Range(_minIdleDuration, _maxIdleDuration);
        _context.currentTarget = null;

        _stateInitialised = true;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (Time.time > _context.timeOfLastStateChange + _waitingDuration && _context.currentTarget == null) {

            FindNextTarget();
            _waitingDuration += _minIdleDuration;
        }
    }

    public override CustomerStateMachine.STATES GetNextState() {
        if (_context.currentTarget != null) {
            return CustomerStateMachine.STATES.SEEKING;
        }

        return StateKey;
    }

    public override void ExitState() {
        _stateInitialised = false;
        
        base.ExitState();
    }

    private void FindNextTarget() {

        List<TaskInfo> tasks = TaskManager.Instance.GetTaskList.Where(t => t.assignedNPC == null && t.state != (TaskObject.TASK_STATE.COMPLETED | TaskObject.TASK_STATE.FAILED) && t.taskHolder != null).ToList();

        if (tasks.Count > 0) {
            // only shopping list items
            tasks = tasks.FindAll(t => _context.shoppingList.Exists(item => item.item == t.task.holdableItem_SO && item.collected == false) && t.taskHolder != null);

            TaskInfo task = (tasks[Random.Range(0, tasks.Count - 1)]);
            task.ChangeAssignedNPC(_context.stateMachine.GetComponentInParent<NPCController>());

            BaseShelf shelf = (task.taskHolder as BaseShelf);
            if (shelf != null && shelf.GetCustomerTarget() != null) {
                _context.currentTarget = shelf.GetCustomerTarget();

            }
        }
    }

}
