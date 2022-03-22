using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutInvader : Enemy
{
    public GameObject projectile;
    float distanceToPlayer;
    public float attackRange;
    public float acceleration;
    float nextShotTime;
    public float cooldown;
    public float projectileSpeed;
    public int projectileDamage;

    public float dodgeForce;
    public int dodgeDuration;

    private void FixedUpdate()
    {
        AvoidCollision();

        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position);
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            if (IsInScreen(0.05f) && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
                nextShotTime = Time.time + cooldown;

                GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
                newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
                newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage;

            }
        }
        m_rigidbody.AddForce(direction * acceleration);
    }

    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);
        Dodge();
        //StartCoroutine(DodgeCoroutine());
    }

    IEnumerator DodgeCoroutine()
    {
        bool leftOrRight = true;
        int dodgeDuration = 20;
        Vector2 dodgeDirection;
        if (Random.value < 0.5) leftOrRight  = false;
        
        for (int t=0; t<dodgeDuration; t++)
        {
            if (leftOrRight) dodgeDirection = Vector2.Perpendicular(direction);
            else dodgeDirection = -Vector2.Perpendicular(direction);
            m_rigidbody.AddForce(dodgeDirection * dodgeForce);
            yield return new WaitForFixedUpdate();
        }

        
    }

    void Dodge()
    {
        Debug.Log("Dodge!");
        Vector2 dodgeDirection;
        if (Random.value < 0.5) dodgeDirection = Vector2.Perpendicular(direction);
        else dodgeDirection = -Vector2.Perpendicular(direction);
        m_rigidbody.AddForce(dodgeDirection * dodgeForce);
    }




}
