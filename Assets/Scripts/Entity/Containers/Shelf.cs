using Tasks;
using UnityEngine;

public class BaseShelf : MonoBehaviour, I_ItemHolder, I_Interactable {

    enum ShelfType {
        SINGLE_TASK,
        MULTI_TASK
    }

    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemTarget;

    [Header("Params")]
    [SerializeField] private ShelfType _shelfType = ShelfType.SINGLE_TASK;

    private HoldableItem _heldItem = null;

    protected  void Start() {
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;

        TaskManager.Instance.AddTaskReceiver(this);
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
        Debug.Log("Interacted with");

        if (caller is PlayerInteract) {
            if (_heldItem != null) {
                _heldItem.ChangeParent((caller as PlayerInteract).GetItemHolder());
            }
        }
    }

    public void SetItem(HoldableItem newItem) {
        Debug.Log("holding new item");

        _heldItem = newItem;
        _heldItem.transform.position = _itemTarget.position;

        if (_heldItem is TimedTask) {
            //(_heldItem as TimedTask).
        }
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

}
