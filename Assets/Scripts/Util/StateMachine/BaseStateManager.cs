using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using R3;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseStateManager<EState> : MonoBehaviour where EState : Enum {

    [NonSerialized] public UnityEvent<BaseState<EState>> StateChangedEvent = new();

    [SerializeField] protected List<BaseState<EState>> States = new();
    [SerializeField] protected BaseState<EState> _currentState;
    public BaseState<EState> CurrentState => _currentState;


    [SerializeField] protected BaseStateSO _initialStateSO;
    public SerializedDictionary<BaseStateSO, BaseState<EState>> _listOfStates;
    public ReactiveProperty<BaseStateSO> currentState { get; private set; }

    [SerializeField] protected EState InitialState;
    protected ScriptableObject _context;

    public void Init(ScriptableObject context) {
        _context = context;
    }

    protected virtual void Awake() {
        currentState = new ReactiveProperty<BaseStateSO>(_initialStateSO);
        _listOfStates = new();
    }

    protected virtual void Start(){
        //foreach (BaseState state in States) {
        //    state.Init(_context);
        //}
        // gotta do this in the derived state machine

        foreach (BaseState<EState> state in GetComponentsInChildren<BaseState<EState>>()) {
            _listOfStates.Add(state.stateRef, state);
        }

        //_currentState = States.Find(s => s.StateKey.Equals(InitialState));

        //_currentState.EnterState();
    }

    protected virtual void Update(){
        EState nextStateKey = _currentState.GetNextState();

        if (nextStateKey.Equals(_currentState.StateKey)){
            _currentState.UpdateState();
        } else {
            TransitionToState(nextStateKey);
        }
    }

    void FixedUpdate(){
        _currentState.FixedUpdateState();
    }

    void OnTriggerEnter(Collider collider){
        _currentState.OnTriggerEnter(collider);
    }
    void OnTriggerStay(Collider collider){
        _currentState.OnTriggerStay(collider);
    }
    void OnTriggerExit(Collider collider){
        _currentState.OnTriggerExit(collider);
    }

    protected void TransitionToState(EState stateKey){
        _currentState.ExitState();
        _currentState = States.Find(s => s.StateKey.Equals(stateKey));
        _currentState.EnterState();
        StateChangedEvent.Invoke(CurrentState);
    }

    protected virtual void OnDestroy() {
        Disposable.Dispose(currentState);
    }

}
