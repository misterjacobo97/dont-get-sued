using UnityEngine;

public class NPCExitArea : MonoBehaviour {

    [Header("Refs")]
    [SerializeField] private TransformListReference _listOfLevelExits;

    private void Start() {
        //NPCManager.Instance.exitArea = this.transform;

        _listOfLevelExits.AddToList(this.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log(collision); 
    }
}
