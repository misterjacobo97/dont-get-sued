using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : PluggableAI.StateController {

    [Header("NPC refs")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody2D rb;
    public Transform nextDestinationTarget;
    [HideInInspector] public Transform agentFollowTarget;
    [HideInInspector] public NPCItemHolder itemHolder;
    public Collider2D exitCollider;
    [SerializeField] private NPCSlapDetectionArea _slapDetect;
    public NPCDatabase npcDatabase;

    [Header("context")]
    public TransformListReference _listOfActiveNPCs;
    [SerializeField] private int shoppingListSize = 3;
    public List<ShoppingItem> shoppingList = new();
    public ScriptableObjectListReference acceptedShoppingItems;
    [SerializeField] private FloatReference _customerSatisfactionScore;

    


    protected override void Awake() {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
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

        _slapDetect.IsSlapped.AsObservable().Subscribe(slapped => {
            if (slapped == true) {
                GetSlapped(GetComponentInChildren<NPCSlapDetectionArea>().SlapDir.Value, 2);
            }
            else {
                currentState.Value = _initialState;
                _aiActive = true;
            }
        });

        InitialiseAI();
    }

    public void Slip(Vector2 direction, float stunTime, bool penalise = false){
        if (penalise) {
            _customerSatisfactionScore.variable.reactiveValue.Value -= 10;
        }

        _aiActive = false;

        agent.isStopped = true;
        itemHolder.DropAllItems();
        AddSlappedForce(direction);

        foreach (ShoppingItem i in shoppingList) {
            i.collected = false;
        }

        Awaitable.WaitForSecondsAsync(stunTime).GetAwaiter().OnCompleted(() => {
            currentState.Value = _initialState;
            _aiActive = true;
        });
    }

    public void GetSlapped(Vector2 direction, float stunTime) {
        _aiActive = false;

        agent.isStopped = true;
        itemHolder.DropAllItems();
        AddSlappedForce(direction);

        foreach (ShoppingItem i in shoppingList) {
            i.collected = false;
        }

    }

    private void AddSlappedForce(Vector2 dir) {
        rb.AddForce(dir * 900);
    }

}
