using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ===== MOVEMENT =====
    public InputAction MoveAction;
    public float speed = 5f;

    Rigidbody2D rb;
    Vector2 move;

    // ===== ANIMATION =====
    Animator animator;
    Vector2 moveDirection = Vector2.down;

    // ===== HEALTH =====
    public int maxHealth = 20;
    int currentHealth;
    public int health { get { return currentHealth; } }

    // ===== INVINCIBLE =====
    public float timeInvincible = 2f;
    bool isInvincible;
    float damageCooldown;

    // ===== PROJECTILE =====
    public InputAction LaunchAction;
    public GameObject projectilePrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth/2;
    }

    void OnEnable()
    {
        MoveAction.Enable();
        LaunchAction.Enable();
    }

    void OnDisable()
    {
        MoveAction.Disable();
        LaunchAction.Disable();
    }

    void Update()
    {
        // READ INPUT
        move = MoveAction.ReadValue<Vector2>();

        // DEBUG (xem có ra Vector2 không)
        Debug.Log("MOVE = " + move);

        // UPDATE DIRECTION
        if (move != Vector2.zero)
        {
            moveDirection = move.normalized;
        }

        // ANIMATION
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // INVINCIBLE TIMER
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown <= 0)
                isInvincible = false;
        }

        // SHOOT
        if (LaunchAction.WasPressedThisFrame())
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        // MOVE PLAYER
        rb.linearVelocity = move * speed;
    }

    // ===== HEALTH =====
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
		Debug.Log("Health = " + currentHealth);
    }

    // ===== PROJECTILE =====
    void Launch()
    {
        GameObject projectileObject = Instantiate(
            projectilePrefab,
            rb.position + moveDirection * 0.5f,
            Quaternion.identity
        );

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");
    }
		void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			ChangeHealth(-2); // giảm máu khi chạm enemy
		}
	}
} 