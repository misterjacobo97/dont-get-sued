using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Cheats_SO", menuName = "Util/Cheats_SO")]
public class Cheats_SO : ScriptableObject {
    [NonSerialized] public UnityEvent SpoilAllTasksEvent = new();

    public void SpoilAllTasks() {
        SpoilAllTasksEvent.Invoke();
    }
}
