using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private EnemyStates enemyState;
    private RaycastHit hit;
    private GameObject eyeHeight;
    private int healthPoints;
    private bool isAttacking;
    public enum EnemyStates
    {
        Idle,
        Roam,
        Chase,
        Dead
    }
    public float chaseRange = 1000;
    public float walkSpeed = 4;
    public float runSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyState = EnemyStates.Idle;
        eyeHeight = transform.Find("EyeHeight").gameObject;
        healthPoints = 100;
        isAttacking = false;
    }
    
    public void Hit(int hitAmount)
    {
        healthPoints -= hitAmount;
        if (healthPoints <= 0)
        {
            enemyState = EnemyStates.Dead;
            return;
        }
        StartCoroutine(Hit());
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyStates.Idle:
                Idle();
                break;
            case EnemyStates.Roam:
                Roam();
                break;
            case EnemyStates.Chase:
                Chase();
                break;
            case EnemyStates.Dead:
                Dead();
                break;
            default:
                break;
        }
    }

    private void Dead()
    {
        animator.SetInteger("State", (int)EnemyStates.Dead);
        //destroy enemy collision
        Destroy(GetComponent<Collider>());
        agent.enabled = false;
    }

    private void Chase()
    {
        agent.speed = runSpeed;
        agent.SetDestination(player.position);
        animator.SetInteger("State", (int)EnemyStates.Chase);

        // check if distance to player is less than stopping distance
        if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        
    }

    private void Roam()
    {
        agent.speed = walkSpeed;
        animator.SetInteger("State", (int)EnemyStates.Roam);
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            enemyState = EnemyStates.Idle;
        }
        IsPlayerInRange();
    }

    private void Idle()
    {
        animator.SetInteger("State", (int)EnemyStates.Idle);

        // if player is in range, change state to chase
        IsPlayerInRange();
        // every second decide if to change state to roam
        StartCoroutine(DecideIdleOrRoam());
    }

    private void IsPlayerInRange()
    {
        if (Physics.Raycast(eyeHeight.transform.position, player.position - eyeHeight.transform.position, out hit, chaseRange))
        {
            if (hit.collider.tag == "Player")
            {
                enemyState = EnemyStates.Chase;
            }
        }
    }

    private Vector3 RandomNavmeshLocation(Vector3 origin, float dist)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, dist, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private IEnumerator DecideIdleOrRoam()
    {
        yield return new WaitForSeconds(1);
        if (UnityEngine.Random.Range(0, 100) < 20)
        {
            enemyState = EnemyStates.Roam;
            agent.SetDestination(RandomNavmeshLocation(transform.position, 30));
        }
    }

    IEnumerator Hit()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(1);
        agent.isStopped = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.1f);
        agent.isStopped = false;
        isAttacking = false;
    }
}
