using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Events;

public class AreaTriggerObservable : MonoBehaviour {

    public UnityEvent AreaEnterEvent = new();
    public UnityEvent AreaExitedEvent = new();

    [SerializeField] private string _triggerLayerName;

    [SerializeField] private Color _gizmoColor = Color.clear;

    private void Awake() {
        this.OnTriggerEnter2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer == LayerMask.NameToLayer(_triggerLayerName)) {
                AreaEnterEvent.Invoke();
            }
        }).AddTo(this);

        this.OnTriggerEnter2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer == LayerMask.NameToLayer(_triggerLayerName)) {
                AreaExitedEvent.Invoke();
            }
        }).AddTo(this);
    }

    private void OnDrawGizmos() {
        Gizmos.color = _gizmoColor;

        if (TryGetComponent(out Collider2D collider)) {
            Gizmos.DrawCube(collider.transform.position, collider.bounds.size);

        }
    }
}
