using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class SlipHazardTask : TaskObject {

    [Header("Hazard refs")]
    [SerializeField] private HoldableItem_SO _hazardNullifyer;
    private List<Collider2D> _nullifyers = new();
    [SerializeField] private SoundClipReference _spawnSound;


    [Header("hazard params")]
    [SerializeField] private string _NPCLayerName;
    [SerializeField] private float _timeToDisappear;


    protected bool _hazardSafeState = false;

    public bool IsSafe => _hazardSafeState;

    [Header("context")]
    [SerializeField] private FloatReference _customerSatisfactionScore;
    [SerializeField] private FloatReference _managementSatisfactionScore;

    protected override void Start() {
        base.Start();
        _spawnSound?.Play();

        Observable.Timer(TimeSpan.FromSeconds(_timeToDisappear)).Subscribe(x => {
            CompleteTask();
            Destroy(gameObject);
        }).AddTo(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer(_NPCLayerName)) {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            collision.GetComponentInParent<NPCStateController>().Slip(this.transform, dir, 2, IsSafe ? false : true);
        }

        if (collision.transform.parent.TryGetComponent<HoldableItem>(out HoldableItem item)) {
            if (item.holdableItem_SO == _hazardNullifyer) {
                _nullifyers.Add(collision);
            }
        }

        if (_nullifyers.Count > 0 && _hazardSafeState == false) {
            _hazardSafeState = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (_nullifyers.Contains(collision)) {
            _nullifyers.Remove(collision);
        }

        if (_nullifyers.Count == 0 && _hazardSafeState == true) {
            _hazardSafeState = false;
        }
    }

    private void OnDrawGizmos() {
        Color gizmosColor = IsSafe ? Color.green : Color.red;

        Gizmos.color = gizmosColor;
        Gizmos.DrawCube(transform.position, new Vector3(1,1,0) * 0.2f);
    }
}
