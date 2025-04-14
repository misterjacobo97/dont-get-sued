using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[CreateAssetMenu(menuName ="Systems/TaskDatabase")]
public class TaskDatabaseSO : ScriptableObject {
    [SerializeField] private List<TaskInfo> _taskItemList;
    public List<TaskInfo> GetTaskItemList => _taskItemList;

    public void AddToTaskItemList(TaskObject newItem) {
        _taskItemList.Add(
                new(
                    _taskItemList.Count,
                    newItem
                    )
            );
    }

    public bool TaskItemExists(TaskObject item) {
        return _taskItemList.Exists(t => t.task == item);
    }

}
