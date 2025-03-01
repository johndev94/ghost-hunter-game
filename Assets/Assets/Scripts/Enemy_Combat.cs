using System.Diagnostics;
using System.Globalization;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float damage = 10f;
    public Transform attackPoint;
    public float weaponRange;
    private float knockbackForce = 8f;
    private float stunTime = 0.4f;

    // Checks if the player is in range of the enemy
    public LayerMask playerLayer;

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Check if the collision is with an object tagged "Enemy"
    //     if (collision.gameObject.tag == "Enemy")
    //     {
    //         UnityEngine.Debug.Log("Collided with Enemy!");

    //         // Reduce player health on collision with an enemy
    //         if (playerHealth != null)
    //         {
    //             playerHealth.TakeDamage(damage);  
    //         }

    //         // Get the current health of the player and display it
    //         float playerHealthCurrent = playerHealth.GetCurrentHealth();
    //         UnityEngine.Debug.Log("Player health: " + playerHealthCurrent);
    //     }
    // }

    public void OnAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if(hits.Length > 0)
        {
            UnityEngine.Debug.Log("Player is in range!");
            
            hits[0].GetComponent<PlayerHealth>().TakeDamage(damage);
            hits[0].GetComponent<PlayerMovement>().KnockBack(transform, knockbackForce, stunTime);
        }

        UnityEngine.Debug.Log("Enemy is attacking!");
    }
}
