using TMPro;
using UnityEngine;

public class GameManager : PersistentSignleton<GameManager>{

    [Header("UI Refs")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _totalScore = 0;

    /// <summary>
    /// This can be both positive or negative scores
    /// </summary>
    /// <param name="amount"></param>
    public void AddToScore(int amount) {
        _totalScore += amount;

        UpdateUI();
    }

    private void UpdateUI() {
        _scoreText.text = _totalScore.ToString();
    }

}
