using Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TaskObject : MonoBehaviour {
    [Header("Refs")]
    [SerializeField] protected TaskObject_SO taskObject_SO;
    [SerializeField] protected Image _taskIcon;

    protected TaskHolder _taskHolder;
    protected bool _taskActive;

    public TaskObject_SO GetTaskObject_SO() {
        return taskObject_SO;
    }

    public void SetTaskHolder (TaskHolder newTaskHolder) { 
        if (_taskHolder != null) {
            _taskHolder.ClearTaskObject();
        }

        if (newTaskHolder.HasTaskObject()) {
            Debug.Log("task holder already has task!");
        }

        _taskHolder = newTaskHolder;

        if (_taskHolder.GetTaskObject != this) {
            _taskHolder.SetTaskObject(this);
        }

        _taskActive = true;
    }

    public TaskHolder GetTaskHolder() {
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

    protected virtual void DeactivateTask() {
        _taskActive = false;

        if (_taskHolder != null) {
            _taskHolder.ClearTaskObject();
        }

        _taskHolder = null;

        _taskIcon.enabled = false;
    }
}


