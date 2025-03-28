using UnityEngine;

public class UIManager : PersistentSignleton<UIManager>
{
    [SerializeField] private GameObject _hudGroup;
    [SerializeField] private Transform _failScreenGroup;
    [SerializeField] private Transform _endScreenGroup;

    [Header("health")]
    [SerializeField] private Transform _healthNodeGroup;
    [SerializeField] private Transform _healthNode;

    private void Start() {
        _hudGroup.SetActive(false);
        _failScreenGroup.gameObject.SetActive(false);
        _endScreenGroup.gameObject.SetActive(false);

        GameManager.Instance.GameStateChanged.AddListener(state => { 
            switch (state) {
                case GameManager.GAME_STATE.MAIN_GAME:
                    _hudGroup.SetActive(true);
                    _failScreenGroup.gameObject.SetActive(false);
                    _endScreenGroup.gameObject.SetActive(false);
                    return;
                case GameManager.GAME_STATE.START_SCREEN | GameManager.GAME_STATE.PRE_GAME:
                    _hudGroup.SetActive(false);
                    _failScreenGroup.gameObject.SetActive(false);
                    _endScreenGroup.gameObject.SetActive(false);
                    return;
                case GameManager.GAME_STATE.SUED:
                    _failScreenGroup.gameObject.SetActive(true);
                    return;
                case GameManager.GAME_STATE.END_GAME:
                    _endScreenGroup.gameObject.SetActive(true);
                    return;
            }
        });
    }

    public void ChangeHealthUI(int newHealth) {
        if (newHealth > _healthNodeGroup.childCount) {
            // add as many nodes as needed
            for (int i = 0; i < newHealth - _healthNodeGroup.childCount; i++) {
                Transform.Instantiate(_healthNode).SetParent(_healthNodeGroup);
            }
        }

        else if (newHealth < _healthNodeGroup.childCount) {
            // remove as many nodes as needed
            for (int i = 0; i < _healthNodeGroup.childCount - newHealth; i++) {
                Destroy(_healthNodeGroup.GetChild(0).gameObject);
            }

           
        }
    }
}
