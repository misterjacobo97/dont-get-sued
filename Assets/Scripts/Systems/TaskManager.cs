using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : PersistentSignleton<TaskManager> {

    [NonSerialized] public UnityEvent taskListChanged = new();

    [Header("Refs")]
    [SerializeField] private TextMeshProUGUI _tasksLeftText;
    [SerializeField] private TextMeshProUGUI _tasksCompletedText;
    [SerializeField] private Transform _tasksParent;

    [SerializeField] private List<TaskInfo> _taskList = new();
    public List<TaskInfo> GetTaskList => _taskList; 


    private void Start() {
        taskListChanged.AddListener(UpdateUI);

        AssignTaskTimer();
    }

    public void AddTaskToList(TaskObject newTask, I_ItemHolder container = null) {


        // in case it exists, just change the parent
        if (_taskList.Exists(t => t.task == newTask)) {
            _taskList.Find(t => t.task == newTask).ChangeTaskHolder(container);

        }
        else {
            TaskInfo newTaskInfo = new TaskInfo(newTask.GetInstanceID(), newTask, container);

            _taskList.Add(newTaskInfo);

            newTaskInfo.ChangedTaskHolder.AddListener(newHolder => { Debug.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new holder: " + newHolder); });
            newTaskInfo.ChangedAssignedNPC.AddListener(newNPC => { Debug.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new assigned NPC: " + newNPC); });
            newTaskInfo.ChangedTaskState.AddListener(newState => { Debug.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new State: " + newState); });

        }
    }

    public void RemoveNPCFromAssignments(NPCController npc) {
        foreach (var item in _taskList.Where(task => task.assignedNPC == npc)) {
            item.assignedNPC = null;
        }
    }

    private async void AssignTaskTimer() {
        Debug.Log("starting assign task timer");

        await Awaitable.WaitForSecondsAsync(3);

        // find any unnasigned tasks and activate them
        List<TaskInfo> inactiveTasks = GetTaskList.Where(t => t.state == TaskObject.TASK_STATE.INACTIVE).ToList();
        int taskIdx = UnityEngine.Random.Range(0, inactiveTasks.Count - 1);

        if (taskIdx > -1) {
            inactiveTasks[taskIdx].task.ActivateTask();
        }

        AssignTaskTimer();
    }

    private void UpdateUI() {
        _tasksCompletedText.text = _taskList.Where(t => t.state == TaskObject.TASK_STATE.COMPLETED).ToList().Count.ToString();
        _tasksLeftText.text = _taskList.Where(t => t.state == TaskObject.TASK_STATE.ACTIVE).ToList().Count.ToString();
    }
}
