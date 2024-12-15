using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    private int facingDirection = 1;
    private float speed = 1.2f;
    public bool isChasing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This auto connects rb to the Rigidbody2D component attached to the GameObject, useful for spawning enemies
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            // Check if the player is to the right of the enemy and the enemy is facing left
            if(player.position.x > transform.position.x && facingDirection == -1)
            {
                // Flip the enemy sprite to face the player
                transform.localScale = new Vector3(1, 1, 1);
                facingDirection = 1;
            }
            // Check if the player is to the left of the enemy and the enemy is facing right
            else if(player.position.x < transform.position.x && facingDirection == 1)
            {
                // Flip the enemy sprite to face the player
                transform.localScale = new Vector3(-1, 1, 1);
                facingDirection = -1;

            }
            // Gets the difference between the player's position and the enemy's position, then normalizes it.
            Vector2 direction = (player.position - transform.position).normalized;
            // So the enemy moves towards the player, we multiply the normalized direction by a speed value
            rb.linearVelocity = direction * speed; // Move the enemy towards the player 
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Set the player reference to the player object
            if(player == null)
            {
                player = collision.transform;
            }
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero; // Stop the enemy when player exits the trigger
        }
    }
}
