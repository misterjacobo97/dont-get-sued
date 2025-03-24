using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// An object to pass refs and context data between customer NPC states
/// </summary>
public class CustomerStateContext_SO : ScriptableObject {
    //signals
    //[NonSerialized] public UnityEvent<Vector2> SlappedTowardsDirection = new();

    // refs
    public CustomerStateMachine stateMachine;
    public NavMeshAgent agent;
    public I_ItemHolder itemHolder;
    public SpriteRenderer spriteRenderer;


    // properties
    public float timeOfLastStateChange;
    public CustomerStateMachine.STATES lastState;

    // tasks related
    public List<ShoppingItem> shoppingList = new();
    public TaskInfo currentTask;
    public Transform currentTarget;

}