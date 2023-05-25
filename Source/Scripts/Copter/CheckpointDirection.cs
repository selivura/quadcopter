using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CopterScore))]
public class CheckpointDirection : MonoBehaviour
{
    CopterScore _playerScore;
    [SerializeField] GameObject _graphics;
    [SerializeField] Transform _arrow;
    public bool Enable = true;
    private void Awake()
    {
        _playerScore = GetComponent<CopterScore>();
    }
    private void Update()
    {
        if(_graphics.activeSelf != Enable)
            _graphics.SetActive(Enable);
        if(_playerScore.NextCheckpoint)
            _arrow.forward = _playerScore.NextCheckpoint.position - _arrow.position;
    }
}
