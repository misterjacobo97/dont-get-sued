using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tasks {
    public class TaskManager : PersistentSignleton<TaskManager> {
        [Header("Refs")]
        [SerializeField] private TextMeshProUGUI _tasksLeftText;
        [SerializeField] private TextMeshProUGUI _tasksCompletedText;
        [SerializeField] private Transform _tasksParent;

        [SerializeField] private List<TaskObject> _unassignedTasks = new();
        [SerializeField] private List<TaskObject> _activeTasks = new();

        private List<TaskObject> _completedTasks = new();
        private List<TaskObject> _failedTasks = new();
        private List<I_ItemHolder> _taskReceivers = new();

        private void Start() {
            AssignTaskTimer();
        }

        private async void AssignTaskTimer() {
            Debug.Log("starting assign task timer");
            if (_unassignedTasks.Count <= 0) {
                return;
            }

            await Awaitable.WaitForSecondsAsync(3);

            AssignNewTask();

            AssignTaskTimer();
        }

        public void AddTaskReceiver(I_ItemHolder newReceiver) {
            if (_taskReceivers.Contains(newReceiver)) {
                return;
            }

            _taskReceivers.Add(newReceiver);
        }

        private void AssignNewTask() {
            if (_unassignedTasks.Count > 0) {
                // check for recievers that accept the task
                List<I_ItemHolder> availableReceivers = GetAvailableReceiversForTask(_unassignedTasks[0].holdableItem_SO);

                if (availableReceivers.Count <= 0) {
                    return;
                }

                // instantiate and then delete from list
                TaskObject newTask = GameObject.Instantiate(_unassignedTasks[0]);
                _unassignedTasks.Remove(_unassignedTasks[0]);

                _activeTasks.Add(newTask);

                I_ItemHolder selectedHolder = availableReceivers[Random.Range(0, availableReceivers.Count - 1)];

                Debug.Log("assigning new task to: " + selectedHolder);

                // set the new parent of the task to be the selected holder
                newTask.ChangeParent(selectedHolder);

                // update tasks left text
                UpdateUI();
            }
        }

        private List<I_ItemHolder> GetAvailableReceiversForTask(HoldableItem_SO currentTask) {
            return _taskReceivers.FindAll((tr) => tr.HasItem() == false && tr.IsItemAccepted(currentTask));
        }

        public void AddCompletedTask(TaskObject newCompletedTask) {
            _activeTasks.Remove(newCompletedTask);
            _completedTasks.Add(newCompletedTask);

            newCompletedTask.transform.parent = _tasksParent;
            newCompletedTask.transform.position = _tasksParent.position;
            UpdateUI();
        }

        public void AddToFailedTasks(TaskObject newTask) {

            _activeTasks.Remove(newTask);
            _failedTasks.Add(newTask);
            newTask.transform.parent = _tasksParent;
            newTask.transform.position = _tasksParent.position;

            UpdateUI();
        }

        private void UpdateUI() {
            _tasksCompletedText.text = _completedTasks.Count.ToString();
            _tasksLeftText.text = _activeTasks.Count.ToString();
        }
    }
}
