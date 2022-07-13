using System.Collections;
using UnityEngine;

public class Soldier : Enemy
{
    EnemyMovement movement;
    private void Start()
    {
        movement = GetComponent<EnemyMovement>();
    }
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (movement.currentTarget.GetComponent<Enemy>())
            movement.currentTarget.GetComponent<Enemy>().Hit(25);
        if (movement.currentTarget.GetComponent<PlayerMovement>())
            movement.currentTarget.GetComponent<PlayerMovement>().Hit(25);
    }


    public override void Hit(int hitAmount)
    {
        if (movement.enemyState == EnemyMovement.EnemyStates.Dead)
        {
            return;
        }
        healthPoints -= hitAmount;
        if (healthPoints <= 0)
        {
            movement.enemyState = EnemyMovement.EnemyStates.Dead;
            movement.uicontroller.EnemyDied(GetComponent<EnemyMovement>());
            return;
        }
        StartCoroutine(Hit());

    }
    IEnumerator Hit()
    {
        movement.agent.isStopped = true;
        movement.animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1);
        if (movement.agent.enabled)
            movement.agent.isStopped = false;
    }
}

public interface ITargetableTarget
{
    public Transform target { get; }
}