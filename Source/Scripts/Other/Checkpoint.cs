using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    private List<QuadcopterController> _checkedCopters = new List<QuadcopterController>();
    [SerializeField] GameObject _checkParticle;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] int _cost = 1000;
    public UnityEvent onCheckpoint;
    public bool CheckIfCompleted(QuadcopterController copter)
    {
        return _checkedCopters.Contains(copter);
    }
    private void OnTriggerEnter(Collider other)
    {
        var copter = other.GetComponent<QuadcopterController>();
        if (copter)
        {
            if (_checkedCopters.Contains(copter))
                return;
            _checkedCopters.Add(copter);
            Destroy(Instantiate(_checkParticle, transform), 3);
            var player = copter.GetComponent<CopterScore>();
            if (player)
            {
                player.LastSpawnpoint = _spawnPoint;
                player.PassCheckpoint(_cost);
                onCheckpoint?.Invoke();
            }
        }
    }
}
