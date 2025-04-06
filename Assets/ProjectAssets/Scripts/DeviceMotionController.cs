using UnityEngine;

public enum SensorMode
{
    Auto,
    Gyroscope,
    Accelerometer
}
public class DeviceMotionController : MonoBehaviour
{
    [Header("Sensor Config")]
    [SerializeField] private SensorData sensorData;
    [SerializeField] private SensorMode inputMode = SensorMode.Auto;
    [SerializeField] private Vector3 accelerometerScale = Vector3.one;
    [SerializeField] private Vector3 gyroscopeEulerScale = new Vector3(1, 1, -1);

    [Header("Debug Values")]
    [SerializeField] private SensorMode currentMode;
    [SerializeField] private Vector3 debugAcceleration;
    [SerializeField] private Vector3 debugScaledAcceleration;
    [SerializeField] private Quaternion debugGyroRotation;
    [SerializeField] private Vector3 debugScaledEuler;

    private Gyroscope deviceGyroscope;
    private bool gyroSupported;
    private SensorMode lastValidMode;

    private void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;
        ValidateSensorMode();
        InitializeSensors();
    }

    private void ValidateSensorMode()
    {
        if (inputMode == SensorMode.Auto)
        {
            if (gyroSupported == true)
            {
                currentMode = SensorMode.Gyroscope;
            }
            else
            {
                currentMode = SensorMode.Accelerometer;
            }
        }
        else if (inputMode == SensorMode.Gyroscope)
        {
            if (gyroSupported == true)
            {
                currentMode = SensorMode.Gyroscope;
            }
            else
            {
                Debug.LogWarning("Gyroscope not available. Using accelerometer.");
                inputMode = SensorMode.Accelerometer;
                currentMode = SensorMode.Accelerometer;
            }
        }
        else
        {
            currentMode = SensorMode.Accelerometer;
        }

        if (currentMode == SensorMode.Gyroscope && gyroSupported == false)
        {
            currentMode = SensorMode.Accelerometer;
        }

        lastValidMode = currentMode;
        sensorData.CurrentSensorMode = currentMode;
    }

    private void InitializeSensors()
    {
        if (gyroSupported == true)
        {
            deviceGyroscope = Input.gyro;
            if (currentMode == SensorMode.Gyroscope)
            {
                deviceGyroscope.enabled = true;
            }
            else
            {
                deviceGyroscope.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (inputMode != lastValidMode || currentMode != lastValidMode)
        {
            ValidateSensorMode();
            InitializeSensors();
        }

        UpdateSensorData();
        UpdateDebugValues();
    }

    private void UpdateSensorData()
    {
        if (currentMode == SensorMode.Gyroscope)
        {
            UpdateGyroData();
        }
        else
        {
            UpdateAccelerometerData();
        }
    }

    private void UpdateGyroData()
    {
        if (deviceGyroscope.enabled == false)
        {
            deviceGyroscope.enabled = true;
        }

        sensorData.RawRotation = deviceGyroscope.attitude;
        Vector3 euler = sensorData.RawRotation.eulerAngles;
        sensorData.ScaledEulerRotation = Vector3.Scale(euler, gyroscopeEulerScale);
    }

    private void UpdateAccelerometerData()
    {
        Vector3 rawAccel = Input.acceleration;

        // Ajuste para orientación landscape
        switch (Screen.orientation)
        {
            case ScreenOrientation.LandscapeLeft:
                // Mapeo correcto para movimiento vertical natural
                rawAccel = new Vector3(rawAccel.x, rawAccel.z, rawAccel.y);
                break;

            case ScreenOrientation.LandscapeRight:
                rawAccel = new Vector3(-rawAccel.x, rawAccel.z, -rawAccel.y);
                break;

            case ScreenOrientation.PortraitUpsideDown:
                rawAccel = new Vector3(-rawAccel.y, -rawAccel.x, rawAccel.z);
                break;
        }

        sensorData.RawAcceleration = rawAccel;
        sensorData.ScaledAcceleration = Vector3.Scale(rawAccel, accelerometerScale);
    }

    private void UpdateDebugValues()
    {
        debugAcceleration = sensorData.RawAcceleration;
        debugScaledAcceleration = sensorData.ScaledAcceleration;
        debugGyroRotation = sensorData.RawRotation;
        debugScaledEuler = sensorData.ScaledEulerRotation;
    }
}
