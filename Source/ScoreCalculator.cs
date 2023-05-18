using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] TMP_Text _score;
    [SerializeField] TMP_Text _finalScore;
    [SerializeField] TMP_Text _restarts;
    [SerializeField] TMP_Text _bonuses;
    [SerializeField] TMP_Text _rank;
    [SerializeField] TMP_Text _winText;

    static readonly float _noRestartMultiplier = 1.2f;
    static readonly float[] _RankPeaks = { 3000, 4000, 5000, 6000, 7000 };
    static readonly string[] _Ranks = { "F", "D", "C", "B", "A", "S" };
    public void SetStats(PlayerStatistics stats)
    {
        _winText.text = $"Игрок {stats.PlayerID}";
        _score.text = $"Счет: {stats.Score}";
        _restarts.text = $"Перезапуски: {stats.Restarts}";

        var final = stats.Score;
        if (stats.Restarts < 1)
        {
            _bonuses.text += $"Без перезапусков: x{_noRestartMultiplier}\n";
            final = Mathf.RoundToInt(final * _noRestartMultiplier);
        }
        _finalScore.text = "Финальный счет: " + final.ToString();
        for (int i = 0; i < _RankPeaks.Length; i++)
        {
            if(final < _RankPeaks[i])
            {
                _rank.text = "Ранг: " + _Ranks[i - 1];
                return;
            }   
        }
    }
}

