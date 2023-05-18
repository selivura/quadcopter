using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<QuadcopterController> SpawnedPlayers { get; private set; } = new List<QuadcopterController>();
    [SerializeField] private QuadcopterController _playerPref;
    private UISetuper _uiSetuper;
    private void Awake()
    {
        _uiSetuper = FindObjectOfType<UISetuper>();
        //Instantiate(_playerPref, transform.position, Quaternion.identity);
    }
    public void WinGame()
    {
        List<PlayerStatistics> stats = new List<PlayerStatistics>();
        foreach (var player in SpawnedPlayers)
        {
            stats.Add(new PlayerStatistics(player.Score, player.RestartAmount, player.PlayerID));
        }
        FindObjectOfType<IngameMenu>().OpenWinScreen(stats.ToArray());
    }
    public void OnPlayerJoined(PlayerInput input)
    {
        var spawnedPlayer = input.GetComponent<QuadcopterController>();
        SpawnedPlayers.Add(spawnedPlayer);
        spawnedPlayer.PlayerID = SpawnedPlayers.Count;
        _uiSetuper.SpawnGUIForPlayer(spawnedPlayer).Initialize(SpawnedPlayers.Count);
        spawnedPlayer.Inputs.onRequestPause += () => FindObjectOfType<IngameMenu>().ToggleMenu();
    }
    public Transform GetSpawnPoint()
    {
        return transform;
    }
}
