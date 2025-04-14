using AYellowpaper.SerializedCollections;
using R3;
using UnityEngine;

namespace PluggableAI {
    /// <summary>
    /// To be set up for your specific ai scenario
    /// </summary>
    public abstract class StateController : MonoBehaviour {
        [Header("state controller")]
        public SerializableReactiveProperty<BaseStateSO> currentState;
        [SerializeField] protected BaseStateSO _initialState;


        private SerializedDictionary<BaseStateSO, PluggableState> _stateDict = new();

        protected bool _aiActive = false;

        protected virtual void Awake() {
            currentState = new SerializableReactiveProperty<BaseStateSO>().AddTo(this);


            foreach (PluggableState state in GetComponentsInChildren<PluggableState>()) {
                currentState.Value = _initialState;

                state.Init(this);
                
                _stateDict.Add(state.stateKey, state);

                state.ChangeBaseState += state => { currentState.Value = state; };
            }
        }

        protected virtual void Update() {
            if (!_aiActive) return;

            if (_stateDict.ContainsKey(currentState.Value)) {
                _stateDict[currentState.Value].UpdateState(this);
            }
        }

        protected virtual void OnDrawGizmos() {
            if (UnityEngine.Application.isPlaying == false || _aiActive == false) return;

            if (_stateDict.ContainsKey(currentState.Value)) {
                Gizmos.color = _stateDict[currentState.Value].sceneGizmosColor;
            }
        }

        protected void InitialiseAI() {

            _aiActive = true;
        }

  
    }
}