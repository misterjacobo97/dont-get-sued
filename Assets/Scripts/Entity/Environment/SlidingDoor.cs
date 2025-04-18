using DG.Tweening;
using R3;
using R3.Triggers;
using UnityEngine;

public class SlidingDoor : MonoBehaviour {
    private enum STATE {
        CLOSED,
        CLOSING,
        OPEN,
        OPENING
    }

    private STATE _state = STATE.CLOSED;

    [Header("refs")]
    [SerializeField] private Transform _door1;
    [SerializeField] private Transform _door2;

    [Header("params")]
    [SerializeField] private string _triggerCollisionLayerName;
    [SerializeField] private float _openDuration = 3;
    [SerializeField] private float _movementDuration = 1;

    private Sequence _tweener;
    [SerializeField] private SerializableReactiveProperty<float> _openTimeLeft;

    private void Awake() {
        this.OnTriggerEnter2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer == LayerMask.NameToLayer(_triggerCollisionLayerName)){
                OpenDoors();
            }
        }).AddTo(this);

        _openTimeLeft = new SerializableReactiveProperty<float>(0).AddTo(this);

        _openTimeLeft.AsObservable().Subscribe(time => {
            if (time <= 0) CloseDoors();
        }).AddTo(this);
    }

    private void Update() {
        if (_openTimeLeft.Value > 0){ 
            _openTimeLeft.Value -= Time.deltaTime;
        }
        else if (_openTimeLeft.Value < 0){
            _openTimeLeft.Value = 0;
        } 
    }

    private void OpenDoors(){
        if (_state == (STATE.OPEN | STATE.OPENING)) {
            _openTimeLeft.Value = _openDuration;
            return;
        }

        if (_tweener != null && _tweener.active) {
            _tweener.Kill();
        }

        _state = STATE.OPENING;
        _tweener = DOTween.Sequence();

        _tweener.Append(_door1.DOLocalMoveY(2.4f, _movementDuration))
            .Insert(0, _door2.DOLocalMoveY(-2.4f, _movementDuration))
            .OnComplete(() => {
                _state = STATE.OPEN;
                _openTimeLeft.Value = _openDuration;
            });
        
    }
    private void CloseDoors(){
        if (_state == (STATE.CLOSED | STATE.CLOSING)) return;

        if (_tweener != null && _tweener.active) {
            _tweener.Kill();
        }
        
        _state = STATE.CLOSING;
        _tweener = DOTween.Sequence();

        _tweener.Append(_door1.DOLocalMoveY(0.88f, _movementDuration))
            .Insert(0, _door2.DOLocalMoveY(-0.88f, _movementDuration))
            .OnComplete(() => {
                _state = STATE.CLOSED;
            });

    }

}
