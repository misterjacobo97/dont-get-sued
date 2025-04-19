using R3;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour {
    [Header("context")]
    [SerializeField] private GameStatsSO _gameStats;


    [Header("refs")]
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _subtitle;


    [Header("params")]
    [SerializeField] private string[] _onWinTitles;
    [SerializeField] private string[] _onWinSubtitles;

    [SerializeField] private string[] _onCustomerLoseTitles;
    [SerializeField] private string[] _onCustomerLoseSubtitles;
    
    [SerializeField] private string[] _onManagementLoseTitles;
    [SerializeField] private string[] _onManagementLoseSubtitles;


    private void Start() {
        _title.color = Color.white;
        _title.text = _onWinTitles[Random.Range(0, _onWinTitles.Length - 1)];
        _subtitle.text = _onWinSubtitles[Random.Range(0, _onWinSubtitles.Length - 1)];


        _gameStats.suedStatus.GetReactiveValue.AsObservable().Subscribe(status => {
            if (status == true){
                _title.color = Color.red;

                if (_gameStats.customerSatisfaction.GetReactiveValue.Value <= 0){
                    _title.text = _onCustomerLoseTitles[Random.Range(0, _onCustomerLoseTitles.Length - 1)];
                    _subtitle.text = _onCustomerLoseSubtitles[Random.Range(0, _onCustomerLoseSubtitles.Length - 1)];

                }
                else if (_gameStats.managementSatisfaction.GetReactiveValue.Value <= 0){
                    _title.text = _onManagementLoseTitles[Random.Range(0, _onManagementLoseTitles.Length - 1)];
                    _subtitle.text = _onManagementLoseSubtitles[Random.Range(0, _onManagementLoseSubtitles.Length - 1)];
                }
            }
        }).AddTo(this);
    }
}
