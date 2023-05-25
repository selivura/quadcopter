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

    public static readonly float NoRestartMultiplier = 1.2f;
    public static readonly float[] RankPeaks = { 2000, 3000, 4000, 5000, 6000, 7000 };
    public static readonly string[] Ranks =    { "F",  "D",  "C",  "B",  "A",  "S" };
    [SerializeField] Color[] _RankColors;
    public void SetStats(PlayerStatistics stats)
    {
        _winText.text = $"PLAYER {stats.PlayerID + 1}";
        _score.text = $"SCORE: {stats.Score}";
        _restarts.text = $"RESTARTS: {stats.Restarts}";
        _bonuses.text = "";
        var final = stats.Score;
        if (stats.Restarts < 1)
        {
            _bonuses.text += $"No restarts: x{NoRestartMultiplier}\n";
            final = Mathf.RoundToInt(final * NoRestartMultiplier);
        }
        _finalScore.text = final.ToString();

        SetRank(CalculateScore(final));
    }
    public static int CalculateScore(int score)
    {
        for (int i = 0; i < RankPeaks.Length; i++)
        {
            if (score <= RankPeaks[i])
            {
                return i;
            }
        }
        return 5;
    }
    private void SetRank(int id)
    {
        _rank.color = _RankColors[id];
        _rank.text = Ranks[id];
    }
}

