using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(QuadcopterController))]
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

    public Action onReset;
    public Action onRequestPause;
    private QuadcopterController _controller;
    private PhysicsVisualizer _visualizer;
    private CheckpointDirection _checkpointDirection;
    private void Start()
    {
        _controller = GetComponent<QuadcopterController>();
        _visualizer = GetComponent<PhysicsVisualizer>();
        _checkpointDirection = GetComponent<CheckpointDirection>();
    }
    private void FixedUpdate()
    {
        AddControls();
    }
    
    void AddControls()
    {
        _controller.SetTargetHeight(Throttle);

        _controller.Pitch = Pitch;

        _controller.Roll = Roll;

        _controller.YawDir = Yaw;
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
        _controller.ResetDrone();
    }
    public void OnToggleVisualization()
    {
        _enbleVisual = !_enbleVisual;
        _visualizer.EnableVisualisation = _enbleVisual;
    }
    public void OnToggleDirection()
    {
        _enableDirection = !_enableDirection;
        _checkpointDirection.Enable = _enableDirection;
    }
    public void OnToggleGUI()
    {
        FindObjectOfType<IngameMenu>().ToggleGUI(_controller.PlayerID);
    }
    private void OnDestroy()
    {
        onReset = null;
        onRequestPause = null;
    }
}
