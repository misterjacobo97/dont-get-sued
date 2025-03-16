using Tasks;
using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour {
    [Header("refs")]
    [SerializeField] private CustomerStateMachine _stateMachine;
    private CustomerStateContext_SO _stateContext;

    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // fill in context refs
        _stateContext = ScriptableObject.CreateInstance<CustomerStateContext_SO>();
        _stateContext.agent = agent;
        _stateContext.itemHolder = GetComponentInChildren<ItemHolder>();

        // the initialise state machine
        _stateMachine.Init(_stateContext);
    }

    private void Start() {
        TaskManager.Instance.taskListsChanged.AddListener(() => {
            _stateContext.possibleTasks = TaskManager.Instance.GetActiveTaskOfType(_stateContext.itemHolder.GetAcceptedItems());
        });
    }

    private void Update() {
        if (_stateContext.target != null && agent.isStopped == true) {
            Debug.Log("here");
            agent.isStopped = false;
            agent.SetDestination(_stateContext.target.position);
        }
        else if (_stateContext.target == null) {
            Debug.Log("there");

            agent.isStopped = true;
        }
    }


}
