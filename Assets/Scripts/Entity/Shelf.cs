using UnityEngine;

public class BaseShelf : MonoBehaviour, Interactable {

    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;

    public void SetSelected() {
        _sprite.color = Color.white;
    }

    public void SetUnselected() { 
        _sprite.color = Color.red;
    }

    public void Interact() { }
}
