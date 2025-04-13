using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimationController : MonoBehaviour {

    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite _sideSprite;

    private SpriteRenderer _sprite;
    private NavMeshAgent _agent;

    private bool _walkingAnimActive = false;
    private bool _isSlapped = false;
    [SerializeField] private Sprite _slappedSprite;

    private void Awake() {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        ControlAnimations();
        ControlSprite(_agent.velocity.normalized);
    }

    private void ControlSprite(Vector2 dir) {
        if (_isSlapped) _sprite.sprite = _slappedSprite;
        else if (dir.y > 0) _sprite.sprite = _backSprite;
        else if (dir.y < 0) _sprite.sprite = _sideSprite;

        if (dir.x < 0 && _sprite.flipX == false) {
            _sprite.flipX = true;
        }

        else if (dir.x > 0 && _sprite.flipX == true) {
            _sprite.flipX = false;
        }
    }

    private void ControlAnimations() {
        if (_walkingAnimActive == true || _agent.isStopped) return;

        _walkingAnimActive = true;

        _sprite.transform.DOLocalMoveY(0.15f, 0.1f).SetEase(Ease.OutCirc).SetLink(this.gameObject).onComplete += () => {
            _sprite.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InCirc).SetLink(this.gameObject).onComplete += () => {
                _walkingAnimActive = false;
            };
        };
    }
}
