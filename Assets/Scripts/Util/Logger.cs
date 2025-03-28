using UnityEngine;

/// <summary>
/// A util class to allow dynamic debug logging within classes
/// </summary>
public class Logger : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] bool _showLog;

    public void Log(object message, Object sender, bool enabled) {
        if (_showLog && enabled) {

            Debug.Log(message, sender);

        }
    }
}
