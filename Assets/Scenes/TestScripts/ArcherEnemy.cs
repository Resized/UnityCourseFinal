using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : RangeEnemy
{
    public override HealthManager _healthManager { get; set; }

    public override void Attack(IHittable target)
    {
        if (!isAttacking)
        {
            ProjectileBase arrow = CreateProjectile(target);
            StartCoroutine(AttackCooldown(arrow));
        }
    }

    IEnumerator AttackCooldown(ProjectileBase arrow)
    {
        isAttacking = true;
        yield return new WaitForSeconds(loadTime);

        arrow.enabled = true;
        yield return new WaitForSeconds(timeBetweenAttacks - loadTime);
        isAttacking = false;
    }



    ProjectileBase CreateProjectile(IHittable target)
    {
        ProjectileBase arrow = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity, transform).GetComponent<ProjectileBase>();
        arrow.SetTarget(target, projectileSpeed);
        return arrow;

    }
}
