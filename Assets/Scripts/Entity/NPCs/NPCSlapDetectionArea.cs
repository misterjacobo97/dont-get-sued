using R3;
using UnityEngine;

public class NPCSlapDetectionArea : MonoBehaviour {

    [SerializeField] private NPCStateController _parentRef;
    [SerializeField] private BaseStateSO _stateEnumObj;
    [SerializeField] private Collider2D _triggerCollider;
    [SerializeField] private LayerMask _slapAreaLayer;


    private void Start() {
        _parentRef.currentState.AsObservable().Subscribe(state => {
            if (state == _stateEnumObj) _triggerCollider.enabled = false;
            else _triggerCollider.enabled = true;
        }).AddTo(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == _slapAreaLayer) {
            _parentRef.isSlapped.Value = true;

            _parentRef.isSlapped.Value = false;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject.layer == _slapAreaLayer) {
    //        _parentRef.isSlapped.Value = true;
    //    }
    //}
}
