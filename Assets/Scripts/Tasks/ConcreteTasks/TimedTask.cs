using Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TimedTask : TaskObject {
    [Header("TimedTask Refs")]
    [SerializeField] private Image _timerRadial;

    [Header("Params")]
    [SerializeField] private float _timeToFail;

    private float _timeLeft;


    private void Start() {
        _timeLeft = _timeToFail;
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

    protected override void DeactivateTask() {

        _timerRadial.enabled = false;
        base.DeactivateTask();

        
    }

}
