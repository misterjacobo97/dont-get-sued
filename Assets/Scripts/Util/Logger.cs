using UnityEngine;

/// <summary>
/// A util class to allow dynamic debug logging within classes
/// </summary>
public class Logger : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] bool _showLog;

    public void Log(object message, Object sender) {
        if (_showLog) {

            Debug.Log(message, sender);

        }
    }
}
