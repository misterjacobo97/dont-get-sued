using System;
using DG.Tweening;
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
    [SerializeField] private Transform _indicator;

    private  Vector2 _lastMovement = Vector2.zero;
    private Tween _indicatorTween;

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


        if (_movement != Vector2.zero && InputManager.Instance.PlayerInteractIsHeld == false) {
            _lastMovement = _movement;
        }

        Vector2 RayPos = transform.TransformPoint(_lastMovement);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lastMovement, 1f, _interactMask);
        Debug.DrawLine(transform.position, RayPos, Color.white);


        if (_movement != Vector2.zero && hit.collider == null) {
            ControlIndicator(_movement);
        }

        if (hit.collider != null) {

            I_Interactable _currentInteractable = hit.transform.GetComponent<I_Interactable>();

            SetSelectedInteractable(_currentInteractable);

            ControlIndicator(_indicator.transform.InverseTransformPoint(hit.collider.transform.position));
        }
        else if (hit.collider == null && _selectedInteractable != null) {
            SetSelectedInteractable(null);
        }
    }

    private void ControlIndicator(Vector2 newPos) {
        if (_indicatorTween != null && _indicatorTween.active) {
            _indicatorTween.Kill();
        }

        _indicatorTween = _indicator.DOLocalMove(newPos, 0.1f);
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
