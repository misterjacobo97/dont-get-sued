using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] private Button _startGameButtom;
    [SerializeField] private Button _quitGameButtom;

    private void Start() {
        //GameManager.Instance.ChangeGameState(GameManager.GAME_STATE.START_SCREEN);

        _startGameButtom.onClick.AddListener(GameManager.Instance.GameStartActions);
        _quitGameButtom.onClick.AddListener(GameManager.Instance.QuitGame);
    }
}
