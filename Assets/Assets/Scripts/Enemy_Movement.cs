using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private EnemyState enemyState;

    private int facingDirection = 1;
    private float speed = 1.2f;
    public float attackRange = 2;

    public Animator anim;

    // Attack-related variables
    public Transform attackPoint;  // Ensure this is assigned in the inspector
    public float weaponRange = 1f; // Ensure this is assigned in the inspector
    public LayerMask playerLayer;  // Ensure this is assigned in the inspector
    public float damage = 10f;     // Ensure this is assigned in the inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This auto connects rb to the Rigidbody2D component attached to the GameObject, useful for spawning enemies
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Set the enemy state to idle
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is in the trigger area
        if (player != null) // Ensure player reference is valid before moving
        {
            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero; // Stop the enemy when player exits the trigger
            }
        }
    }

    void Chase()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            ChangeState(EnemyState.Attacking);
        }

        // Flip Logic
        // Check if the player is to the right of the enemy and the enemy is facing left or vice versa, then flip the enemy
        else if (player.position.x < transform.position.x && facingDirection == 1 ||
        player.position.x > transform.position.x && facingDirection == -1)
        {
            Flip();
        };

        // Chase Logic
        // Gets the difference between the player's position and the enemy's position, then normalizes it.
        Vector2 direction = (player.position - transform.position).normalized;
        // So the enemy moves towards the player, we multiply the normalized direction by a speed value
        rb.linearVelocity = direction * speed; // Move the enemy towards the player 
    }

    void FixedUpdate()
    {
        // Update animator parameters
        anim.SetFloat("horizontal", Mathf.Abs(rb.linearVelocity.x)); // Use Mathf.Abs for positive values
        anim.SetFloat("vertical", Mathf.Abs(rb.linearVelocity.y));
    }

    // Flip the enemy sprite to face the player
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Set the player reference to the player object
            if (player == null)
            {
                player = collision.transform;
            }

            // Change the enemy state to chasing
            ChangeState(EnemyState.Chasing);

            // Test what the enemy state is on enter
            UnityEngine.Debug.Log("Enemy state on trigger: " + enemyState);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero; // Stop the enemy when player exits the trigger

            // Change the enemy state to idle
            ChangeState(EnemyState.Idle);

            // Clear the player reference when the player leaves the trigger
            player = null;

            // Test what the enemy state is on exit
            UnityEngine.Debug.Log("Enemy state on exit trigger: " + enemyState);
        }
    }

    public void Attack()
    {
        // Ensure that the attack point, player layer, and other variables are assigned
        if (attackPoint == null || playerLayer == 0)
        {
            UnityEngine.Debug.LogError("Attack point or player layer not assigned.");
            return;
        }

        // Check if player reference is valid before attacking
        if (player != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

            if (hits.Length > 0)
            {
                UnityEngine.Debug.Log("Player is in range!");

                // Check if the hit object has the PlayerHealth component before applying damage
                PlayerHealth playerHealth = hits[0].GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    UnityEngine.Debug.Log("Player took damage!");
                }
                else
                {
                    UnityEngine.Debug.LogWarning("PlayerHealth component not found on hit object.");
                }
            }
            else
            {
                UnityEngine.Debug.Log("No players in range.");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("Player reference is null, cannot attack.");
        }

        UnityEngine.Debug.Log("Enemy is attacking!");
    }

    // This method changes the enemy state and activates the corresponding animation
    public void ChangeState(EnemyState newState)
    {
        // Exit the current animation state
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", false);
        }

        // Updates the new enemy state
        enemyState = newState;

        // Update the new animation state
        if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            anim.SetBool("isChasing", true);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", true);
        }
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}
