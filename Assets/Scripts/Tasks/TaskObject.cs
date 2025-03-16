using Tasks;

public class TaskObject : HoldableItem {

    public enum TASK_STATE {
        UNASSIGNED,
        ACTIVE,
        FAILED,
        COMPLETED
    }

    protected TASK_STATE state = TASK_STATE.UNASSIGNED;
    protected I_ItemHolder _taskHolder;
    protected bool _taskActive;

    public void CompleteTask() {
        DeactivateTask();

        TaskManager.Instance.AddCompletedTask(this);
    }

    public void FailTask() {
        DeactivateTask();

        TaskManager.Instance.AddToFailedTasks(this);
    }

    protected void DeactivateTask() {
        _taskActive = false;

        if (_parentHolder != null) {
            _parentHolder.RemoveItem();
        }
        _parentHolder = null;

        _sprite.enabled = false;
        _collider.enabled = false;
    }
}


