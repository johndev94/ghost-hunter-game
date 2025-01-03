using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    
    
    
    public float attackRange = 2;
    public float weaponRange = 1f;

    public float damage = 10f;   
    public float playerDetectRange = 5f;
    public Animator anim;
    // Attack-related variables
    public Transform attackPoint; 
    public LayerMask playerLayer; 
    public Transform detectionPoint;
    public float attackCooldown = 1f;

    private float attackCooldownTimer = 0f;
    private float speed = 1.2f;
    private int facingDirection = 1;
    private Rigidbody2D rb;
    private Transform player;
    private EnemyState enemyState;

    private CombatMusicManager musicManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This auto connects rb to the Rigidbody2D component attached to the GameObject, useful for spawning enemies
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        musicManager = FindFirstObjectByType<CombatMusicManager>(); // Finds the CombatMusicManager in the scene
        // Set the enemy state to idle
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
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
        

        // Flip Logic
        // Check if the player is to the right of the enemy and the enemy is facing left or vice versa, then flip the enemy
        if (player.position.x < transform.position.x && facingDirection == 1 ||
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

    private void CheckForPlayer()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if(hits.Length > 0)
        {
            player = hits[0].transform;
            // if player is in attack range and cooldown is ready
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.transform.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else{
            // Stops enemy from drifting when player is out of range
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
            // Test what the enemy state is on enter
            UnityEngine.Debug.Log("Enemy state on trigger: " + enemyState);
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
        if (musicManager != null) musicManager.StopCombatMusic(); // Stop combat music when idle
    }
    else if (enemyState == EnemyState.Chasing)
    {
        anim.SetBool("isChasing", false);
    }
    else if (enemyState == EnemyState.Attacking)
    {
        anim.SetBool("isAttacking", false);
        if (musicManager != null) musicManager.PlayCombatMusic(); // Start combat music on attack
    }
    anim.SetBool("isIdle", true);

    // Updates the new enemy state
    enemyState = newState;

    // Update the new animation state
    if (enemyState == EnemyState.Idle)
    {
        anim.SetBool("isIdle", true);
        UnityEngine.Debug.Log("Enemy is idle.");
    }
    else if (enemyState == EnemyState.Chasing)
    {
        anim.SetBool("isChasing", true);
        UnityEngine.Debug.Log("Enemy is chasing.");
    }
    else if (enemyState == EnemyState.Attacking)
    {
        anim.SetBool("isAttacking", true);
        UnityEngine.Debug.Log("Enemy is attacking.");
    }
}

}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}
