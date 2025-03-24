using UnityEngine;

public class UIManager : PersistentSignleton<UIManager>
{
    [SerializeField] private CanvasGroup _uiCanvas;

    private void Start() {
        _uiCanvas.alpha = 0;

        GameManager.Instance.GameStateChanged.AddListener(state => { 
            switch (state) {
                case GameManager.GAME_STATE.MAIN_GAME:
                    _uiCanvas.alpha = 1;
                    return;
                default:
                    _uiCanvas.alpha = 0;
                    return;
            }
        });
    }
}
