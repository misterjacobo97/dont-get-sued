using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : PersistentSignleton<TaskManager> {

    [NonSerialized] public UnityEvent taskListChanged = new();

    [Header("Refs")]
    [SerializeField] private CanvasGroup _uiCanvas;
    [SerializeField] private TextMeshProUGUI _tasksLeftText;
    [SerializeField] private TextMeshProUGUI _tasksCompletedText;
    [SerializeField] private Transform _tasksParent;

    [SerializeField] private List<TaskInfo> _taskList = new();
    public List<TaskInfo> GetTaskList => _taskList;

    [Header("params")]
    [SerializeField] private float _taskInterval = 10;

    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;

    private void Start() {
        taskListChanged.AddListener(UpdateUI);

        GameManager.instance.GameStateChanged.AddListener(state => {
            switch (state) {
                case GameManager.GAME_STATE.MAIN_GAME:
                    _uiCanvas.alpha = 1;
                    AssignTaskTimer();
                    return;

                case GameManager.GAME_STATE.START_SCREEN or GameManager.GAME_STATE.END_GAME or GameManager.GAME_STATE.SUED or GameManager.GAME_STATE.PRE_GAME:

                    _uiCanvas.alpha = 0;
                    ResetManager();
                    return;
            }
        });

        if (GameManager.Instance.GetGameState != GameManager.GAME_STATE.MAIN_GAME) _uiCanvas.alpha = 0;
    }

    public void AddTaskToList(TaskObject newTask, I_ItemHolder container = null) {
        // in case it exists, just change the parent
        if (_taskList.Exists(t => t.task == newTask)) {
            _taskList.Find(t => t.task == newTask).ChangeTaskHolder(container);

        }
        else {
            TaskInfo newTaskInfo = new TaskInfo(newTask.GetInstanceID(), newTask, container);

            _taskList.Add(newTaskInfo);

            newTaskInfo.ChangedTaskHolder.AddListener(newHolder => { _logger.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new holder: " + newHolder, this, _showDebugLogs); });
            newTaskInfo.ChangedAssignedNPC.AddListener(newNPC => { _logger.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new assigned NPC: " + newNPC, this, _showDebugLogs); });
            newTaskInfo.ChangedTaskState.AddListener(newState => { _logger.Log("taskInfo at pos: " + _taskList.IndexOf(newTaskInfo) + " has a new State: " + newState, this, _showDebugLogs); });
        }
    }

    public void RemoveNPCFromAssignments(NPCController npc) {
        foreach (var item in _taskList.Where(task => task.assignedNPC == npc)) {
            item.assignedNPC = null;
        }
    }

    private async void AssignTaskTimer() {
        if (GameManager.Instance.GetGameState != GameManager.GAME_STATE.MAIN_GAME) return;

        _logger.Log("starting assign task timer", this, _showDebugLogs);

        await Awaitable.WaitForSecondsAsync(_taskInterval);

        // find any unnasigned tasks and activate them
        List<TaskInfo> inactiveTasks = GetTaskList.Where(t => t.state == TaskObject.TASK_STATE.INACTIVE).ToList();

        //  check in case all tasks are currently active
        if (inactiveTasks.Count > 0) {
            int taskIdx = UnityEngine.Random.Range(0, inactiveTasks.Count - 1);

            if (taskIdx > -1) {
                inactiveTasks[taskIdx].task.ActivateTask();
            }
        }

        AssignTaskTimer();
    }

    private void UpdateUI() {
        if (GameManager.instance.GetGameState != GameManager.GAME_STATE.MAIN_GAME) return;

        _tasksCompletedText.text = _taskList.Where(t => t.state == TaskObject.TASK_STATE.COMPLETED).ToList().Count.ToString();
        _tasksLeftText.text = _taskList.Where(t => t.state == TaskObject.TASK_STATE.ACTIVE).ToList().Count.ToString();
    }

    private void ResetManager() {
        _logger.Log("manager was reset", this, _showDebugLogs);
        _taskList.Clear();
    }


}
