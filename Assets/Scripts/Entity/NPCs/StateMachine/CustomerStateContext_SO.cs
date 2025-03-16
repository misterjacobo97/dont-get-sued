using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An object to pass refs and context data between customer NPC states
/// </summary>
public class CustomerStateContext_SO : ScriptableObject {
    // refs
    public CustomerStateMachine stateMachine;
    public NavMeshAgent agent;
    public Transform target = null;
    public ItemHolder itemHolder;

    // properties
    public float timeOfLastStateChange;
    public CustomerStateMachine.STATES lastState;

    // tasks related
    public List<TaskObject> possibleTasks = new();
    public TaskObject currentTask;

}