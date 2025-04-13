using System;
using PluggableAI;
using R3;
using UnityEngine;

public abstract class PluggableDecision : ScriptableObject {
    public Action<StateController> OnDecisionMade;

    public abstract void Decide(StateController controller);
}
