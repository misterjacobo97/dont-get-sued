using UnityEngine;

public class PlayerItemHolder : MonoBehaviour, I_ItemHolder {

    [Header("Refs")]
    [SerializeField] private Transform _itemTarget;

    private HoldableItem _heldItem = null;

    private void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();

        if (_movement != Vector2.zero) {
            _itemTarget.localPosition = _movement / 2;
        }
    }

    public void SetItem(HoldableItem newItem) {
        Debug.Log("holding new item");

        _heldItem = newItem;

        _heldItem.transform.position = _itemTarget.position;
    }

    public bool HasItem() {
        if (_heldItem == null) {
            return false;
        }
        else return true;
    }

    public HoldableItem GetHeldItem() {
        if (_heldItem == null) {
            Debug.Log("no held item");
            return null;
        }

        HoldableItem item = _heldItem;
        _heldItem = null;
        return item;
    }


    public Transform GetItemTargetTransform() {
        if (_itemTarget == null) {
            Debug.Log("no item taget ref at: " + this);
            return null;
        }

        return _itemTarget;
    }

    public void RemoveItem() {
        _heldItem = null;
    }
}
