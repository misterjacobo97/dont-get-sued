using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

public interface I_Interactable {
    public void SetSelected();
    public void SetUnselected();
    public void Interact(UnityEngine.Object caller);
}

public class PlayerInteract : MonoBehaviour {

    [Header("refs")]
    [SerializeField] private InteractContextSO _playerInteractContext;
    [SerializeField] private Image _throwIndicator;
    [SerializeField] private SoundClipReference _throwingSound;


    [Header("params")]
    [SerializeField] private LayerMask _interactMask;

    [SerializeField] private float _circleCastRadius = 0.2f;
    [SerializeField] private float _circleCastDistance = 0.5f;

    [SerializeField] private float _throwInputThreshold = 0.4f;
    [SerializeField] private float _throwForceMax = 900;
    [SerializeField] private float _throwForceBuildUpPerSec = 500;

    private bool _canDropItem = false;
    private float _timeSinceInteractInput = 0f;
    private float _currentThrowForce = 0;

    
    [Header("Interact Refs")]
    [SerializeField] private Transform _itemHolderRef;
    [SerializeField] private Transform _indicator;

    [Header("context")]
    [SerializeField] private UserInputChannelSO _userInputChannel;
    [SerializeField] private GameStatsSO _gameStatsDB;
    private Camera _mainCamera;

    private void Start() {
        _mainCamera = Camera.main;

        HandleInteractInput();

        _throwIndicator.enabled = false;
    }

    void Update() {
        if (_gameStatsDB.pauseStatus.GetReactiveValue.Value == true) return;

        if (GameManager.Instance.GetGameState.CurrentValue != (GameManager.GAME_STATE.MAIN_GAME) && GameManager.Instance.GetGameState.CurrentValue != (GameManager.GAME_STATE.TUTORIAL)) {
            return;
        }

        HandleItemDrop();

        Vector2 screenMousePos = InputManager.Instance.GetMousePosition() - (Vector2)_mainCamera.WorldToScreenPoint(transform.position);
        Vector2 relativeMouseDir = (screenMousePos - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + relativeMouseDir, _circleCastRadius, relativeMouseDir, _circleCastDistance, _interactMask);
        

        if (hit.collider == null) {
            ControlIndicator((Vector2)transform.position + relativeMouseDir);
        }

        if (_playerInteractContext.selectedInteractableObject.Value != null){
            ControlIndicator(_playerInteractContext.selectedInteractableObject.Value.position);
        }

        if (hit.collider != null && hit.transform.TryGetComponent(out I_Interactable item) && hit.transform != _playerInteractContext.selectedInteractableObject.Value) {
            _playerInteractContext.selectedInteractableObject.Value = hit.transform;

        }
        else if (hit.collider == null && _playerInteractContext.selectedInteractableObject != null) {
            _playerInteractContext.selectedInteractableObject.Value = null;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_indicator.position, _circleCastRadius);
    }

    private void ControlIndicator(Vector2 newPos) {
        _indicator.position = newPos;
    }

    private void HandleItemDrop(){
        PlayerItemHolder holder;

        // if not holding an item, do nothing
        if (!_itemHolderRef.TryGetComponent(out holder)) return;
        if (!holder.HasItem()) {
            _throwIndicator.fillAmount = 0;
            _throwIndicator.enabled = false;
            return;
        }

        bool interactHeld = _userInputChannel.InteractInput.GetReactiveValue.Value;
        
        // decide and add to timer if interact is held
        if (interactHeld && _canDropItem) {
            _timeSinceInteractInput += Time.deltaTime;

            if (_timeSinceInteractInput > _throwInputThreshold) {
                _throwIndicator.enabled = true;
                _currentThrowForce = Mathf.Clamp(_timeSinceInteractInput * _throwForceBuildUpPerSec, 0, _throwForceMax);

                _throwIndicator.fillAmount = _currentThrowForce / _throwForceMax;
            }
        }
    }

    private void HandleInteractInput() {
        _userInputChannel.InteractInput.GetReactiveValue.AsObservable().Subscribe(state => {
            // if pressed
            if (state == true) {
                // and if there is interactable
                if (_playerInteractContext.selectedInteractableObject.Value != null) {

                    _playerInteractContext.selectedInteractableObject.Value.GetComponent<I_Interactable>().Interact(this);
                    _canDropItem = false;
                }
            }

            // if released
            else if (state == false){
                if (!GetItemHolder().HasItem()) return;

                // make sure it doesnt drop on the initial grab
                if (_canDropItem == false) {
                    _canDropItem = true;
                }

                // if is not holding interact 
                else if (_timeSinceInteractInput < _throwInputThreshold) {
                    GetItemHolder().GetHeldItem()?.DropItem();
                    
                    _throwIndicator.fillAmount = 0;
                    _throwIndicator.enabled = false;
                    _timeSinceInteractInput = 0;

                    _canDropItem = false;
                    return;
                }

                else {
                    GetItemHolder().GetHeldItem()?.ThrowItem((_indicator.position - transform.position).normalized, Mathf.Clamp(_currentThrowForce, 0, _throwForceMax));
                    _throwingSound?.Play();

                    _throwIndicator.fillAmount = 0;
                    _throwIndicator.enabled = false;
                    _timeSinceInteractInput = 0;
                    
                    _canDropItem = false;
                }
            }
        }).AddTo(this);
    }

    public bool HasItemHolder() {
        return _itemHolderRef != null;
    }

    public I_ItemHolder GetItemHolder() {
        if (_itemHolderRef == null) return null;

        return _itemHolderRef.GetComponent<I_ItemHolder>();
    }
}
