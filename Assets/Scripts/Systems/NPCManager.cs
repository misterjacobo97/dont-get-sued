using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : PersistentSignleton<NPCManager> {

    [SerializeField] private List<Transform> _shopperPrefabs = new();

    [Header("npcs")]
    [SerializeField] private NPCDatabase npcDatabase;
    [SerializeField] private float _npcSpawnInterval = 3;
    [SerializeField] private int _maxNpcCount = 4;
    private bool _npcSpawningActive = true;


    [Header("debug")]
    [SerializeField] private Logger _logger;
    [SerializeField] private bool _showDebugLogs = true;

    private void Start() {
        GameManager.Instance.GameStateChanged.AddListener(state => {

            switch (state) {
                case GameManager.GAME_STATE.MAIN_GAME:
                    SpawnNewNPC();
                    return;
                default:
                    //ResetManager();
                    return;
            }
        });
    }

    private async void SpawnNewNPC() {
        await Awaitable.WaitForSecondsAsync(_npcSpawnInterval);

        if (npcDatabase.ActiveNPCList.GetList().Count >= _maxNpcCount) {
            _npcSpawningActive = false;
        }
        else _npcSpawningActive = true;

        if (_npcSpawningActive && GameManager.Instance.GetGameState.CurrentValue == GameManager.GAME_STATE.MAIN_GAME ) {
            // spawn and relocate
            NPCStateController newNPC = Transform.Instantiate(_shopperPrefabs[UnityEngine.Random.Range(0, _shopperPrefabs.Count - 1)]).GetComponent<NPCStateController>();

            //pick exit to enter from
            newNPC.transform.position = npcDatabase.GetRandomExit().position;

        }

        SpawnNewNPC();
    }



}
