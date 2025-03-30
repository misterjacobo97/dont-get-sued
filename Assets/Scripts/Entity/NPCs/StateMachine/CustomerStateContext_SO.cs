using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// An object to pass refs and context data between customer NPC states
/// </summary>
public class CustomerStateContext_SO : ScriptableObject {
    // refs
    public CustomerStateMachine stateMachine;
    public NavMeshAgent agent;
    public Rigidbody2D rb;
    public NPCItemHolder itemHolder;
    public SpriteRenderer spriteRenderer;


    // properties
    public float timeOfLastStateChange;
    public CustomerStateMachine.STATES lastState;
    public Vector2 lastMovementDir = Vector2.zero;

    // tasks related
    public List<ShoppingItem> shoppingList = new();
    public TaskInfo currentTask;
    public Transform currentTarget;

}