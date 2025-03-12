using Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TaskObject : HoldableItem {
    [Header("Refs")]
    [SerializeField] protected TaskObject_SO taskObject_SO;
    //[SerializeField] protected SpriteRenderer _taskIcon;


    protected I_ItemHolder _taskHolder;
    protected bool _taskActive;

    public TaskObject_SO GetTaskObject_SO() {
        return taskObject_SO;
    }

    public I_ItemHolder GetItemHolder() {
        return _taskHolder;
    }

    public void CompleteTask() {
        Debug.Log("Completing task: " + this);

        DeactivateTask();

        TaskManager.Instance.AddCompletedTask(this);
    }

    public void FailTask() {
        Debug.Log("failing task: " + this);

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


