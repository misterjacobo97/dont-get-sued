using System;
using DG.Tweening;
using UnityEngine;

public interface I_Interactable {
    public void SetSelected();
    public void SetUnselected();
    public void Interact(UnityEngine.Object caller);
}

public class PlayerInteract : MonoBehaviour {

    [Header("Game events")]
    [SerializeField] private GameEventSO<object> _onPlayerInteractSelectedChangedEvent;

    [SerializeField] private GameEventSO _onPlayerInteractEvent;

    [Header("refs")]
    [SerializeField] private InteractContextSO _playerInteractContext;

    public static PlayerInteract Instance { get; private set; }

    //public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
    //public class OnSelectedInteractableChangedEventArgs : EventArgs {
    //    public I_Interactable selectedInteractable;
    //}

    [Header("params")]
    [SerializeField] private LayerMask _interactMask;

    [Header("Interact Refs")]
    [SerializeField] private Transform _itemHolderRef;
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

    private void Start() {
        InputManager.Instance.PlayerInteractPressedEvent.AddListener(OnInteractInput);

        InputManager.Instance.PlayerInteractHeldReleasedEvent.AddListener(OnInteractHeldReleasedInput);
    }

    void Update() {
        if (GameManager.Instance.GetGameState != GameManager.GAME_STATE.MAIN_GAME) {
            return;
        }

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

        if (hit.collider != null && hit.transform.TryGetComponent(out I_Interactable item) && hit.transform != _playerInteractContext.selectedInteractableObject.Value) {
            _playerInteractContext.selectedInteractableObject.Value = hit.transform;

            ControlIndicator(_indicator.transform.InverseTransformPoint(_playerInteractContext.selectedInteractableObject.Value.position));
        }
        else if (hit.collider == null && _playerInteractContext.selectedInteractableObject != null) {
            _playerInteractContext.selectedInteractableObject.Value = null;
        }
    }

    private void ControlIndicator(Vector2 newPos) {
        if (_indicatorTween != null && _indicatorTween.active) {
            _indicatorTween.Kill();
        }

        _indicatorTween = _indicator.DOLocalMove(newPos, 0.1f);
    }

    private void OnInteractInput() {
        _playerInteractContext.selectedInteractableObject.Value?.GetComponent<I_Interactable>().Interact(this);
        
        if (_playerInteractContext.selectedInteractableObject.Value == null) {
            // drop item
            GetItemHolder().GetHeldItem()?.DropItem();
        }
    }

    private void OnInteractHeldReleasedInput() {
        _itemHolderRef.GetComponent<PlayerItemHolder>().ThrowItem(_lastMovement);
    }

    public bool HasItemHolder() {
        return _itemHolderRef != null;
    }

    public I_ItemHolder GetItemHolder() {
        if (_itemHolderRef == null) return null;

        return _itemHolderRef.GetComponent<I_ItemHolder>();
    }
}
