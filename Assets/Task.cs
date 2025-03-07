using System.Threading.Tasks;
using UnityEngine;

public class TaskHolder : MonoBehaviour {
    [Header("Refs")]
    [SerializeField] private SpriteRenderer marker;

    public object TaskObject; // TaskDecorator task;
    private bool _taskActive = false;

    private void Start() {
        marker.enabled = false;

        StartCountdown();
    }

    private async void StartCountdown() {

        await Awaitable.WaitForSecondsAsync(Random.Range(10, 30));

        EnableTask();

    }

    private void EnableTask() {
        _taskActive = true;
        marker.enabled = true;
    }
}
