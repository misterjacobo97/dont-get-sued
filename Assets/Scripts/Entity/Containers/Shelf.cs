using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



public class BaseShelf : MonoBehaviour, I_ItemHolder, I_Interactable {

    enum ShelfType {
        SINGLE_TASK,
        MULTI_TASK
    }

    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemTarget;
    [SerializeField] public List<HoldableItem_SO> _acceptedItems = new();
    [SerializeField] private Transform _customerMarker;


    [Header("Params")]
    [SerializeField] private ShelfType _shelfType = ShelfType.SINGLE_TASK;
    [SerializeField] private bool _showGizmos = true;
    
    // item spawn 
    [SerializeField] private float _itemRespawnTime = 10f;
    private float _timeOfLastItemDrop;

    private HoldableItem_SO _lastHeldItem = null;
    private HoldableItem _heldItem = null;

    protected  void Start() {
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;

        // if target gameobject has a child, check if its allowed and set it up as held item
        if (_itemTarget.GetChild(0) != null) {
            (_itemTarget.GetChild(0).GetComponent<HoldableItem>()).ChangeParent(this);
        }
    }

    protected void Update() {
        if (_heldItem == null && _lastHeldItem != null && Time.time > _timeOfLastItemDrop + _itemRespawnTime) {
            Transform.Instantiate<HoldableItem>(_lastHeldItem.possiblePrefabs.GetComponent<HoldableItem>()).ChangeParent(this);

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
            PlayerInteract player = caller as PlayerInteract;

            if (_heldItem != null) {
                Debug.Log("here");
                _heldItem.ChangeParent(player.GetItemHolder());
            }

            else if (_heldItem == null &&
                player.GetItemHolder().HasItem() &&
                IsItemAccepted(player.GetItemHolder().GetHeldItem().holdableItem_SO)
            ) {

                player.GetItemHolder().GetHeldItem().ChangeParent(this);
            }
        }
    }

    public void SetItem(HoldableItem newItem) {
        _heldItem = newItem;
        _lastHeldItem = _heldItem.holdableItem_SO;
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
        _timeOfLastItemDrop = Time.time;
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
