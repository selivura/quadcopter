using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class IngameMenu : MonoBehaviour
{
    [SerializeField] GameObject _tutorialContainer;
    [SerializeField] GameObject _menuContainer;
    [SerializeField] GameObject _tablesContainer;
    [SerializeField] GameObject _winScreen;
    [SerializeField] ScoreCalculator _scoreTablePref;
     List<ScoreCalculator> _spawnedScoreTables = new List<ScoreCalculator>();
    [SerializeField] Button _winScreenButton;
    [SerializeField] Button _tutorialButton;
    [Tooltip("Их не будет видно без загруженого уровня")]
    [SerializeField] GameObject[] _ingameObjects;
    EventSystem _eventSystem;
    [SerializeField] PlayerGUI[] _GUIs;
    [SerializeField] GameObject _SelectPlayerLabel;

    public Action onLevelLoaded;
    private void Awake()
    {
        SetActiveIngameButtons(false);
        _SelectPlayerLabel.SetActive(false);
        _eventSystem = FindObjectOfType<EventSystem>();
        DespawnAllGUI();
    }
    private void Start()
    {
        MapLoader.LoadLevel(0);
    }
    public void SetActiveIngameButtons(bool val)
    {
        foreach (var obj in _ingameObjects)
        {
            obj.SetActive(val);
        }
    }
    public PlayerGUI SpawnGUIForPlayer(QuadcopterController copter)
    {
        _SelectPlayerLabel.SetActive(false);
        foreach (var gui in _GUIs)
        {
            if (gui.Copter == null)
            {
                gui.Copter = copter;
                gui.gameObject.SetActive(true);
                return gui;
            }
        }
        throw new System.Exception("No GUIS");
    }
    public void DespawnAllGUI()
    {
        foreach (var gui in _GUIs)
        {
            gui.gameObject.SetActive(false);
        }
    }
    public void ToggleGUI(int id)
    {
        for (int i = 0; i < _GUIs.Length; i++)
        {
            if (i == id)
                _GUIs[i].gameObject.SetActive(!_GUIs[i].isActiveAndEnabled);
        }
    }
    public void ToggleMenu()
    {
        SetActiveMenu(!_menuContainer.activeSelf);
    }
    public void SetActiveMenu(bool value)
    { 
        _menuContainer.SetActive(value);
        PauseController.Pause(value);
    }
    public void SetActiveTutorial(bool value)
    {
        _tutorialContainer.SetActive(value);
        _eventSystem.SetSelectedGameObject(_tutorialButton.gameObject);
    }
    public void CreateWinScreens(PlayerStatistics[] playersStats)
    {
        PauseController.Pause(true);
        DespawnAllGUI();
        _winScreen.SetActive(true);
        foreach (var stats in playersStats)
        {
            var spawned = Instantiate(_scoreTablePref, _tablesContainer.transform);
            spawned.SetStats(stats);
            _spawnedScoreTables.Add(spawned);
        }
        _eventSystem.SetSelectedGameObject(_winScreenButton.gameObject);
    }

    private void DespawnAllWinScreens()
    {
        _winScreen.SetActive(false);
        for (int i = 0; i < _spawnedScoreTables.Count; i++)
        {
            Destroy(_spawnedScoreTables[i].gameObject);
        }
        _spawnedScoreTables.Clear();
    }

    public void ReloadLevel()
    {
        LoadLevel(MapLoader.CurrentLevel - 1);
    }
    [SerializeField] GameObject _settingsGO;

    public void ToggleSettings()
    {
        _settingsGO.SetActive(!_settingsGO.activeSelf);
    }
    public void LoadLevel(int level)
    {
        FullReset();
        onLevelLoaded?.Invoke();
        MapLoader.LoadLevel(level);
    }

    public void FullReset()
    {
        DespawnAllWinScreens();
        DespawnAllGUI();
        SetActiveMenu(false);
        SetActiveIngameButtons(true);
        _SelectPlayerLabel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        MapLoader.LoadLevel(0);
        DespawnAllWinScreens();
        DespawnAllGUI();
        SetActiveMenu(true);
        SetActiveIngameButtons(false);
        PauseController.Pause(false);
        _SelectPlayerLabel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
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

