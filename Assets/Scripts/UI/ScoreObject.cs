using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreObject : MonoBehaviour {
    public TextMeshProUGUI _text;
    private Sequence _sequence;

    public void Init(int scoreText, Vector2 position) {
        _text.color = scoreText >= 0 ? Color.blue : Color.red;
        _text.text = scoreText.ToString();
        transform.position = position;

        Animate();
    }

    private void Animate(){
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOLocalMoveY(2, 2).SetRelative(true))
            .Insert(0, transform.DOLocalRotate(new (0,0,45), 2).SetRelative(true))
            .Insert(0, _text.transform.DOScale(.05f, 2).SetRelative(true))
            .Insert(0, _text.DOFade(0, 2))
            .OnComplete(() => {
                Destroy(this.gameObject);
            });

    }

    private void OnDestroy() {
        _sequence.Kill();
    }
}
