using System;
using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override HealthManager _healthManager { get; set; }
    bool isSwinging = false;

    public override void Attack(IHittable target)
    {

        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown());
            StartCoroutine(SwingDelay(target));
        }
    }

    private IEnumerator SwingDelay(IHittable target)
    {
        yield return new WaitForSeconds(timeBetweenAttacks / 2);
        target.ProcessHit(attackDamage);
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

}

