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

// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//   	// Variables related to player character movement
//   	public InputAction MoveAction;
//   	Rigidbody2D rigidbody2d;
//   	Vector2 move;
//   	public float speed = 3.0f;
	
//   	// Variables related to the health system
//   	public int maxHealth = 5;
//   	int currentHealth;
//   	public int health { get { return currentHealth; }}

//   	// Variables related to temporary invincibility
//   	public float timeInvincible = 2.0f;
//   	bool isInvincible;
// float damageCooldown;

//   	// Variables related to animation
//    	Animator animator;
//    	Vector2 moveDirection = new Vector2(1, 0);

//   	// Variables related to projectiles
//    	public GameObject projectilePrefab;
//    	public InputAction LaunchAction;



//   	// Start is called once before the first execution of Update after the MonoBehaviour is created 
//   	void Start()
//   	{
//      		MoveAction.Enable();
//      		LaunchAction.Enable();
//      		rigidbody2d = GetComponent<Rigidbody2D>();
//      		animator = GetComponent<Animator>();

//      		currentHealth = maxHealth;
//   	}
 
//   	// Update is called once per frame
//   	void Update()
//   	{
//      		move = MoveAction.ReadValue<Vector2>();

// if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
//        	{
//            		moveDirection.Set(move.x, move.y);
//            		moveDirection.Normalize();
//        	}

//        	animator.SetFloat("Look X", moveDirection.x);
//        	animator.SetFloat("Look Y", moveDirection.y);
//        	animator.SetFloat("Speed", move.magnitude);
//        	if (isInvincible)
//        	{
//            		damageCooldown -= Time.deltaTime;
//            		if (damageCooldown < 0)
//            		{
//                		isInvincible = false;
//            		}
//        	}

//        	if (LaunchAction.WasPressedThisFrame())
//        	{
//            		Launch();
//        	}

//    	}

// // FixedUpdate has the same call rate as the physics system
//   	void FixedUpdate()
//   	{
//      		Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
//      		rigidbody2d.MovePosition(position);
//   	}

//   	public void ChangeHealth (int amount)
//   	{
//      		if (amount < 0)
//        	{
//            		if (isInvincible)
// 			{
//                		return;
//  			}         
//            		isInvincible = true;
//            		damageCooldown = timeInvincible;
//     			animator.SetTrigger("Hit");
//        	}

//      		currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
//      		UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
//   	}

// void Launch()
//    	{
//        	GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
//        	Projectile projectile = projectileObject.GetComponent<Projectile>();
//        	projectile.Launch(moveDirection, 300);

//        	animator.SetTrigger("Launch");
//    	}
// }