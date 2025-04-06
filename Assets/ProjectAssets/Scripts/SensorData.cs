﻿using UnityEngine;

[CreateAssetMenu(fileName = "Sensor Data", menuName = "ScriptableObjects/Settings/Sensor Data", order = 1)]
public class SensorData : ScriptableObject
{
    [Header("Sensor Mode")]
    [SerializeField] private SensorMode currentSensorMode;

    [Header("Accelerometer")]
    [SerializeField] private Vector3 rawAcceleration;
    [SerializeField] private Vector3 scaledAcceleration;

    [Header("Gyroscope")]
    [SerializeField] private Quaternion rawRotation;
    [SerializeField] private Vector3 scaledEulerRotation;

    public Vector3 RawAcceleration
    {
        get
        {
            return rawAcceleration;
        }
        set
        {
            rawAcceleration = value;
        }
    }

    public Vector3 ScaledAcceleration
    {
        get
        {
            return scaledAcceleration;
        }
        set
        {
            scaledAcceleration = value;
        }
    }

    public Quaternion RawRotation
    {
        get
        {
            return rawRotation;
        }
        set
        {
            rawRotation = value;
        }
    }

    public Vector3 ScaledEulerRotation
    {
        get
        {
            return scaledEulerRotation;
        }
        set
        {
            scaledEulerRotation = value;
        }
    }

    public SensorMode CurrentSensorMode
    {
        get
        {
            return currentSensorMode;
        }
        set
        {
            currentSensorMode = value;
        }
    }
}