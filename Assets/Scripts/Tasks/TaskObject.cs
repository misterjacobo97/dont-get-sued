using System;
using UnityEngine.Events;
using UnityEngine;

public class TaskObject : MonoBehaviour {
    [NonSerialized] public UnityEvent<TASK_STATE> ChangedTaskState = new();
    [NonSerialized] public UnityEvent<TASK_STATE> ChangedParentHolder = new();

    [SerializeField] private TaskDatabaseSO _taskDatabase;

    public enum TASK_STATE {
        INACTIVE,
        ACTIVE,
        FAILED,
        COMPLETED
    }

    protected TASK_STATE state = TASK_STATE.INACTIVE;
    public TASK_STATE GetTaskState => state;
    
    protected bool _taskActive;

    [SerializeField] protected int _taskScore;

    protected void Start() {
        _taskDatabase.AddToTaskItemList(this);

        ChangedParentHolder.AddListener(newParent => {
            _taskDatabase.AddToTaskItemList(this);
        });
    }

    public void ActivateTask() {
        state = TASK_STATE.ACTIVE;
        ChangedTaskState.Invoke(state);
    }

    public void CompleteTask() {
        DeactivateTask();
        ChangedTaskState.Invoke(TASK_STATE.COMPLETED);

    }

    public void FailTask() {
        DeactivateTask();
        ChangedTaskState.Invoke(TASK_STATE.FAILED);

    }

    protected void DeactivateTask() {
        _taskActive = false;

        // disconnect all event listeners
        ChangedTaskState.RemoveAllListeners();
        ChangedParentHolder.RemoveAllListeners();

    }
}


