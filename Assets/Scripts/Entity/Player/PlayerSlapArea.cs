using R3;
using UnityEngine;

public class PlayerSlapArea : MonoBehaviour {
    [Header("refs")]
    [SerializeField] private LayerMask _npcLayer;
    [SerializeField] private Collider2D _slapCollider;
    [SerializeField] private SoundClipReference _slapSound;
    [SerializeField] private Animator _animator;


    [Header("params")]
    [SerializeField] private int _slapFrames = 3;
    private int _currentSlapFrames = 0;
    [SerializeField] private float _slapReach = 0.5f;
    [SerializeField] private float _npcStunTime = 2f;
    private bool _isSlapping = false;

    private void Awake() {

        Observable.EveryValueChanged(this, x => x._isSlapping).Subscribe(state => {
            if (state == true) {
                _animator.SetTrigger("slapTrigger");
            }
        }).AddTo(this);
    }

    private void Start() {
        InputManager.Instance.MovementInputEvent.AddListener(ChangeAreaRotation);
    }

    private void Update() {
        ControlSlapCollision();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("NPC")) return;
        
        _slapSound.Play();
        // find direction of npc
        Vector2 slapDir = (collision.transform.position - transform.position).normalized;

        collision.transform.parent.GetComponentInChildren<NPCStateController>().GetSlapped(slapDir, _npcStunTime);
        
    }

    public void ChangeAreaRotation(Vector2 moveDir) {

        _slapCollider.offset = _slapReach * moveDir / 2;

        if (moveDir == Vector2.right) {
            (_slapCollider as BoxCollider2D).size = new(_slapReach, 1);
        }

        else if (moveDir == Vector2.up) {
            (_slapCollider as BoxCollider2D).size = new(1, _slapReach);
        }

        else if (moveDir == Vector2.left) {
            (_slapCollider as BoxCollider2D).size = new(_slapReach, 1);
        }

        else if (moveDir == Vector2.down) {
            (_slapCollider as BoxCollider2D).size = new(1, _slapReach);
        }
    }

    private void ControlSlapCollision() {
        // this makes it hit slaps more consistently
        if (_currentSlapFrames >= 0 && _currentSlapFrames < _slapFrames && _slapCollider.enabled == true) {
            _currentSlapFrames++;
            return;
        }
        
        if (_currentSlapFrames >= _slapFrames && _slapCollider.enabled == true) {
            _isSlapping = false;
            _slapCollider.enabled = false;
            _currentSlapFrames = 0;
            return;
        }

        if (InputManager.Instance.PlayerSlapWasPressed && _currentSlapFrames == 0 & _slapCollider.enabled == false) {
            _isSlapping = true;
            _slapCollider.enabled = true;
            
        }
    }
}
