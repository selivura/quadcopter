using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] GameObject _checkParticle;
    private void OnTriggerEnter(Collider other)
    {
        var copter = other.GetComponent<QuadcopterController>();
        if (copter)
        {           
            Destroy(Instantiate(_checkParticle, transform), 3);
            FindObjectOfType<PlayerSpawner>().WinGame();
            copter.GetComponent<AudioSource>().enabled = false;
            foreach (var source in  copter.GetComponentsInChildren<AudioSource>() )
            {
                source.enabled = false;
            }   
        }
    }
}
