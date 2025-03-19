using System.Collections.Generic;
using UnityEngine;

public class BaseShelf : MonoBehaviour, I_ItemHolder, I_Interactable {

    enum ShelfType {
        SINGLE_TASK,
        MULTI_TASK
    }

    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemTarget;
    [SerializeField] private List<HoldableItem_SO> _acceptedItems = new();
    [SerializeField] private Transform _customerMarker;


    [Header("Params")]
    [SerializeField] private ShelfType _shelfType = ShelfType.SINGLE_TASK;
    [SerializeField] private bool _showGizmos = true;

    private HoldableItem _heldItem = null;

    protected  void Start() {
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;

        // if target gameobject has a child, check if its allowed and set it up as held item
        if (_itemTarget.GetChild(0) != null) {
            (_itemTarget.GetChild(0).GetComponent<HoldableItem>()).ChangeParent(this);
        }
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
        _sprite.color = Color.yellow;
    }

    public void SetUnselected() { 
        _sprite.color = Color.white;
    }

    public void Interact(object caller) {
        if (caller is PlayerInteract) {
            if (_heldItem != null) {
                _heldItem.ChangeParent((caller as PlayerInteract).GetItemHolder());
            }
        }
    }

    public void SetItem(HoldableItem newItem) {
        _heldItem = newItem;
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
            return null;
        }

        return _itemTarget;
    }

    public void RemoveItem() {
        _heldItem = null;
    }


    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }

    public Transform GetCustomerTarget() {
        return _customerMarker;
    }


    private void OnDrawGizmos() {
        if (_showGizmos) {
            Gizmos.DrawCube(_customerMarker.position, new(0.1f,0.1f,0));
            Gizmos.color = Color.green;
        }
    }
}
