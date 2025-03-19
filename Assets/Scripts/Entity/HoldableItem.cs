using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// the base holdable item class. This allows classes with the I_ItemHolder Interface to interact with these. 
/// HoldableItems are responsible for changing their own parent.
/// </summary>
public class HoldableItem : MonoBehaviour, I_Interactable {
    [NonSerialized] public UnityEvent<I_ItemHolder> ChangedParentHolder = new();


    [Header("holdable refs")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] public HoldableItem_SO holdableItem_SO;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected SpriteRenderer _sprite;

    private bool _heldState = false;
    protected I_ItemHolder _parentHolder = null;

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

    protected void SetHeldState(bool newState) {
        _heldState = newState;

        if (GetHeldState() == false) {

            _rb.simulated = true;
            _collider.gameObject.SetActive(true);
        }
        else {
            // reset rotation
            _rb.transform.rotation = Quaternion.Euler(0,0,0);

            _rb.simulated = false;
            _collider.gameObject.SetActive(false);
        }
    }

    public bool GetHeldState() => _heldState;
    public I_ItemHolder GetParentHolder() => _parentHolder;

    /// <summary>
    /// This should be used for whenever you want to try to change the parent of a holdable item. 
    /// It will check if the new parent allows items of the same kind and will automatically set its position to it.
    /// </summary>
    /// <param name="newParent"></param>
    public void ChangeParent(I_ItemHolder newParent) {
        if (newParent.IsItemAccepted(holdableItem_SO) == false) {
            return;
        }

        newParent.SetItem(this);

        _parentHolder = newParent;
        transform.position = newParent.GetItemTargetTransform().position;
        transform.parent = newParent.GetItemTargetTransform();

        SetHeldState(true);
        ChangedParentHolder.Invoke(_parentHolder);
    }

    public void DropItem() {
        transform.parent = null;

        _parentHolder.RemoveItem();
        _parentHolder = null;
        SetHeldState(false);
    }

    public void SetSelected() { }
    public void SetUnselected() { }
    public void Interact(object caller) {
        if (holdableItem_SO.pickableItem == false) {
            return;
        }
        
        if (caller is PlayerInteract) {

            if (GetHeldState() == false) {
                ChangeParent((caller as PlayerInteract).GetItemHolder());
            }
        }
    }
}
