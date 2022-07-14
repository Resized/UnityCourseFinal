using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmyUnitAI : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    private ITargetable currentTarget;
    [SerializeField] GameObject curTarget;
    private Animator animator;
    Collider[] collidersInRange;
    private float attackRange = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (curTarget == null)
        {
            SearchTargetsInRange();
        }

    }


    void SearchTargetsInRange()
    {
        collidersInRange = Physics.OverlapSphere(transform.position, attackRange);

        ITargetable closestEnemy = null;
        if (collidersInRange.Length != 0)
        {
            foreach (var collider in collidersInRange)
            {
                ITargetable target;
                if (!collider.gameObject.TryGetComponent<ITargetable>(out target))
                {
                    continue;
                }
                if (closestEnemy == null)
                {
                    closestEnemy = target;
                    continue;
                }
                if (CheckDistanceToTarget(closestEnemy) > CheckDistanceToTarget(target))
                {
                    closestEnemy = target;
                }

            }
            if (closestEnemy != null)
            {

                currentTarget = closestEnemy;
                curTarget = closestEnemy.gameObject;
            }

        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    float CheckDistanceToTarget(ITargetable target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    // private void Idle()
    // {
    //     animator.SetInteger("State", (int)EnemyStates.Idle);

    //     // if player is in range, change state to chase
    //     ChooseEnemyInRange();
    //     // every second decide if to change state to roam
    //     StartCoroutine(DecideIdleOrRoam());
    // }
    // private void Roam()
    // {
    //     agent.speed = walkSpeed;
    //     animator.SetInteger("State", (int)EnemyStates.Roam);
    //     if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
    //     {
    //         enemyState = EnemyStates.Idle;
    //     }
    //     ChooseEnemyInRange();
    // }
    // private void Chase()
    // {
    //     agent.speed = runSpeed;
    //     animator.SetInteger("State", (int)EnemyStates.Chase);
    //     agent.SetDestination(currentTarget.transform.position);

    //     // check if distance to player is less than stopping distance
    //     if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange)
    //     {
    //         if (!isAttacking)
    //         {
    //             agent.isStopped = true;
    //             StartCoroutine(Attack());
    //         }
    //     }
    //     else
    //     {
    //         agent.isStopped = false;
    //     }

    //     if (currentTarget.GetComponent<EnemyMovement>() && currentTarget.GetComponent<EnemyMovement>().isDead)
    //     {
    //         agent.ResetPath();
    //         hasCurrentTarget = false;
    //         enemyState = EnemyStates.Roam;
    //     }
    //     if (!hasCurrentTarget)
    //     {
    //         if ((int)Time.time % 2 == 0)
    //             ChooseEnemyInRange();
    //     }

    // }
    // private void Dead()
    // {
    //     animator.SetInteger("State", (int)EnemyStates.Dead);
    //     foreach (var icon in attackIcons)
    //     {
    //         icon.gameObject.SetActive(false);
    //     }
    //     //destroy enemy collision
    //     StopAllCoroutines();
    //     isDead = true;
    //     GetComponent<Collider>().enabled = false;
    //     agent.enabled = false;
    // }
    // private void Controlled()
    // {
    //     agent.SetDestination(controlledTarget);
    //     if (Vector3.Distance(controlledTarget, transform.position) <= 5)
    //     {
    //         enemyState = EnemyStates.Roam;
    //     }
    // }
    // private void ChooseEnemyInRange()
    // {
    //     currentTarget = maxTarget;
    //     foreach (GameObject target in targets)
    //     {
    //         if (target.GetComponent<EnemyMovement>())
    //         {
    //             if (Vector3.Distance(transform.position, target.transform.position)
    //                 <= Vector3.Distance(transform.position, currentTarget.transform.position)
    //                 && !target.GetComponent<EnemyMovement>().isDead)
    //             {
    //                 currentTarget = target;
    //             }

    //         }
    //         else if (target.GetComponent<PlayerMovement>())
    //         {
    //             if (Vector3.Distance(transform.position, target.transform.position)
    //                 <= Vector3.Distance(transform.position, currentTarget.transform.position))
    //                 currentTarget = target;
    //         }
    //     }
    //     enemyState = EnemyStates.Chase;
    // }

}


public interface ITargetable
{
    Transform transform { get; }
    GameObject gameObject { get; }
}