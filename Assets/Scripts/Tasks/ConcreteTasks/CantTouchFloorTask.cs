using UnityEngine;

public class CantTouchFloorTask : MonoBehaviour {
    
    bool _isOnFloor = false;
    [SerializeField] private ScoreObject _scoreObject;
    [SerializeField] private float _intervalDuration = 1.5f;
    [SerializeField] private int _score = -1;
    [SerializeField] private GameStatsSO _gamestate;

    private float _addScoreTimer = 0f;


    private void Awake() {
        _addScoreTimer = _intervalDuration;
    }

    private void Update() {
        _isOnFloor = transform.parent == null;

        if (_isOnFloor == false) {
            _addScoreTimer = _intervalDuration;
            return;
        }

        _addScoreTimer -= Time.deltaTime;

        if (_addScoreTimer <= 0f){
            _addScoreTimer = _intervalDuration;

            _gamestate.managementSatisfaction.AddToReactiveValue(_score);
            GameObject.Instantiate(_scoreObject, null).Init(_score, transform.position);
        }

    }



}
