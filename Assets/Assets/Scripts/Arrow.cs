using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb;
    public Vector2 dirrection = Vector2.right;
    public float lifeSpawn = 2;
    public float speed;

    void Start()
    {
        rb.linearVelocity = dirrection * speed;
        Destroy(gameObject, lifeSpawn);
    }
}
