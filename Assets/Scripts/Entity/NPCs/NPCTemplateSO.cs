using UnityEngine;

[CreateAssetMenu(menuName = "NPC/NPCTemplateSO")]
public class NPCTemplateSO : ScriptableObject {

    public Transform npcPrefab;

    [Header("sprites")]
    public Sprite npcSideSprite;
    public Sprite npcBackSprite;
    public Sprite npcSlappedSprite;
    public Sprite npcSlippedSprite;


}
