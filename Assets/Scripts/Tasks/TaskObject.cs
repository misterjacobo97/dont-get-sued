using Tasks;
using UnityEngine;

public class TaskObject : HoldableItem {
    protected I_ItemHolder _taskHolder;
    protected bool _taskActive;

    public void CompleteTask() {
        //Debug.Log("Completing task: " + this);

        DeactivateTask();

        TaskManager.Instance.AddCompletedTask(this);
    }

    public void FailTask() {
        //Debug.Log("failing task: " + this);

        DeactivateTask();

        TaskManager.Instance.AddToFailedTasks(this);
    }

    protected void DeactivateTask() {
        _taskActive = false;

        if (_taskHolder != null) {
            _taskHolder.RemoveItem();
        }

        _taskHolder = null;

        _sprite.enabled = false;
        _collider.enabled = false;
    }
}


