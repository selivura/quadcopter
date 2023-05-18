using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DroneInputs : MonoBehaviour
{
    private float _pitch;
    private float _yaw;
    private float _roll;
    private float _throttle;
    private bool _enbleVisual = false;
    private bool _enableDirection = true;
    public float Pitch { get => _pitch; set => _pitch = value; }
    public float Yaw { get => _yaw; set => _yaw = value; }
    public float Roll { get => _roll; set => _roll = value; }
    public float Throttle { get => _throttle; set => _throttle = value; }
    public bool EnbleVisual { get => _enbleVisual; set => _enbleVisual = value; }
    public bool EnableDirection { get => _enableDirection; set => _enableDirection = value; }

    public Action onReset;
    public Action onRequestPause;
    private PlayerInput _input;
    private QuadcopterController _controller;
    private void Start()
    {
        _controller = GetComponent<QuadcopterController>();
    }
    public void OnPause(InputValue value)
    {
        onRequestPause?.Invoke();
    }
    public void OnPitch(InputValue value)
    {
        _pitch = value.Get<float>();
    }
    public void OnYaw(InputValue value)
    {
        _yaw = value.Get<float>();
    }
    public void OnRoll(InputValue value)
    {
        _roll = value.Get<float>();
    }
    public void OnThrottle(InputValue value)
    {
        _throttle = value.Get<float>();
    }
    public void OnResetDrone()
    {
        onReset?.Invoke();
    }
    public void OnToggleVisualization()
    {
        _enbleVisual = !_enbleVisual;
    }
    public void OnToggleDirection()
    {
        _enableDirection = !_enableDirection;
    }
    public void OnToggleGUI()
    {
        FindObjectOfType<UISetuper>().ToggleGUI(_controller.PlayerID);
    }
    private void OnDestroy()
    {
        onReset = null;
        onRequestPause = null;
    }
}
