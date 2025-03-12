using System;
using UnityEngine;

public interface I_Interactable {
    public void SetSelected();
    public void SetUnselected();
    public void Interact(object caller);
}

public class PlayerInteract : MonoBehaviour {

    public static PlayerInteract Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
    public class OnSelectedInteractableChangedEventArgs : EventArgs {
        public I_Interactable selectedInteractable;
    }

    [Header("params")]
    [SerializeField] private LayerMask _interactMask;

    [Header("Interact Refs")]
    [SerializeField] private Transform _itemHolderRef;
    private I_Interactable _selectedInteractable;

    private  Vector2 _lastMovement = Vector2.zero;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

    protected void Start() {
        InputManager.Instance.PlayerInteractPressedEvent.AddListener(OnInteractInput);
    }

    void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();

        if (_movement != Vector2.zero && InputManager.Instance.PlayerIntereactIsHeld == false) {
            _lastMovement = _movement;
        }

        Vector2 RayPos = transform.TransformPoint(_lastMovement);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lastMovement, 1f, _interactMask);
        Debug.DrawLine(transform.position, RayPos, Color.white);

        if (hit.collider != null) {

            I_Interactable _currentInteractable = hit.transform.GetComponent<I_Interactable>();

            SetSelectedInteractable(_currentInteractable);
        }
        else if (hit.collider == null && _selectedInteractable != null) {
            SetSelectedInteractable(null);
        }
    }

    private void OnInteractInput() {
        if (_selectedInteractable != null) {
            _selectedInteractable.Interact(this);
        }
        else if (GetItemHolder() != null && GetItemHolder().HasItem()) {
            // drop item
            GetItemHolder().GetHeldItem().DropItem();
        }
    }

    private void SetSelectedInteractable (I_Interactable newInteractable) {
        if (newInteractable == _selectedInteractable) {
            return;
        }

        this._selectedInteractable = newInteractable;

        Debug.Log("new interactable selected: " + newInteractable);

        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
            selectedInteractable = _selectedInteractable
        });
    }

    public bool HasItemHolder() {
        return _itemHolderRef != null;
    }

    public I_ItemHolder GetItemHolder() {
        if (_itemHolderRef == null) return null;

        return _itemHolderRef.GetComponent<I_ItemHolder>();
    }
}
