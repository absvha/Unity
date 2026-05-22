using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile hit " + other.name);

        // Nếu va chạm với cáo
            if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);

            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.ChangeHealth(+2);
            }
        }

        // Xóa đạn sau va chạm
        Destroy(gameObject);
    }
}
