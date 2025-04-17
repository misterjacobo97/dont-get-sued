using UnityEngine;

public class NPCChildSpawner : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] private Transform _childPrefab;
    [SerializeField] private NPCDatabase _npcDatabase;

    [Header("params")]
    [SerializeField] private float _spawnDistance = 1;
    [SerializeField] private float _chanceToSpawn = 0.3f;

    private void Start() {
        bool childSpawned = Random.Range(0f, 1f) > _chanceToSpawn;

        // if failed
        if (childSpawned == false) return;

        Transform child = Transform.Instantiate(_childPrefab);

        child.position = transform.position + (Vector3)(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * _spawnDistance);

        _npcDatabase.AssignNewFollower(child.GetComponent<NPCStateController>(), GetComponent<NPCStateController>());
    }

}
