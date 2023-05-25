using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    [SerializeField] GameObject _checkParticle;
    [SerializeField] int _cost = 100;
    [SerializeField] float _rotationSpeed = 10;
    [SerializeField] AudioClip _pickupClip;
    private void FixedUpdate()
    { 
        transform.Rotate(Vector3.up, _rotationSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<CopterScore>();
        if (player)
        {
            Destroy(Instantiate(_checkParticle, transform.position, Quaternion.identity), 3);
            player.Score += _cost;
            Destroy(gameObject);
        }
    }
    public void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(_pickupClip, transform.position);
    }
}
