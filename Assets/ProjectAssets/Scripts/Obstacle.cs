using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private HealthData healthTemplate;
    [SerializeField] private int damageToPlayer = 1;

    private int currentHealth;
    private int maxHealth;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value <= 0)
            {
                currentHealth = 0;
            }
            else if (value > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = value;
            }

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        maxHealth = healthTemplate.MaxHealth;
        currentHealth = maxHealth;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.left * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthData playerHealth = other.GetComponent<PlayerController>().HealthData;
            playerHealth.TakeDamage(damageToPlayer);
            Destroy(gameObject);
        }

        if (other.CompareTag("DeleterObstacle"))
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = CurrentHealth - amount;
    }
}