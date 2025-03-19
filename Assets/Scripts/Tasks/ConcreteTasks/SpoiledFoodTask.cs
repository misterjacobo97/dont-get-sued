using UnityEngine;
using UnityEngine.UI;
public class SpoiledFoodTask : TaskObject {
    [Header("TimedTask Refs")]
    [SerializeField] private Image _timerRadial;
    [SerializeField] protected Canvas _timerIcon;
    [SerializeField] protected Image _spoiledIcon;


    [Header("Params")]
    [SerializeField] private float _timeToSpoil;
    private bool _spoiled = false;
    public bool IsSpoiled => _spoiled;

    private float _timeLeft;

    protected void Awake() {
        _timerIcon.enabled = false;
        ChangedTaskState.AddListener(state => {
            if (state == TASK_STATE.ACTIVE) _timerIcon.enabled = true;
        });
    }

    protected new void Start() {
        base.Start();
        _timeLeft = _timeToSpoil;
        _taskActive = true;
        _spoiledIcon.enabled = false;
    }

    private void Update() {
        if (/*_taskActive */ state == TASK_STATE.ACTIVE) {
            _timeLeft -= Time.deltaTime;

            _timerRadial.fillAmount = _timeLeft / _timeToSpoil;


            if (_timeLeft <= 0) {
                if (_spoiled == false) _spoiled = true;
                if (_spoiledIcon.enabled == false) _spoiledIcon.enabled = true;
                //_taskActive = false;
                //FailTask();

                float factor = Mathf.Abs(Mathf.Sin(Time.time * 3f) * 2f);
                _spoiledIcon.transform.localScale = new Vector3(1, 1, 1) * Mathf.Clamp(factor, 0.5f, 2);
                
            }
        }
    }

    //public new void ActivateTask() {


    //    _timerIcon.enabled = true;        
    //    base.ActivateTask();
    //}

    public new void CompleteTask() {
        if (_spoiled == true) {
            _taskScore = 0;
        }

        if (_spoiled == true) {
            base.FailTask();
        }

        else base.CompleteTask();
    }

    public new void DeactivateTask() {

        _timerIcon.enabled = false;
        base.DeactivateTask();
    }

}
