using UnityEngine;

public class NPCExitArea : MonoBehaviour {

    private void Start() {
        NPCManager.Instance.exitArea = this.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision); 
    }
}
