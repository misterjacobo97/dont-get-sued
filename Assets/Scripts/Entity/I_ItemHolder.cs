using UnityEngine;

public interface I_ItemHolder {
    public void SetItem(HoldableItem newItem);
    public bool HasItem();
    public HoldableItem GetHeldItem();
    public Transform GetItemTargetTransform();
    public void RemoveItem();
    public bool IsItemAccepted(HoldableItem_SO item);
}
