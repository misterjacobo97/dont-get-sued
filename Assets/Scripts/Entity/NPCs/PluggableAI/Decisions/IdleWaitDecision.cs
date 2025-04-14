using System.Collections.Generic;
using PluggableAI;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="PluggableAI/Decisions/WaitDecision")]
public class WaitDecision : PluggableDecision {
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    private List<StateController> _awaiters = new();

    private void OnEnable() {
        SceneManager.activeSceneChanged += (a, b) => { _awaiters.Clear(); };
    }

    public override void Decide(StateController controller) {

        if (_awaiters.Contains(controller)) return;

        IdleWait(controller as NPCStateController);
    }

    private void IdleWait(NPCStateController controller) {
        if (_awaiters.Contains(controller)) return;

        _awaiters.Add(controller);
        Awaitable.WaitForSecondsAsync(Random.Range(minWaitTime, maxWaitTime)).GetAwaiter().OnCompleted(() => { 
            if (controller != null) {
                OnDecisionMade.Invoke(controller);
            }
            _awaiters.Remove(controller);
        });


    }


}
