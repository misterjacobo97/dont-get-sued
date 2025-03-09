using UnityEngine;

public abstract class TaskHolder : MonoBehaviour {
    [Header("Refs")]
    //[SerializeField] private Transform marker;
    [SerializeField] private TaskObject _taskObject;
    [SerializeField] private Transform _taskObjectPos;


    protected virtual void Start() {
        //marker.enabled = false;

    }

    public TaskObject GetTaskObject => _taskObject;

    public void SetTaskObject(TaskObject taskObject) {
        this._taskObject = taskObject;

        _taskObject.SetTaskHolder(this);
        _taskObject.transform.parent = _taskObjectPos;
        _taskObject.transform.position = _taskObjectPos.position;

        //marker.sprite = _taskObject.GetTaskObject_SO().iconSprite;
        //marker.enabled = true;
    }

    public void ClearTaskObject() {
        _taskObject = null;

        //marker.sprite = null;
        //marker.enabled = true;

    }

    public bool HasTaskObject() {
        return _taskObject != null;
    }

    public void CompleteTask() {
        _taskObject.CompleteTask();
    }

}
