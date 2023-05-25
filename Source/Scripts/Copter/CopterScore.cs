using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QuadcopterController))]
public class CopterScore : MonoBehaviour
{
    public int Score;
    public int RestartAmount { get; private set; }
    [HideInInspector] public Transform LastSpawnpoint;
    [HideInInspector] public Transform NextCheckpoint;
    private Map _map;
    private QuadcopterController _quadcopter;
    private void Awake()
    {
        _quadcopter = GetComponent<QuadcopterController>();
        _map = FindObjectOfType<Map>();
        LastSpawnpoint = FindAnyObjectByType<PlayerSpawner>().GetSpawnPoint();
        if (_map)
            NextCheckpoint = _map.GetNextCheckpoint(_quadcopter);
        _quadcopter.onDeath += OnDroneReset;
    }
    void OnDroneReset()
    {
        RestartAmount++;
        transform.position = LastSpawnpoint.position;
        transform.rotation = LastSpawnpoint.rotation;
    }
    private void OnDestroy()
    {
        _quadcopter.onDeath -= OnDroneReset;
    }
    public void PassCheckpoint(int value)
    {
        Score += value;
        NextCheckpoint = _map.GetNextCheckpoint(_quadcopter);
    }
}
