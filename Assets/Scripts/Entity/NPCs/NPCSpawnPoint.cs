using UnityEngine;

public class NPCSpawnPoint : MonoBehaviour {
    private void Start() {
        NPCManager.Instance.AddSpawnpoint(this);
    }
}

