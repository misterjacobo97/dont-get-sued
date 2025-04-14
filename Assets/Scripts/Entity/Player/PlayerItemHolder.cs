using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHolder : MonoBehaviour, I_ItemHolder {

    [Header("Refs")]
    [SerializeField] private Transform _itemTarget;
    //[SerializeField] private List<HoldableItem_SO> _acceptedItems = new();
    [SerializeField] private AudioClip _pickUpSound;
    [SerializeField] private ScriptableObjectListReference _acceptedItems;


    [Header("throwing params")]
    [SerializeField] private float _throwForce = 200f;

    private HoldableItem _heldItem = null;

    private void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();

        if (_movement != Vector2.zero) {
            _itemTarget.localPosition = _movement / 2;
        }
    }

    //public List<HoldableItem_SO> GetAcceptedItems() {
    //    return _acceptedItems.list;
    //}

    public void SetItem(HoldableItem newItem) {
        Debug.Log("holding new item");

        _heldItem = newItem;
        SoundManager.Instance.PlaySound(_pickUpSound);
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

        return _heldItem;
    }


    public Transform GetItemTargetTransform() {
        if (_itemTarget == null) {
            Debug.Log("no item taget ref at: " + this);
            return null;
        }

        return _itemTarget;
    }

    public void ThrowItem(Vector2 dir) {
        if (_heldItem == null) return;

        _heldItem.ThrowItem(dir, _throwForce);
    }

    public void RemoveItem() {
        _heldItem = null;
    }

    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }

}
