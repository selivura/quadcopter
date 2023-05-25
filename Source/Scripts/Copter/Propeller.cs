using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Propeller : MonoBehaviour
{
    [SerializeField] float _rotationFactor = 200;
    [SerializeField] Transform _propellerModel;
    public float RotationSpeed = 100;
    [SerializeField] float _maxRotationSpeed = 100;
    public float health = 100;
    [SerializeField] float _maxHealth = 100;
    public UnityEvent onDamageTaken;
    AudioSource _audioSource;

    public float _baseSoundVolume = 0.5f;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //var currentAngle =
        //    new Vector3
        //    (
        //        0,
        //        Mathf.LerpAngle(_propellerModel.localEulerAngles.z, _propellerModel.localEulerAngles.z + RotationSpeed * _rotationFactor, Time.deltaTime * _lerpT),
        //        0
        //        );
        //_propellerModel.localEulerAngles = currentAngle;
        _propellerModel.Rotate(Vector3.up, RotationSpeed * _rotationFactor * Time.deltaTime);
        _audioSource.volume = (RotationSpeed / _maxRotationSpeed) * _baseSoundVolume * SaveManager.CurrentSave.Sound;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Damage();
    }
    public void Damage(float value = 25)
    {
        health -= value;
        health = Mathf.Clamp(health, 0, 100);
        onDamageTaken?.Invoke();
    }
}
