using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] GameObject _checkParticle;
    [SerializeField] int _cost = 100;
    private void FixedUpdate()
    { 
        transform.Rotate(Vector3.up, 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        var copter = other.GetComponent<QuadcopterController>();
        if (copter)
        {
            Destroy(Instantiate(_checkParticle, transform.position, Quaternion.identity), 3);
            copter.Score += _cost;
            Destroy(gameObject);
        }
    }
}
