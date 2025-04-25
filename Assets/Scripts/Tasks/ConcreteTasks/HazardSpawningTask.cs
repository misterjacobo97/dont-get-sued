using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;
public class HazardSpawningTask : MonoBehaviour {
    private enum HAZARD_SPAWNING_TASK_STATE {
        ACTIVE,
        INACTIVE
    }

    [Header("hazard spawner Refs")]
    [SerializeField] private Image _timerRadial;
    [SerializeField] protected Canvas _timerIcon;
    [SerializeField] private Transform _hazardToSpawn;


    [Header("Params")]
    [SerializeField] private float _timeToSpawn = 10;
    [SerializeField] private SerializableReactiveProperty<HAZARD_SPAWNING_TASK_STATE> _state; 
    [SerializeField] private SerializableReactiveProperty<float> _timeLeft ;

    protected void Awake() {
        _timerIcon.enabled = false;
        _timeLeft = new SerializableReactiveProperty<float>(_timeToSpawn);
        _state = new SerializableReactiveProperty<HAZARD_SPAWNING_TASK_STATE>(HAZARD_SPAWNING_TASK_STATE.INACTIVE);

        _timeLeft.AsObservable().Subscribe(t => {
            if (t > 0) return;

            SpawnHazard();
        }).AddTo(this);
    }

    private void Update() {
        if (transform.parent == null && _state.Value != HAZARD_SPAWNING_TASK_STATE.ACTIVE) {
            _timerIcon.enabled = true;                
            _state.Value = HAZARD_SPAWNING_TASK_STATE.ACTIVE;
        }
        else if (transform.parent != null && _state.Value != HAZARD_SPAWNING_TASK_STATE.INACTIVE) {
            _state.Value = HAZARD_SPAWNING_TASK_STATE.INACTIVE;
            _timerIcon.enabled = false;
        }

        if (_timeLeft.Value > 0 && _state.Value == HAZARD_SPAWNING_TASK_STATE.ACTIVE) {
            _timeLeft.Value -= Time.deltaTime;

            _timerRadial.fillAmount = _timeLeft.Value / _timeToSpawn;
        }


    }

    private void OnDestroy() {
        Disposable.Dispose(_state, _timeLeft);
    }

    private void SpawnHazard(){
        Transform haz = Transform.Instantiate(_hazardToSpawn, null);
        haz.position = transform.position;

        Destroy(gameObject);
    }

}
