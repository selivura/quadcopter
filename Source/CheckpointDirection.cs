using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckpointDirection : MonoBehaviour
{
    [SerializeField] QuadcopterController _quadcopterController;
    [SerializeField] GameObject _graphics;
    private void Update()
    {
        if(_graphics.activeSelf != _quadcopterController.Inputs.EnableDirection)
            _graphics.SetActive(_quadcopterController.Inputs.EnableDirection);
        if(_quadcopterController.NextCheckpoint)
        transform.forward = _quadcopterController.NextCheckpoint.position - transform.position;
    }
}
