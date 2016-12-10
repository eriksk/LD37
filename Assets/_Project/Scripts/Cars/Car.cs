using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Car : MonoBehaviour
{
    public List<WheelSetting> Wheels;

    [Range(1f, 90f)]
    public float SteeringRange = 15f;
    [Range(0f, 1000f)]
    public float EngineForce = 100f;
    [Range(0f, 1000f)]
    public float BrakeForce = 100f;

    private float _steerAngle;
    private float _motorTorque;
    private float _brakeTorque;

    void Start ()
    {
    
    }

    public void UpdateInput(float steerAngle, float motor, float brakes)
    {
        _steerAngle = steerAngle;
        _motorTorque = motor;
        _brakeTorque = brakes;
    }

    void Update ()
    {
        if (Wheels == null) return;

        foreach (var wheel in Wheels)
            ApplyWheel(wheel);
    }

    private void ApplyWheel(WheelSetting wheel)
    {
        if (wheel.Steering)
            wheel.Collider.steerAngle = _steerAngle * SteeringRange;
        if (wheel.Motor)
            wheel.Collider.motorTorque = _motorTorque * EngineForce;
        if (wheel.Brakes)
            wheel.Collider.brakeTorque = _brakeTorque * BrakeForce;

        Vector3 worldPosition;
        Quaternion worldRotation;
        wheel.Collider.GetWorldPose(out worldPosition, out worldRotation);
        wheel.Model.position = worldPosition;

        wheel.Model.rotation = wheel.Flip ? worldRotation * Quaternion.Euler(0f, 180, 0f):  worldRotation;

    }
}

[Serializable]
public class WheelSetting
{
    public string Name;
    public WheelCollider Collider;
    public Transform Model;
    public bool Motor;
    public bool Steering;
    public bool Brakes;
    public bool Flip;
}
