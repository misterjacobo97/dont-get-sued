using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : PersistentSignleton<TaskManager> {
    [NonSerialized] public UnityEvent taskListChanged = new();

    public event EventHandler<OnTaskCompletedEventArgs> OnTaskCompletedEvent;


    public class OnTaskCompletedEventArgs : EventArgs {
        public TaskInfo taskInfoCompleted;
    }

    [Header("Refs")]
    [SerializeField] private CanvasGroup _uiCanvas;
    [SerializeField] private Transform _tasksParent;

    [SerializeField] private List<TaskInfo> _taskList = new();
    [SerializeField] private TaskDatabaseSO _taskDB;

    public List<TaskInfo> GetTaskList => _taskList;

    [Header("params")]
    [SerializeField] private float _taskInterval = 10;

    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;

    private void Start() {
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

        if (GameManager.Instance.GetGameState.CurrentValue != GameManager.GAME_STATE.MAIN_GAME) _uiCanvas.alpha = 0;
    }


    private async void AssignTaskTimer() {
        if (GameManager.Instance.GetGameState.CurrentValue != GameManager.GAME_STATE.MAIN_GAME) return;

        _logger.Log("starting assign task timer", this, _showDebugLogs);

        await Awaitable.WaitForSecondsAsync(_taskInterval);

        // find any unnasigned tasks and activate them
        List<TaskInfo> inactiveTasks = _taskDB.GetTaskItemList.Where(t => t.state == TaskObject.TASK_STATE.INACTIVE && t.task is SpoiledFoodTask).ToList();

        //  check in case all tasks are currently active
        if (inactiveTasks.Count > 0) {
            int taskIdx = UnityEngine.Random.Range(0, inactiveTasks.Count - 1);

            if (taskIdx > -1) {
                inactiveTasks[taskIdx].task.ActivateTask();
            }
        }

        AssignTaskTimer();
    }

    private void ResetManager() {
        _logger.Log("manager was reset", this, _showDebugLogs);
        _taskList.Clear();
    }


}
