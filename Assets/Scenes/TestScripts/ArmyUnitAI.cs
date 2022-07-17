using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Enemy))]
public class ArmyUnitAI : MonoBehaviour
{
    [SerializeField] AIStates AIState = AIStates.Idle;
    NavMeshAgent agent;
    Animator animator;
    Vector3 startingPosition;
    Vector3 roamingDestination = Vector3.zero;
    [SerializeField] Enemy enemy;
    IHittable currentTarget;
    [SerializeField] GameObject targetGameObject;
    bool IsAttacking = false;
    bool IsIdle = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        startingPosition = transform.position;
        agent.stoppingDistance = enemy.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.isAlive)
        {
            SwitchState(AIStates.Dead);
        }

        switch (AIState)
        {
            case AIStates.Idle:
                Idle();
                SearchTargetsInRange();
                break;
            case AIStates.Roam:
                Roam();
                SearchTargetsInRange();
                break;
            case AIStates.Chase:
                Chase();
                break;
            case AIStates.Attack:
                Attack();
                break;
            case AIStates.Dead:
                Dead();
                break;
            case AIStates.Controlled:
                Controlled();
                break;
            default:
                break;
        }


    }



    private void Roam()
    {
        if (roamingDestination == Vector3.zero)
        {
            agent.speed = 1;
            animator.SetInteger("State", (int)AIStates.Roam);
            roamingDestination = GetPointInRange(enemy.visionRange);
            agent.SetDestination(roamingDestination);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            SwitchState(AIStates.Idle);
            roamingDestination = Vector3.zero;
        }


    }
    private void Idle()
    {
        if (!IsIdle)
        {
            StartCoroutine(IdleForSeconds(5));
        }
    }
    IEnumerator IdleForSeconds(float seconds)
    {
        IsIdle = true;
        animator.SetInteger("State", (int)AIStates.Idle);
        yield return new WaitForSeconds(seconds);
        SwitchState(AIStates.Roam);
        IsIdle = false;
    }

    Vector3 GetPointInRange(float range)
    {
        float pointX = Random.Range(10, range) * (Random.Range(0, 2) * 2 - 1);
        float pointY = 0;
        float pointZ = Random.Range(10, range) * (Random.Range(0, 2) * 2 - 1);
        var test = new Vector3(pointX, pointY, pointZ);
        test = startingPosition + test;
        return test;

    }

    void SearchTargetsInRange()
    {
        Collider[] collidersInRange;
        collidersInRange = Physics.OverlapSphere(transform.position, enemy.visionRange);
        IHittable closestEnemy = null;
        if (collidersInRange.Length != 0)
        {
            foreach (var collider in collidersInRange)
            {
                if (collider.gameObject == gameObject)
                {
                    continue;
                }
                IHittable target;
                if (!collider.gameObject.TryGetComponent<IHittable>(out target))
                {

                    continue;
                }

                if (target._healthManager.IsDead)
                {

                    continue;
                }
                if (closestEnemy == null)
                {

                    closestEnemy = target;
                    continue;
                }
                if (GetDistanceToTarget(closestEnemy) > GetDistanceToTarget(target))
                {
                    closestEnemy = target;
                }

            }
            if (closestEnemy != null)
            {

                currentTarget = closestEnemy;
                targetGameObject = currentTarget.gameObject;

                StartChasing();
            }

        }

    }
    float? GetDistanceToTarget(ITargetable target)
    {
        if (target != null)
        {

            return Vector3.Distance(transform.position, target.transform.position);
        }
        return null;
    }



    void SwitchState(AIStates state)
    {
        AIState = state;
    }
    void StartChasing()
    {

        agent.speed = 2;
        animator.SetInteger("State", 2);
        SwitchState(AIStates.Chase);

    }
    private void Chase()
    {
        agent.SetDestination(currentTarget.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            print("test");
            SwitchState(AIStates.Attack);
        }
    }


    private void Attack()
    {
        if (currentTarget._healthManager.IsDead)
        {
            SwitchState(AIStates.Idle);
            currentTarget = null;
            return;
        }
        if (Vector3.Distance(transform.position, currentTarget.transform.position) > enemy.attackRange && !enemy.isAttacking)
        {
            StartChasing();
            return;
        }
        agent.transform.LookAt(currentTarget.transform);
        agent.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        if (!enemy.isAttacking)
        {
            animator.SetTrigger("Attack");
            enemy.Attack(currentTarget);
        }
        animator.SetInteger("State", 0);

    }


    private void Dead()
    {
        animator.SetInteger("State", 3);
        StopAllCoroutines();
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
    }
    private void Controlled()
    {

    }




}
public enum AIStates
{
    Idle,
    Roam,
    Chase,
    Dead,
    Controlled,
    Attack
}