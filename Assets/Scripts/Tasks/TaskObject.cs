using System;
using UnityEngine.Events;
using UnityEngine;

public class TaskObject : HoldableItem {
    [NonSerialized] public UnityEvent<TASK_STATE> ChangedTaskState = new();
    [SerializeField] protected int _taskScore;

    public enum TASK_STATE {
        INACTIVE,
        ACTIVE,
        FAILED,
        COMPLETED
    }

    protected TASK_STATE state = TASK_STATE.INACTIVE;
    //protected I_ItemHolder _taskHolder = null;
    protected bool _taskActive;

    protected new void Start() {
        base.Start();
        TaskManager.Instance.AddTaskToList(this, _parentHolder);

        ChangedParentHolder.AddListener(newParent => {
            TaskManager.Instance.AddTaskToList(this, newParent);
        });
    }

    public void ActivateTask() {
        state = TASK_STATE.ACTIVE;
        ChangedTaskState.Invoke(state);
    }

    public void CompleteTask() {
        DeactivateTask();
        ChangedTaskState.Invoke(TASK_STATE.COMPLETED);

        GameManager.Instance.AddToScore(_taskScore);
    }

    public void FailTask() {
        DeactivateTask();
        ChangedTaskState.Invoke(TASK_STATE.FAILED);

    }

    protected void DeactivateTask() {
        _taskActive = false;

        if (_parentHolder != null) {
            _parentHolder.RemoveItem();
        }
        _parentHolder = null;

        _sprite.enabled = false;
        _collider.enabled = false;

        // disconnect all event listeners
        ChangedTaskState.RemoveAllListeners();
        ChangedParentHolder.RemoveAllListeners();

    }
}


