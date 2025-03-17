using UnityEngine;
using UnityEngine.AI;
public class NPCController : MonoBehaviour {
    [Header("refs")]
    [SerializeField] private CustomerStateMachine _stateMachine;
    private CustomerStateContext_SO _stateContext;
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Sprites")]
    [SerializeField] private Sprite _sideSprite;
    [SerializeField] private Sprite _backSprite;

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
            agent.isStopped = false;
            agent.SetDestination(_stateContext.target.position);
        }
        else if (_stateContext.target == null) {

            agent.isStopped = true;
        }

        ControlSprite(agent.velocity.normalized);
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

}
