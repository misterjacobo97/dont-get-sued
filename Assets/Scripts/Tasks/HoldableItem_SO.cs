using UnityEngine;

[CreateAssetMenu(fileName = "HoldableItem", menuName ="Holdables/Item")]
public class HoldableItem_SO : ScriptableObject {
    public Transform prefab;
    public Sprite iconSprite;
    public string taskName;
    public bool pickableItem;
}
