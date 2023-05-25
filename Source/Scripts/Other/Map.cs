using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> _checkpoints;
    public List<Checkpoint> Checkpoints { get => _checkpoints; }
    [SerializeField] Transform _levelEnd;
    public Transform GetNextCheckpoint(QuadcopterController _caller)
    {
        for (int i = 0; i < Checkpoints.Count; i++)
        {
            if(!Checkpoints[i].CheckIfCompleted(_caller))
                return Checkpoints[i].transform;
        }
        return _levelEnd;
    }
}
