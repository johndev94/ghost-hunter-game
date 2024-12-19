using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 movement;
    public int facingDirection = 1; // 1 for right, -1 for left
    public Rigidbody2D rb;
    public Animator anim;

    private bool IsKnockedBack;

    void FixedUpdate()
    {
        if(!IsKnockedBack)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Update animator parameters
            anim.SetFloat("horizontal", Mathf.Abs(horizontal)); // Use Mathf.Abs for positive values
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            // Check and flip the character's facing direction
            if ((horizontal > 0 && transform.localScale.x < 0) || (horizontal < 0 && transform.localScale.x > 0))
            {
                Flip();
            }

            // Adjust speed for running
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = 10f; // Boosted speed
            }
            else
            {
                speed = 5f; // Default speed
            }

            // Apply velocity
            rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
        }
        
    }

    // Method to flip the character
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void KnockBack(Transform enemy, float knockbackForce, float stunTime)
    {
        IsKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(KnockbackCounter(stunTime));
    }
    
    public IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        IsKnockedBack = false;
    }
}
