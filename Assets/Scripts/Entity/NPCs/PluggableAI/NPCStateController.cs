using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : PluggableAI.StateController {

    private Awaitable _waitTimer;

    [Header("NPC refs")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody2D rb;
    public Transform nextDestinationTarget;
    [HideInInspector] public Transform agentFollowTarget;
    [HideInInspector] public NPCItemHolder itemHolder;
    public Collider2D exitCollider;

    public NPCDatabase npcDatabase;

    [Header("list references")]    
    public TransformListReference _listOfActiveNPCs;
    [SerializeField] private int shoppingListSize = 3;
    public List<ShoppingItem> shoppingList = new();
    public ScriptableObjectListReference acceptedShoppingItems;

    [Header("NPC params")]
    public SerializableReactiveProperty<bool> isSlapped = new(false);
    public Sprite slappedSprite;

    protected override void Awake() {
        base.Awake();
        itemHolder = GetComponentInChildren<NPCItemHolder>();
        

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected void Start() {
        for (int i = 0; i < shoppingListSize; i++) {
            shoppingList.Add(new ShoppingItem());

            
            shoppingList[i].item = acceptedShoppingItems.GetRandomFromList() as HoldableItem_SO;
        }

        _listOfActiveNPCs.AddToList(this.transform);

        InitialiseAI();
    }

    public void WaitBeforeNextAction(float waitTime, Action callback) {
        if (_waitTimer != null && !_waitTimer.IsCompleted) return;

        _waitTimer = Awaitable.WaitForSecondsAsync(waitTime);

        _waitTimer.GetAwaiter().OnCompleted(() => { callback(); });
    }

    public void GetSlapped(Vector2 direction) {
        itemHolder.DropAllItems();

        AddSlappedForce(direction);

        foreach (ShoppingItem i in shoppingList) {
            i.collected = false;
        }
    }

    private void AddSlappedForce(Vector2 dir) {
        rb.AddForce(dir * 900);
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        _waitTimer?.Cancel();
        isSlapped.Dispose();
    }



}
