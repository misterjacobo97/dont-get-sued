using UnityEngine;

interface Interactable {
    public void SetSelected();
    public void SetUnselected();
    public void Interact();

}

public class PlayerInteract : MonoBehaviour {

    [Header("params")]
    [SerializeField] private LayerMask _interactMask;

    private Interactable _selectedInteractable;
    Vector2 _lastMovement = Vector2.zero;

    // Update is called once per frame
    void Update() {
        Vector2 _movement = InputManager.Instance.GetPlayerMovement();

        if (_movement != Vector2.zero) {
            _lastMovement = _movement;
        }

        Vector2 RayPos = transform.TransformPoint(_lastMovement);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, _lastMovement, 1f, _interactMask);
        Debug.DrawLine(transform.position, RayPos, Color.white);

        if (hit.collider != null) {
            Debug.Log("here");

            Interactable _currentInteractable = hit.transform.GetComponent<Interactable>();

            if (_selectedInteractable != null && _currentInteractable != _selectedInteractable) {
                _selectedInteractable.SetUnselected();
            }

            _selectedInteractable = _currentInteractable;
            hit.transform.GetComponent<Interactable>().SetSelected();
        }
        else if (hit.collider == null && _selectedInteractable != null) {
            _selectedInteractable.SetUnselected();
            _selectedInteractable = null;
        }

    }
}
