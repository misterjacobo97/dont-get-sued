using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class ShoppingItem {
    public int id;
    public HoldableItem_SO item;
    public bool collected;
}

public class NPCController : MonoBehaviour {
    [Header("refs")]
    [SerializeField] private CustomerStateMachine _stateMachine;
    private CustomerStateContext_SO _stateContext;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform _itemHolder;


    [Header("Sprites")]
    [SerializeField] private Sprite _sideSprite;
    [SerializeField] private Sprite _backSprite;
    private bool _walkingAnimActive = false;

    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // fill in context refs
        _stateContext = ScriptableObject.CreateInstance<CustomerStateContext_SO>();
        _stateContext.agent = agent;
        _stateContext.itemHolder = GetItemHolder();
        _stateContext.shoppingList = new();

        // the initialise state machine
        _stateMachine.Init(_stateContext);
    }

    private void Start() {
        NPCManager.Instance.AddNPC(this);
    }

    private void Update() {
        if (_stateContext.currentTarget != null && agent.isStopped == true) {
            agent.isStopped = false;
            agent.SetDestination(_stateContext.currentTarget.position);
        }
        else if (_stateContext.currentTarget == null) {

            agent.isStopped = true;
        }

        ControlSprite(agent.velocity.normalized);
        ControlAnimations();
    }
    private void ControlSprite(Vector2 dir) {

        if (dir.y > 0) _sprite.sprite = _backSprite;
        else if (dir.y < 0) _sprite.sprite = _sideSprite;

        if (dir.x < 0 && _sprite.flipX == false) {
            _sprite.flipX = true;
        }

        else if (dir.x > 0 && _sprite.flipX == true) {
            _sprite.flipX = false;
        }
    }

    private void ControlAnimations() {
        if (_walkingAnimActive == true || agent.isStopped) return;

        _walkingAnimActive = true;

        _sprite.transform.DOLocalMoveY(0.15f, 0.1f).SetEase(Ease.OutCirc).SetLink(this.gameObject).onComplete += () => {
            _sprite.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InCirc).SetLink(this.gameObject).onComplete += () => {
                _walkingAnimActive = false;
            };
        };
    }

    #region Shopping
    public void AddToShoppingList(List<ShoppingItem> items) {
        _stateContext.shoppingList = items;
    }

    public void AddToShoppingList(ShoppingItem item) {
        _stateContext.shoppingList.Add(item);
    }

    public void GrabItem(HoldableItem item) {
        I_ItemHolder holder = _itemHolder.GetComponent<I_ItemHolder>();

        holder.SetItem(item);
    }

    public void GetSlapped() {
        (GetItemHolder() as NPCItemHolder).DropAllItems();

        foreach (ShoppingItem i in _stateContext.shoppingList.Where(item => item.collected == true)) {
            i.collected = false;
        }

        TaskManager.Instance.RemoveNPCFromAssignments(this);
    }
    #endregion


    public I_ItemHolder GetItemHolder() {
        if (_itemHolder == null) return null;

        return _itemHolder.GetComponent<I_ItemHolder>();
    }

    private void OnDestroy() {
        NPCManager.Instance.RemoveNPC(this);
    }
}
