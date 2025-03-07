using System.Collections.Generic;

public class TaskManager : PersistentSignleton<TaskManager> {

    private List<TaskHolder> _currentTasks = new();

    public void AddTask(TaskHolder newTask) {
        _currentTasks.Add(newTask);
    }
}
