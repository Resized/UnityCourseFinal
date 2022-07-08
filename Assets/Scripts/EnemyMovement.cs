using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject currentTarget;
    [SerializeField]
    private GameObject[] targets;
    private Animator animator;
    [SerializeField] private EnemyStates enemyState;
    private RaycastHit hit;
    private GameObject eyeHeight;
    [SerializeField] private int healthPoints;
    private bool isAttacking;
    public GameObject maxTarget;
    public enum EnemyStates
    {
        Idle,
        Roam,
        Chase,
        Dead
    }
    public float chaseRange = 1000;
    public float walkSpeed = 1;
    public float runSpeed = 2;

    [SerializeField] private bool isDead = false;
    private string enemyTeam = "";

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Team1")
        { 
            targets = GameObject.FindGameObjectsWithTag("Team2");
            enemyTeam = "Team2";
        }
        else { 
            targets = GameObject.FindGameObjectsWithTag("Team1");
            enemyTeam = "Team1";
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyState = EnemyStates.Idle;
        eyeHeight = transform.Find("EyeHeight").gameObject;
        healthPoints = 100;
        isAttacking = false;
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
        StopAllCoroutines();
        isDead = true;
        Destroy(GetComponent<Collider>());
        agent.enabled = false;
    }

    private void Chase()
    {
        agent.speed = runSpeed;
        agent.SetDestination(currentTarget.transform.position);
        animator.SetInteger("State", (int)EnemyStates.Chase);

        // check if distance to player is less than stopping distance
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= agent.stoppingDistance && !isAttacking)
        {
            if (currentTarget.GetComponent<EnemyMovement>())
                currentTarget.GetComponent<EnemyMovement>().Hit(25);
            if (currentTarget.GetComponent<PlayerMovement>())
                currentTarget.GetComponent<PlayerMovement>().Hit(25);
            StartCoroutine(Attack());
        }

        if (currentTarget.GetComponent<EnemyMovement>() && currentTarget.GetComponent<EnemyMovement>().isDead)
        {
            enemyState = EnemyStates.Roam;
        }
    }

    private void Idle()
    {
        animator.SetInteger("State", (int)EnemyStates.Idle);

        // if player is in range, change state to chase
        ChooseEnemyInRange();
        // every second decide if to change state to roam
        StartCoroutine(DecideIdleOrRoam());
    }
    private void Roam()
    {
        agent.speed = walkSpeed;
        animator.SetInteger("State", (int)EnemyStates.Roam);
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            enemyState = EnemyStates.Idle;
        }
        ChooseEnemyInRange();
    }

    private void ChooseEnemyInRange()
    {
        currentTarget = maxTarget;
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<EnemyMovement>())
            {
                if (Vector3.Distance(transform.position, target.transform.position) 
                    <= Vector3.Distance(transform.position, currentTarget.transform.position) 
                    && !target.GetComponent<EnemyMovement>().isDead)
                {
                    currentTarget = target;
                }

            }
            else if (target.GetComponent<PlayerMovement>())
            {
                if (Vector3.Distance(transform.position, target.transform.position)
                    <= Vector3.Distance(transform.position, currentTarget.transform.position))
                    currentTarget = target;
            }
        }
        enemyState = EnemyStates.Chase;
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
        if (agent.enabled)
            agent.isStopped = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.1f);
        if (agent.enabled)
            agent.isStopped = false;
        isAttacking = false;
    }
}
