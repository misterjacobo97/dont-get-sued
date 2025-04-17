using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Systems/TaskDatabase")]
public class TaskDatabaseSO : ScriptableObject {
    [SerializeField] private List<TaskInfo> _taskItemList;
    public List<TaskInfo> GetTaskItemList => _taskItemList;

    private void OnEnable () {
        _taskItemList?.Clear();

        SceneManager.activeSceneChanged += (a, b) => {
            _taskItemList?.Clear();
        };
    }

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
