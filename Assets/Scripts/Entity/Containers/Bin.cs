using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour, I_ItemHolder, I_Interactable {
    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemTarget;
    [SerializeField] private List<ScriptableObject> _acceptedItems = new();

    private HoldableItem _heldItem = null;

    protected void Start() {
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
    }

    private void OnSelectedInteractableChanged(object sender, PlayerInteract.OnSelectedInteractableChangedEventArgs e) {
        if (e.selectedInteractable == (I_Interactable)this) {
            SetSelected();
        }
        else {
            SetUnselected();
        }
    }

    public void SetSelected() {
        _sprite.color = Color.white;
    }

    public void SetUnselected() {
        _sprite.color = Color.red;
    }

    public void Interact(object caller) {
        if (caller is PlayerInteract) {


            if ((caller as PlayerInteract).GetItemHolder().HasItem() && HasItem() == false) {
                Debug.Log("receiving task from: " + caller.ToString());

                // set holdable parent to this
                (caller as PlayerInteract).GetItemHolder().GetHeldItem().ChangeParent(this);
            }
        }
    }

    public void SetItem(HoldableItem newItem) {
        _heldItem = newItem;

        // complete if the item is a task
        CompleteTaskItem();

        // then remove and destroy the item
        HoldableItem item = _heldItem;

        RemoveItem();

        Destroy(item.gameObject);
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

    private void CompleteTaskItem() {
        if (_heldItem is not TaskObject) return;

        (_heldItem as TaskObject).CompleteTask();
    }


    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }

}
