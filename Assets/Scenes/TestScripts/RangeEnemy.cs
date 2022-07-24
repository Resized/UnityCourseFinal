using UnityEngine;
using System.Collections;
public abstract class RangeEnemy : Enemy
{

    public float loadTime;
    bool isLoading;
    [Range(5, 50)]
    public float projectileSpeed;
    public ProjectileBase pfProjectile;
    public Transform projectileSpawnPoint;
    public GameObject weapon;

    protected virtual ProjectileBase CreateProjectile(IHittable target)
    {
        ProjectileBase projectile = Instantiate(pfProjectile, projectileSpawnPoint.position, Quaternion.identity, transform).GetComponent<ProjectileBase>();
        projectile.SetTarget(target, projectileSpeed, gameObject);
        projectile.damageOnHit = attackDamage;
        return projectile;

    }
    public override void Attack(IHittable target)
    {
        if (!isAttacking)
        {
            ProjectileBase projectile = CreateProjectile(target);
            StartCoroutine(AttackCooldown(projectile));
        }
    }

    IEnumerator AttackCooldown(ProjectileBase projectile)
    {
        isAttacking = true;
        yield return new WaitForSeconds(loadTime);
        projectile.enabled = true;
        yield return new WaitForSeconds(timeBetweenAttacks - loadTime);
        isAttacking = false;
    }
}