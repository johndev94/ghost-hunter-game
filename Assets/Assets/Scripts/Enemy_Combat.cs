using System.Diagnostics;
using System.Globalization;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float damage = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Enemy"
        if (collision.gameObject.tag == "Enemy")
        {
            UnityEngine.Debug.Log("Collided with Enemy!");

            // Reduce player health on collision with an enemy
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);  
            }

            // Get the current health of the player and display it
            float playerHealthCurrent = playerHealth.GetCurrentHealth();
            UnityEngine.Debug.Log("Player health: " + playerHealthCurrent);


        }
    }
}
