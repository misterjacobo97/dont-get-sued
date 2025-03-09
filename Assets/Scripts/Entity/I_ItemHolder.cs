using UnityEngine;

public interface I_ItemHolder {
    public void SetItem(Transform newItem);
    public bool HasItem();
    public Transform GetHeldItem();

    public Transform GetItemTargetTransform();
}
