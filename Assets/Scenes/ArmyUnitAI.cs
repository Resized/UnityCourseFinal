using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArmyUnitAI : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    private GameObject currentTarget;
    private Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
