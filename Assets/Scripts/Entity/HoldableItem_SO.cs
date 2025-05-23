using UnityEngine;

[CreateAssetMenu(fileName = "HoldableItem", menuName ="Holdables/Item")]
public class HoldableItem_SO : ScriptableObject {
    public Transform possiblePrefabs;
    public Sprite iconSprite;
    public string taskName;
    public bool pickableItem;
    public bool spoilable;
    public InteractContextSO playerInteractContext;
}
