using System.Collections.Generic;
using UnityEngine;

public class SlipHazardTask : TaskObject {

    [Header("Hazard refs")]
    [SerializeField] private HoldableItem_SO _hazardNullifyer;
    private List<Collider2D> _nullifyers = new();

    [Header("hazard params")]
    [SerializeField] private string _NPCLayerName;

    protected bool _hazardSafeState = false;

    public bool IsSafe => _hazardSafeState;

    [Header("context")]
    [SerializeField] private FloatReference _customerSatisfactionScore;
    [SerializeField] private FloatReference _managementSatisfactionScore;


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer(_NPCLayerName)) {
            Vector2 dir = (transform.position - collision.transform.position).normalized;

            collision.GetComponentInParent<NPCStateController>().Slip(dir, 2, IsSafe ? false : true);
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
