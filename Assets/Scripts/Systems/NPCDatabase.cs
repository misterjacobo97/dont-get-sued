using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Systems/NPCManagerSO")]
public class NPCDatabase : ScriptableObject {
    [SerializeField] private TransformListReference _listOfShelves;
    [SerializeField] private TransformListReference _listActiveOfNPCs;
    [SerializeField] private TransformListReference _listOfLevelExits;

    public TransformListReference ActiveNPCList => _listActiveOfNPCs;

    [SerializeField] private TransformDictionaryReference _DictOfFollowerTargets;
    [SerializeField] private TransformDictionaryReference _DictOfShelfTargets;

    #region Shelf Targets
    private BaseShelf FindFreeShelfWithItem(HoldableItem_SO item) {
        BaseShelf[] shelves = _listOfShelves.GetList()
            // convert to BaseShelf
            .Select<Transform, BaseShelf>(shelf => shelf.GetComponent<BaseShelf>())
            // filter any with assigned to an NPC and with a different / null item
            .Where(shelf => _DictOfShelfTargets.TryGetValue(shelf.transform) == null && shelf.HasItem() && shelf.GetHeldItem().holdableItem_SO == item).ToArray();

        // return a random value
        return shelves.Length > 0 ? shelves[UnityEngine.Random.Range(0, shelves.Length - 1)] : null;    
    }

    public void AssignNPCToShelf(NPCStateController npc) {
        // do nothing if already assigned
        if (_DictOfShelfTargets.KeyExists(npc.transform)) {
            return;
        }

        if (!npc.shoppingList.Any(i => i.collected == false)) return;

        // find free shelf
        BaseShelf shelf = FindFreeShelfWithItem(npc.shoppingList.First(i => i.collected == false).item);

        // assign to npc
        _DictOfShelfTargets.AddToDict(npc.transform, shelf.transform);
    }

    public void UnassignNPCToShelf(NPCStateController npc) {
        // do nothing if already assigned
        if (!_DictOfShelfTargets.KeyExists(npc.transform)) {
            return;
        }

        _DictOfShelfTargets.Remove(npc.transform);
    }

    public bool IsNPCAssignedToShelf(NPCStateController npc) { 
        if (_DictOfShelfTargets.KeyExists(npc.transform)) {
            return true;
        }

        return false;
    }

    public Transform TryGetNPCShelfTarget(NPCStateController npc) {

        if (_DictOfShelfTargets.KeyExists(npc.transform)) {
            return _DictOfShelfTargets.TryGetValue(npc.transform);
        }

        return default(Transform);
    }
    #endregion

    #region Exits
    public bool ExitExists(Transform exit) {
        if (_listOfLevelExits.Contains(exit)) {
            return true;
        }

        return false;
    }



    public Transform GetRandomExit() {
        return _listOfLevelExits.GetRandomFromList();
    }
    #endregion


    #region Followers

    public bool IsFollowerAssigned(NPCStateController follower) {
        return _DictOfFollowerTargets.KeyExists(follower.transform);
    }

    public void AssignNewFollower(NPCStateController follower, NPCStateController targetNPC) {

        if (_DictOfFollowerTargets.KeyExists(follower.transform)) return;

        _DictOfFollowerTargets.AddToDict(follower.transform, targetNPC.transform);
    }

    public Transform GetFollowerTarget(NPCStateController follower) {
        if (_DictOfFollowerTargets.KeyExists(follower.transform)) {
            return _DictOfFollowerTargets.TryGetValue(follower.transform);
        }

        return null;
    }

    public void UnassignTarget(NPCStateController follower) {
        if (!_DictOfFollowerTargets.KeyExists(follower.transform)) return;

        _DictOfFollowerTargets.Remove(follower.transform);
    }

    #endregion

}
