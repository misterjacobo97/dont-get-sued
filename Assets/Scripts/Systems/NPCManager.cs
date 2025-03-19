using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : PersistentSignleton<NPCManager> {

    [Serializable]
    public class NPCTargets {
        public NPCController npc;
        public Transform target;

        // constructor
        public NPCTargets(NPCController newNpc, Transform newTarget) {
            npc = newNpc;
            target = newTarget;
        }
    }

    [SerializeField] private List<Transform> _shopperPrefabs = new();
    [SerializeField] private List<HoldableItem_SO> _shoppableItems = new();

    [Header("shopping")]
    [SerializeField] private int _maxShoppingListSize = 3;
    [SerializeField] private List<NPCTargets> _activeNpcTargetList = new();
    public List<NPCTargets> GetActiveTargetList => _activeNpcTargetList;

    [Header("npcs")]
    [SerializeField] private float _npcSpawnInterval = 3;
    [SerializeField] private int _maxNpcCount = 4;
    private bool _npcSpawningActive = true;

    private List<NPCController> _activeNpcList = new();
    private List<NPCSpawnPoint> _spawnPoints = new();
    public Transform exitArea;


    private void Start() {
        SpawnNewNPC();
    }

    public void AddSpawnpoint(NPCSpawnPoint newSpot) {
        _spawnPoints.Add(newSpot);
    }

    public void AddNPC(NPCController newNPC) {
        _activeNpcList.Add(newNPC);
    }

    private async void SpawnNewNPC() {
        await Awaitable.WaitForSecondsAsync(_npcSpawnInterval);

        if (_activeNpcList.Count > _maxNpcCount) {
            _npcSpawningActive = false;
        }
        else _npcSpawningActive = true;


        if (_npcSpawningActive) {
            // spawn and relocate
            NPCController newNPC = Transform.Instantiate(_shopperPrefabs[UnityEngine.Random.Range(0, _shopperPrefabs.Count - 1)]).GetComponent<NPCController>();
            newNPC.transform.position = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)].transform.position;

            // add shopping list to NPC
            List<ShoppingItem> newItems = GenerateNewShoppingList();
            AssignShoppingListToNPC(newNPC, newItems);
        }

        //await Awaitable.WaitForSecondsAsync(_npcSpawnInterval);

        SpawnNewNPC();
    }

    private List<ShoppingItem> GenerateNewShoppingList() {

        List<ShoppingItem> list = new();

        for (int i = 0; i < _maxShoppingListSize; i++) {
            ShoppingItem item = new();

            item.id = list.Count;
            item.collected = false;
            item.item = _shoppableItems[UnityEngine.Random.Range(0, _shoppableItems.Count - 1)];

            list.Add(item);
        }

        return list;
    }

    private void AssignShoppingListToNPC(NPCController npc, List<ShoppingItem> newList) {
        npc.AddToShoppingList(newList);
    }

    public void AddActiveTarget(NPCController npc, Transform target) {
        _activeNpcTargetList.Add(new NPCTargets(npc, target));
    }

    public void RemoveActiveTarget(NPCController npc) {
        _activeNpcTargetList.Remove(_activeNpcTargetList.Find(t => t.npc == npc));
    }

}
