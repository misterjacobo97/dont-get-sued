//using System;
using R3;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// the base holdable item class. This allows classes with the I_ItemHolder Interface to interact with these. 
/// HoldableItems are responsible for changing their own parent.
/// </summary>
public class HoldableItem : MonoBehaviour, I_Interactable {
    [System.NonSerialized] public UnityEvent<Transform> ChangedParentHolder = new();

    [Header("holdable refs")]
    private Rigidbody2D _rb;
    [SerializeField] public HoldableItem_SO holdableItem_SO;
    private Collider2D _collider;
    private SpriteRenderer _sprite;
    private I_Interactable interactableRef;


    private bool _heldState = false;
    protected I_ItemHolder _parentHolder = null;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        interactableRef = GetComponent<I_Interactable>();
    }

    protected void Start() {
        holdableItem_SO.playerInteractContext.selectedInteractableObject.AsObservable().Subscribe((item) => {
            if (item != null && item.GetComponent<I_Interactable>() == interactableRef) {
                SetSelected();
            }
            else SetUnselected();
        }).AddTo(this);
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

        if (_parentHolder != null) _parentHolder.RemoveItem();
        newParent.SetItem(this);

        _parentHolder = newParent;
        transform.position = newParent.GetItemTargetTransform().position;
        transform.parent = newParent.GetItemTargetTransform();

        SetHeldState(true);
        ChangedParentHolder.Invoke((_parentHolder as MonoBehaviour).transform);
    }

    public void DropItem() {
        transform.parent = null;

        if (_parentHolder is PlayerItemHolder) (_parentHolder as PlayerItemHolder).RemoveItem();
        else (_parentHolder as NPCItemHolder).RemoveItem(this) ;
        
        _parentHolder = null;

        SetHeldState(false);
    }

    public void SetSelected() { }
    public void SetUnselected() { }

    public void ThrowItem(Vector2 dir, float force) {
        DropItem();

        _rb.AddForce(dir * force);

    }
    public void Interact(Object caller) {
        if (holdableItem_SO.pickableItem == false) {
            return;
        }
        
        if (caller is PlayerInteract) {
            if ((caller as PlayerInteract).GetItemHolder().HasItem()) return;

            if (GetHeldState() == false) {
                ChangeParent((caller as PlayerInteract).GetItemHolder());
            }
        }
    }


}
