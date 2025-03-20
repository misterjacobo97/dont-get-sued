using System.Collections.Generic;
using UnityEngine;

public class NPCItemHolder : MonoBehaviour, I_ItemHolder {

    [Header("Refs")]
    [SerializeField] private List<Transform> _itemTargets = new();
    [SerializeField] private List<HoldableItem_SO> _acceptedItems = new();
    [SerializeField] private AudioClip _pickUpSound;


    private List<HoldableItem> _heldItems = new();

    private void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();

        //if (_movement != Vector2.zero) {
        //    _itemTarget.localPosition = _movement / 2;
        //}
    }

    public void CompleteItems() {
        _heldItems.ForEach(i => {
            (i as SpoiledFoodTask).CompleteTask();
        });
    }

    public List<HoldableItem_SO> GetAcceptedItems() {
        return _acceptedItems;
    }

    public void SetItem(HoldableItem newItem) {
        Debug.Log("holding new item");

        _heldItems.Add(newItem);
        SoundManager.Instance.PlaySound(_pickUpSound);
    }

    public bool HasItem() {
        if (_heldItems.Count == 0) {
            return false;
        }
        else return true;
    }

    public HoldableItem GetHeldItem() {
        if (_heldItems.Count == 0) {
            Debug.Log("no held item");
            return null;
        }

        return _heldItems[0];
    }


    public Transform GetItemTargetTransform() {
        if (_itemTargets == null) {
            Debug.Log("no item taget ref at: " + this);
            return null;
        }

        return _itemTargets[_heldItems.Count - 1];
    }

    public void RemoveItem() { }
    public void RemoveItem(int idx) {
        _heldItems.RemoveAt(idx);
    }

    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }



}

