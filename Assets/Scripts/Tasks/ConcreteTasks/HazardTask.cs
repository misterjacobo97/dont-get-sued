using System.Collections.Generic;
using UnityEngine;

public class HazardTask : TaskObject {
    public enum HAZARD_TYPE {
        PUDDLE
    }

    [Header("Hazard refs")]
    [SerializeField] private HoldableItem_SO _hazardNullifyer;
    private List<Collider2D> _nullifyers = new();

    [Header("hazard params")]
    protected bool _hazardSafeState = false;
    [SerializeField] protected HAZARD_TYPE _hazardType;

    private void OnTriggerEnter2D(Collider2D collision) {
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
}
