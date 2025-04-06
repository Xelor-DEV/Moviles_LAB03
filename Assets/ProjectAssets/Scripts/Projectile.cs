using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private int damage;

    public void Initialize(int projectileDamage)
    {
        damage = projectileDamage;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Obstacle obstacle = other.GetComponent<Obstacle>();
            obstacle.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("DeleterProjectile"))
        {
            Destroy(gameObject);
        }
    }
}