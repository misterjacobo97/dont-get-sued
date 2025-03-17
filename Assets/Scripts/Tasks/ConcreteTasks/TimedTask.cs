using UnityEngine;
using UnityEngine.UI;
public class TimedTask : TaskObject {
    [Header("TimedTask Refs")]
    [SerializeField] private Image _timerRadial;
    [SerializeField] protected Canvas _timerIcon;

    [Header("Params")]
    [SerializeField] private float _timeToFail;

    private float _timeLeft;

    protected void Awake() {
        _timerIcon.enabled = false;
        ChangedTaskState.AddListener(state => {
            if (state == TASK_STATE.ACTIVE) _timerIcon.enabled = true;
        });
    }

    protected new void Start() {
        base.Start();
        _timeLeft = _timeToFail;
        _taskActive = true;
    }

    private void Update() {
        if (/*_taskActive */ state == TASK_STATE.ACTIVE) {
            _timeLeft -= Time.deltaTime;

            _timerRadial.fillAmount = _timeLeft / _timeToFail;


            if (_timeLeft <= 0) {
                _taskActive = false;
                FailTask();
            }
        }
    }

    //public new void ActivateTask() {


    //    _timerIcon.enabled = true;        
    //    base.ActivateTask();
    //}

    public new void DeactivateTask() {

        _timerIcon.enabled = false;
        base.DeactivateTask();

        
    }

}
