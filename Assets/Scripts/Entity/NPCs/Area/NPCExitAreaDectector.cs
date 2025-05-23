using System;
using UnityEngine;
using UnityEngine.Events;

public class NPCExitAreaDectector : MonoBehaviour {

    // [SerializeField] private NPCItemHolder _itemHolder;
    // [SerializeField] private NPCDatabase _npcDB;
    [SerializeField] private NPCStateController _parent;

    [NonSerialized] public UnityEvent unaliveNPC = new();

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.TryGetComponent<NPCExitArea>(out NPCExitArea area)) {
            // _itemHolder.CompleteItems();

            // _npcDB.UnassignTarget(_parent);
            // _npcDB.ActiveNPCList.GetList().Remove(_parent.transform);

            // Destroy(_parent.gameObject);
            unaliveNPC.Invoke();
        }
    }

    
}
