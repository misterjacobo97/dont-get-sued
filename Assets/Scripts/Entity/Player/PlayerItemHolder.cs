using UnityEngine;

public class PlayerItemHolder : MonoBehaviour, I_ItemHolder {

    [Header("Refs")]
    [SerializeField] private Transform _itemTarget;

    private Transform _heldItem = null;

    public void SetItem(Transform newItem) {
        Debug.Log("holding new item");

        _heldItem = newItem;

        _heldItem.position = _itemTarget.position;
        _heldItem.parent = _itemTarget;
    }

    public bool HasItem() {
        return _heldItem != null;
    }

    public Transform GetHeldItem() {
        if (_heldItem == null) {
            Debug.Log("no held item");
            return null;
        }

        return _heldItem;
    }


    public Transform GetItemTargetTransform() { 
        if (_itemTarget == null) {
            Debug.Log("no item taget ref at: " + this);
            return null;
        }

        return _itemTarget;
    }
}
