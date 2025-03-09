using UnityEngine;

[CreateAssetMenu(fileName = "TaskObject", menuName ="Task/TaskObject")]
public class TaskObject_SO : ScriptableObject {
    public Transform prefab;
    public Sprite iconSprite;
    public string taskName;
}
