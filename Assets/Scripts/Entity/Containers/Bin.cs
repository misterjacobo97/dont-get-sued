using System.Collections.Generic;
using DG.Tweening;
using R3;
using Unity.Mathematics;
using UnityEngine;

public class Bin : MonoBehaviour, I_ItemHolder, I_Interactable {
    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemTarget;
    [SerializeField] private ScriptableObjectListReference _acceptedItems;
    [SerializeField] private AudioClip _trashSound;
    [SerializeField] private BinItemDetectArea _itemDetect;
    private I_Interactable interactableRef;

    [Header("context objects")]
    [SerializeField] private InteractContextSO _playerContext;

    private Sequence _interactTween;

    private HoldableItem _heldItem = null;

    private void Awake() {
        interactableRef = GetComponent<I_Interactable>();
    }

    private void Start() {
        //PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;

        _itemDetect.ItemDetected.AddListener(item => {
            item.ChangeParent(this);
        });

        _playerContext.selectedInteractableObject.AsObservable().Subscribe((item) => {
            if (item != null && item.GetComponent<I_Interactable>() == interactableRef) {
                Debug.Log("its me");
                SetSelected();
            }
            else SetUnselected();
        }).AddTo(this);
    }


    //private void OnSelectedInteractableChanged(object sender, PlayerInteract.OnSelectedInteractableChangedEventArgs e) {
    //    if (e.selectedInteractable == (I_Interactable)this) {
    //        SetSelected();
    //    }
    //    else {
    //        SetUnselected();
    //    }
    //}

    public void SetSelected() {
        _sprite.color = Color.gray;
    }

    public void SetUnselected() {
        _sprite.color = Color.white;
    }

    public void Interact(Object caller) {
        if (caller is PlayerInteract) {

            if ((caller as PlayerInteract).GetItemHolder().HasItem() && HasItem() == false) {
                Debug.Log("receiving item from: " + caller.ToString());

                // set holdable parent to this
                (caller as PlayerInteract).GetItemHolder().GetHeldItem().ChangeParent(this);
            }
        }
    }

    private void HandleAnimation() {
        if (_interactTween != null && _interactTween.active) {
            _interactTween.Kill();
        }

        _sprite.transform.rotation = quaternion.EulerXYZ(0,0,0);

        _interactTween = DOTween.Sequence();

        _interactTween.SetRelative(true);

        _interactTween.Append(_sprite.transform.DOScale(0.01f, 0.1f))
            .Insert(0, _sprite.transform.DORotate(new Vector3(0, 0, 20), 0.1f))
            .Append(_sprite.transform.DOScale(-0.01f, 0.1f))
            .Insert(0.1f, _sprite.transform.DORotate(new Vector3(0, 0, -40), 0.1f))
            .Append(_sprite.transform.DORotate(new Vector3(0, 0, 20), 0.05f));




    }

    public void SetItem(HoldableItem newItem) {
        _heldItem = newItem;

        // complete if the item is a task
        CompleteTaskItem();

        // then remove and destroy the item
        HoldableItem item = _heldItem;

        RemoveItem();

        HandleAnimation();
        SoundManager.Instance.PlaySound(_trashSound);

        Destroy(item.gameObject);
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

    private void CompleteTaskItem() {
        if (_heldItem is not TaskObject) return;

        if (_heldItem is SpoiledFoodTask) (_heldItem as SpoiledFoodTask).CompleteTask();

        else (_heldItem as TaskObject).CompleteTask();
    }


    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }

}
