using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SpaceShipData spaceShipData;
    [SerializeField] private HealthData healthData;
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private SensorData sensorData;

    [Header("Movement Settings")]
    [SerializeField] private float movementSmoothing = 5f;
    [SerializeField] private float verticalLimit = 4f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float scoreInterval = 1f;

    private float nextFireTime;

    private float targetYPosition;
    private Transform shipTransform;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    public HealthData HealthData
    {
        get
        {
            return healthData;
        }
    }

    private void Awake()
    {
        shipTransform = transform;
        mainCamera = Camera.main;
        healthData.Initialize(spaceShipData.MaxHealth);
        scoreData.ResetCurrentScore();
    }

    private void Start()
    {
        CalculateMovementLimits();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spaceShipData.ShipSprite;
        }

        StartCoroutine(ScoreUpdateRoutine());
        StartCoroutine(AutoFireRoutine());
    }

    private IEnumerator ScoreUpdateRoutine()
    {
        scoreData.CurrentScore = scoreData.CurrentScore + spaceShipData.ScoreSpeed;
        yield return new WaitForSeconds(scoreInterval);
        StartCoroutine(ScoreUpdateRoutine());
    }

    private IEnumerator AutoFireRoutine()
    {
        FireProjectile();
        yield return new WaitForSeconds(fireRate);
        StartCoroutine(AutoFireRoutine());
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(spaceShipData.ProjectileDamage);
    }

    private void CalculateMovementLimits()
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(shipTransform.position);
        verticalLimit = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, viewportPoint.z)).y;
    }

    private void Update()
    {
        HandleMovementInput();
        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        float input = GetSensorInput();
        targetYPosition = targetYPosition + (input * spaceShipData.Handling * Time.deltaTime);
        targetYPosition = Mathf.Clamp(targetYPosition, -verticalLimit, verticalLimit);
    }

    private float GetSensorInput()
    {
        switch (sensorData.CurrentSensorMode)
        {
            case SensorMode.Gyroscope:
                return sensorData.ScaledEulerRotation.y;

            case SensorMode.Accelerometer:
                return sensorData.ScaledAcceleration.y;

            default:
                return 0f;
        }
    }

    private void ApplyMovement()
    {
        Vector3 newPosition = shipTransform.position;
        newPosition.y = Mathf.Lerp(newPosition.y, targetYPosition, movementSmoothing * Time.deltaTime);
        shipTransform.position = newPosition;
    }
}