using UnityEngine;

public class HoldableItem : MonoBehaviour, I_Interactable {
    [Header("holdable refs")]
    private Rigidbody2D _rb;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected SpriteRenderer _sprite;

    private bool _heldState = false;
    private I_ItemHolder _parentHolder = null;

    protected void Start() {
        _rb = GetComponent<Rigidbody2D>();

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

        if (_heldState == false) {
            _collider.gameObject.SetActive(true);
        }
        else _collider.gameObject.SetActive(false);
    }

    public bool GetHeldState() => _heldState;

    public void ChangeParent(I_ItemHolder newParent) {

        newParent.SetItem(this);

        _parentHolder = newParent;
        transform.position = newParent.GetItemTargetTransform().position;
        transform.parent = newParent.GetItemTargetTransform();

        SetHeldState(true);
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
        if (caller is PlayerInteract) {

            if (_heldState == false) {
                ChangeParent((caller as PlayerInteract).GetItemHolder());
            }
        }
    }
}
