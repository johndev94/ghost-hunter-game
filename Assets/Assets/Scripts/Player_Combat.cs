using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform attackPoint;
    public float weaponRange = 1;
    public LayerMask enemyLayer;
    public int damage = 1;

    public Animator anim;
    public float cooldown = 13f;
    private float timer;
    public float knockbackForce = 10f;
    public float knockbackTime = 0.15f;
    public float stunTime = 0.5f;


    public void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }     
    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);


            

            timer = cooldown;
        }
    }

    public void DealDamage(){

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

            if(enemies.Length > 0)
            {
                enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-damage);
                enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            }
    }


    public void FinishAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }

}
