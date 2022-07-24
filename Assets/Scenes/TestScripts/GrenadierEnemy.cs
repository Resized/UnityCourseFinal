using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierEnemy : RangeEnemy
{
    public override HealthManager _healthManager { get; set; }
    public float projectileDamageRadius;
    public float bombAreaDamage;
    // public override void Attack(IHittable target)
    // {
    //     if (!isAttacking)
    //     {
    //         ProjectileBase arrow = CreateProjectile(target);
    //         StartCoroutine(AttackCooldown(arrow));
    //     }
    // }

    // IEnumerator AttackCooldown(ProjectileBase arrow)
    // {
    //     isAttacking = true;
    //     yield return new WaitForSeconds(loadTime);

    //     arrow.enabled = true;
    //     yield return new WaitForSeconds(timeBetweenAttacks - loadTime);
    //     isAttacking = false;
    // }



    protected override ProjectileBase CreateProjectile(IHittable target)
    {
        ProjectileBase bomb = base.CreateProjectile(target);
        BombProjectile bp = (BombProjectile)bomb;
        bp.areaDamage = bombAreaDamage;
        bp.explosionRadius = projectileDamageRadius;
        return bp;
    }
}
