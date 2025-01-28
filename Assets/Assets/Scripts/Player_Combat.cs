using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator anim;
    public float cooldown = 1.5f;
    private float timer;

    public void Attack()
    {
        if(timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
        anim.SetBool("isAttacking", true);
    }

    public void FinishAttack()
    {
        anim.SetBool("isAttacking", false);
    }

}
