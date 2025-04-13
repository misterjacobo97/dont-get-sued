using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// a class to store info about tasks, and any info relating to them
/// </summary>
[Serializable]
public class TaskInfo {
    [NonSerialized] public UnityEvent<TaskObject.TASK_STATE> ChangedTaskState = new();
    [NonSerialized] public UnityEvent<I_ItemHolder> ChangedTaskHolder = new();
    //[NonSerialized] public UnityEvent<NPCController> ChangedAssignedNPC = new();

    public TaskInfo(int newId, TaskObject newTask, I_ItemHolder container = null/*, NPCController npc = null*/) {
        id = newId;
        task = newTask;
        taskHolder = container;
        //assignedNPC = npc;

        ConnectEventListeners();
    }

    public int id;
    public TaskObject.TASK_STATE state = TaskObject.TASK_STATE.INACTIVE;
    public TaskObject task;
    public I_ItemHolder taskHolder;
    //public NPCController assignedNPC;

    /// <summary>
    /// Use to change the assigned NPC for this task, leave empty to remove any assignment previously made.
    /// </summary>
    /// <param name="newNpc"></param>
    //public void ChangeAssignedNPC(NPCController newNpc = null) {
    //    assignedNPC = newNpc;
    //    ChangedAssignedNPC.Invoke(assignedNPC);

    //}

    public void ChangeTaskHolder(I_ItemHolder newHolder) {
        taskHolder = newHolder;
        ChangedTaskHolder.Invoke(taskHolder);
        //ChangeAssignedNPC(null);
    }

    private void ChangeTaskState(TaskObject.TASK_STATE newState) {
        state = newState;
        ChangedTaskState.Invoke(state);

    }

    private void ConnectEventListeners() {
        // connect to events
        task.ChangedParentHolder.AddListener(ChangeTaskHolder);

        task.ChangedTaskState.AddListener(ChangeTaskState);
    }


};
