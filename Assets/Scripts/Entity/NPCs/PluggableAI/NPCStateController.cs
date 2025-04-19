using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateController : PluggableAI.StateController {

    [Header("NPC refs")]
    public NPCTemplateSO npcTemplateSO;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Rigidbody2D rb;
    public Transform nextDestinationTarget;
    [HideInInspector] public Transform agentFollowTarget;
    [HideInInspector] public NPCItemHolder itemHolder;
    public Collider2D exitCollider;
    [SerializeField] private NPCSlapDetectionArea _slapDetect;
    [SerializeField] private List<Transform> _hazardsEncountered = new();


    [Header("context")]
    public NPCDatabase npcDatabase;
    public TransformListReference _listOfActiveNPCs;
    [SerializeField] private int shoppingListSize = 3;
    public List<ShoppingItem> shoppingList = new();
    public ScriptableObjectListReference acceptedShoppingItems;
    [SerializeField] private FloatReference _customerSatisfactionScore;
    // [SerializeField] private GameStateEventChannel _gameEventChannel;


    protected override void Awake() {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        itemHolder = GetComponentInChildren<NPCItemHolder>();

        GetComponentInChildren<NPCExitAreaDectector>().unaliveNPC.AddListener(UnaliveNPC);

        // _gameEventChannel.gameFinished.RegisterListener((_) => {UnaliveNPC();});

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

    public void Slip(Transform hazard, Vector2 direction, float stunTime, bool penalise = false){
        if (_hazardsEncountered.Contains(hazard)) return;

        if (penalise) {
            _customerSatisfactionScore.variable.reactiveValue.Value -= 10;
        }

        _hazardsEncountered.Add(hazard);
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

    public void UnaliveNPC(){
            itemHolder.CompleteItems();

            npcDatabase.UnassignTarget(this);
            npcDatabase.ActiveNPCList.GetList().Remove(transform);

            Destroy(gameObject);
    }

}
