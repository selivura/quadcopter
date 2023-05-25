using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<QuadcopterController> SpawnedPlayers { get; private set; } = new List<QuadcopterController>();
    [SerializeField] private QuadcopterController _playerPref;
    private IngameMenu _UI;
    PlayerInputManager _manager;
    private void Awake()
    {
        _UI = FindObjectOfType<IngameMenu>();
        _manager = GetComponent<PlayerInputManager>();
        MapLoader.onLevelLoaded += DespawnAll;
        _UI.onLevelLoaded += () => _manager.enabled = true;
    }
    public void WinGame()
    {
        List<PlayerStatistics> stats = new List<PlayerStatistics>();
        int maxScore = -100;
        int score = 0;
        foreach (var player in SpawnedPlayers)
        {
            score = player.GetComponent<CopterScore>().Score;
            stats.Add(new PlayerStatistics(player.GetComponent<CopterScore>().Score, player.GetComponent<CopterScore>().RestartAmount, player.PlayerID));
            if (score > maxScore)
                maxScore = score;
        }
        SaveManager.SetLevelStat(MapLoader.CurrentLevel - 1, score);
        if (score >= ScoreCalculator.RankPeaks[2])
            SaveManager.UnlockLevel(MapLoader.CurrentLevel);
        FindObjectOfType<IngameMenu>().CreateWinScreens(stats.ToArray());

    }
    public void OnPlayerJoined(PlayerInput input)
    {
        var spawnedPlayer = input.GetComponent<QuadcopterController>();
        spawnedPlayer.PlayerID = SpawnedPlayers.Count;
        spawnedPlayer.GetComponent<DroneInputs>().onRequestPause += () => FindObjectOfType<IngameMenu>().ToggleMenu();

        SpawnedPlayers.Add(spawnedPlayer);
        _UI.SpawnGUIForPlayer(spawnedPlayer).Initialize(SpawnedPlayers.Count);
    }
    public void DespawnAll()
    {
        for (int i = 0; i < SpawnedPlayers.Count; i++)
        {
            Destroy(SpawnedPlayers[i].gameObject);
        }
        SpawnedPlayers.Clear();
    }
    private void OnDestroy()
    {
        MapLoader.onLevelLoaded -= DespawnAll;
        _UI.onLevelLoaded -= () => _manager.enabled = true;
    }
    public Transform GetSpawnPoint()
    {
        return transform;
    }
}
