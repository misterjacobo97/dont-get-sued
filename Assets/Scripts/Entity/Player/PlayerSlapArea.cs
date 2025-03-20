using UnityEngine;

public class PlayerSlapArea : MonoBehaviour {
    [SerializeField] private LayerMask _npcLayer;
    [SerializeField] private Collider2D _slapCollider;

    [SerializeField] private int _slapFrames = 3;
    private int _currentSlapFrames = 0;

    private void Update() {
        ControlSlapCollision();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        Debug.Log("slap");
        collision.GetComponentInParent<NPCController>().GetSlapped();
    }

    private void ControlSlapCollision() {
        // this makes it hit slaps more consistently
        if (_currentSlapFrames >= 0 && _currentSlapFrames < _slapFrames && _slapCollider.enabled == true) {
            _currentSlapFrames++;
            return;
        }
        
        if (_currentSlapFrames >= _slapFrames && _slapCollider.enabled == true) {
            _slapCollider.enabled = false;
            _currentSlapFrames = 0;
            return;
        }

        if (InputManager.Instance.PlayerSlapWasPressed && _currentSlapFrames == 0 & _slapCollider.enabled == false) {
            _slapCollider.enabled = true;
            
        }


    }
}
