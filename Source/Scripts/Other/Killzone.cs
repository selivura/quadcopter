using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private float _lastDamageTime = 0;
    [SerializeField] private float _damagePerSecond = 10;
    [SerializeField] private float _damageEverySeconds = 1;
    List<Propeller> _colliding = new List<Propeller>();
    private void Update()
    {
        if (Time.time >= _lastDamageTime + _damageEverySeconds)
        {
            foreach (var propeller in _colliding)
            {
                propeller.Damage(_damagePerSecond);
            }
            _lastDamageTime = Time.time;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       var collided = other.GetComponent<Propeller>();
        if(collided)
            _colliding.Add(collided);
    }

    private void OnTriggerExit(Collider other)
    {
        var collided = other.GetComponent<Propeller>();
        if (collided)
            _colliding.Remove(collided);
    }
}
