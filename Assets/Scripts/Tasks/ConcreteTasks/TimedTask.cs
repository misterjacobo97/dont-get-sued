using UnityEngine;
using UnityEngine.UI;

public class TimedTask : TaskObject {
    [Header("TimedTask Refs")]
    [SerializeField] private Image _timerRadial;
    [SerializeField] protected Canvas _timerIcon;

    [Header("Params")]
    [SerializeField] private float _timeToFail;

    private float _timeLeft;
    protected new void Start() {
        base.Start();
        _timeLeft = _timeToFail;
        _taskActive = true;
    }

    private void Update() {
        if (_taskActive) {
            _timeLeft -= Time.deltaTime;

            _timerRadial.fillAmount = _timeLeft / _timeToFail;


            if (_timeLeft <= 0) {
                _taskActive = false;
                FailTask();
            }
        }
    }

    protected new void DeactivateTask() {

        _timerIcon.enabled = false;
        base.DeactivateTask();

        
    }

}
