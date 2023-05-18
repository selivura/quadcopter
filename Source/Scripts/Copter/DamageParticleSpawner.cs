using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject _particle;
    public void Spawn(float lifetime)
    {
        Destroy(Instantiate(_particle, transform.position, Quaternion.identity),lifetime);
    }
}
