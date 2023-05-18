using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngameMenu : MonoBehaviour
{
    [SerializeField] GameObject _tutorialContainer;
    [SerializeField] GameObject _menuContainer;
    [SerializeField] GameObject _tablesContainer;
    [SerializeField] GameObject _winScreen;
    [SerializeField] ScoreCalculator _scoreTablePref;
    [SerializeField] List<ScoreCalculator> _spawned = new List<ScoreCalculator>();
    [SerializeField] Button _winScreenButton;
    public void ToggleMenu()
    {
        _menuContainer.SetActive(!_menuContainer.activeSelf);
        PauseController.Pause(_menuContainer.activeSelf);
        _tutorialContainer.SetActive(false);
    }
    public void OpenMainMenu()
    {
        PauseController.Pause(false);
        MapLoader.LoadMainMenu();
    }
    public void OpenTutorial()
    {
        PauseController.Pause(true);
        _tutorialContainer.SetActive(true);
    }
    public void OpenWinScreen(PlayerStatistics[] playersStats)
    {
        PauseController.Pause(true);
        _winScreen.SetActive(true);
        foreach (var stats in playersStats)
        {
            var spawned = Instantiate(_scoreTablePref, _tablesContainer.transform);
            spawned.SetStats(stats);
            _spawned.Add(spawned);
        }
        FindObjectOfType<EventSystem>().SetSelectedGameObject(_winScreenButton.gameObject);
    }
}

public class PlayerStatistics
{
    public readonly int Score, Restarts, PlayerID;
    public PlayerStatistics(int score, int restarts, int playerID)
    {
        Score = score;
        Restarts = restarts;
        PlayerID = playerID;
    }
}

