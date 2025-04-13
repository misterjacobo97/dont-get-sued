using System;
using UnityEngine;
using UnityEngine.Events;

public class BinItemDetectArea : MonoBehaviour
{
    [NonSerialized] public UnityEvent<HoldableItem> ItemDetected = new();

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.parent.TryGetComponent<HoldableItem>(out HoldableItem item)) {
            //Debug.Log(item);

            ItemDetected.Invoke(item);
        }
    }
}
