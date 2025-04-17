using System;
using UnityEngine.Events;

/// <summary>
/// a class to store info about tasks, and any info relating to them
/// </summary>
[Serializable]
public class TaskInfo {
    [NonSerialized] public UnityEvent<TaskObject.TASK_STATE> ChangedTaskState = new();
    [NonSerialized] public UnityEvent<I_ItemHolder> ChangedTaskHolder = new();

    public TaskInfo(int newId, TaskObject newTask, I_ItemHolder container = null) {
        id = newId;
        task = newTask;
    }

    public int id;
    public TaskObject.TASK_STATE state = TaskObject.TASK_STATE.INACTIVE;
    public TaskObject task;

};
