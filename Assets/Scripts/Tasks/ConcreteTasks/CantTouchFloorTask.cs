using R3;
using UnityEngine;

public class CantTouchFloorTask : MonoBehaviour {
    
    bool _isOnFloor = false;
    [SerializeField] private ScoreObject _scoreObject;
    [SerializeField] private float _intervalDuration = 1.5f;
    [SerializeField] private int _score = -1;
    [SerializeField] private GameStatsSO _gamestate;

    private void Awake() {
        Observable.EveryValueChanged(this, x => this._isOnFloor).Subscribe(state =>  {
            if (state != false){
                React();
            }
        }).AddTo(this);
    }

    private void Update() {
        _isOnFloor = transform.parent == null;
    }

    private async void React(){
        if (!_isOnFloor) return;

        _gamestate.managementSatisfaction.AddToReactiveValue(_score);
        GameObject.Instantiate(_scoreObject, null).Init(_score, transform.position);

        await Awaitable.WaitForSecondsAsync(_intervalDuration);
        
        if (transform != null) React();

    }



}
