using System.Collections.Generic;
using UnityEngine;

public class NPCItemHolder : MonoBehaviour, I_ItemHolder {

    [Header("Refs")]
    [SerializeField] private List<Transform> _itemTargets = new();
    [SerializeField] private ScriptableObjectListReference _acceptedItems;

    [SerializeField] private AudioClip _pickUpSound;


    private List<HoldableItem> _heldItems = new();
    public List<HoldableItem> HeldItems => _heldItems;

    [Header("context")]
    [SerializeField] private FloatReference _customerSatisfaction;
    [SerializeField] private FloatReference _scoreRef;


    private void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();
    }

    public void CompleteItems() {
        _heldItems.ForEach(i => {
            if (i.TryGetComponent(out SpoiledFoodTask task)) {
                switch (task.IsSpoiled){
                    case true:
                        _customerSatisfaction.AddToReactiveValue(-5);
                        break;
                    case false:
                        _customerSatisfaction.AddToReactiveValue(5);
                        _scoreRef.AddToReactiveValue(5);
                        break;
                }

                task.CompleteTask();
            }
            else {
                _customerSatisfaction.AddToReactiveValue(5);
                _scoreRef.AddToReactiveValue(5);
            }
        });
    }

    public void SetItem(HoldableItem newItem) {
        Debug.Log("holding new item");

        _heldItems.Add(newItem);
        SoundManager.Instance.PlaySound(_pickUpSound);
    }

    public bool HasItem() {
        if (_heldItems.Count == 0) {
            return false;
        }
        else return true;
    }

    public HoldableItem GetHeldItem() {
        if (_heldItems.Count == 0) {
            Debug.Log("no held item");
            return null;
        }

        return _heldItems[0];
    }

    public Transform GetItemTargetTransform() {
        if (_itemTargets == null) {
            Debug.Log("no item taget ref at: " + this);
            return null;
        }

        return _itemTargets[_heldItems.Count - 1];
    }

    public void RemoveItem() { }
    public void RemoveItem(int idx) {
        _heldItems.RemoveAt(idx);
    }

    public bool IsItemAccepted(HoldableItem_SO item) {
        return _acceptedItems.Contains(item);
    }

    public void DropAllItems() {
        for (int i = _heldItems.Count - 1; i >= 0; i--) {
            Vector2 dir = new Vector2(Random.Range(-1,1), Random.Range(-1, 1));

            _heldItems[i].ThrowItem(dir, 900); 
            RemoveItem(i);
        }
    }
}

