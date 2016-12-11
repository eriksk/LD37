using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Scripts.Cars;
using Assets._Project.Scripts.Tracks;
using Assets._Project.Scripts.Game;

public class Car : MonoBehaviour
{
    public List<WheelSetting> Wheels;

    [Range(1f, 90f)]
    public float SteeringRange = 15f;
    [Range(0f, 1000f)]
    public float EngineForce = 100f;
    [Range(0f, 1000f)]
    public float BrakeForce = 100f;

    public float DownForce = 10f;

    public float TimeNotGroundedTresholdDownForce = 0.2f;

    private float _steerAngle;
    private float _motorTorque;
    private float _brakeTorque;
    private bool _isEngaged = false;

    private Rigidbody _rigidyBody;
    public CarController Controller;
    public RaceTrack Track;

    public CarAiVariables AiVariables = new CarAiVariables();

    void Start ()
    {
        AiVariables = new CarAiVariables();
        _rigidyBody = GetComponent<Rigidbody>();
    }

    public void EngageControl()
    {
        _isEngaged = true;
    }

    public int CurrentSpeed
    {
        get { return _rigidyBody == null ? 0 : (int)_rigidyBody.velocity.magnitude; }
    }

    public void UpdateInput(float steerAngle, float motor, float brakes)
    {
        _steerAngle = steerAngle;
        _motorTorque = motor;
        _brakeTorque = brakes;
    }

    public float _timeNotGrounded;
    void FixedUpdate()
    {
        var grounded = false;

        float forwardSlip = 0;
        float sideSlip = 0;
        for (int i = 0; i < Wheels.Count; i++)
        {
            var wheel = Wheels[i];
            WheelHit hit;
            if(wheel.Collider.GetGroundHit(out hit))
            {
                grounded = true;
                forwardSlip += hit.forwardSlip;
                sideSlip += hit.sidewaysSlip;
            }
        }

        if (!grounded)
        {
            _timeNotGrounded += Time.deltaTime;
        }
        else
        {
            _timeNotGrounded = 0f;
        }


        float downForce = _timeNotGrounded > TimeNotGroundedTresholdDownForce ? 0f : 1f;
        var velocity = _rigidyBody.velocity.magnitude;
        _rigidyBody.AddForce(transform.up * -DownForce * velocity * downForce);
    }

    void Update ()
    {
        if (!_isEngaged) return;

        Controller.UpdateControl(this, Track);

        if (Wheels == null) return;

        foreach (var wheel in Wheels)
            ApplyWheel(wheel);

        if(CurrentSpeed <= AiVariables.VelocityCheckTreshold)
        {
            AiVariables.TimeWithVelocityBelowTreshold += Time.deltaTime;
        }
        else
        {
            AiVariables.TimeWithVelocityBelowTreshold = 0f;
        }
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

    public void Reset()
    {
        AiVariables.Reset();
        _timeNotGrounded = 0f;
        var state = GetComponent<RaceProgressState>();
        var checkpoint = state.GetCurrentResetCheckpoint();
        var nextCheckpoint = state.GetNextCheckpoint();

        _rigidyBody.velocity = Vector3.zero;
        _rigidyBody.angularVelocity = Vector3.zero;
        foreach (var wheel in Wheels)
        {
            wheel.Collider.motorTorque = 0;
            wheel.Collider.brakeTorque = 1000000;
            wheel.Collider.steerAngle = 0;
        }

        transform.position = checkpoint.transform.position + (Vector3.up * 3f);
        transform.LookAt(nextCheckpoint.transform.position);
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

public class CarAiVariables
{
    public float Angle;
    public float VelocityCheckTreshold = 3;
    public float TimeWithVelocityBelowTreshold = 0f;

    internal void Reset()
    {
        Angle = 0;
        TimeWithVelocityBelowTreshold = 0f;
    }
}