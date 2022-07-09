using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    UIController uicontroller;
    private NavMeshAgent agent;
    [SerializeField] private GameObject currentTarget;
    [SerializeField]
    private GameObject[] targets;
    private Animator animator;
    [SerializeField] private EnemyStates enemyState;
    private RaycastHit hit;
    private GameObject eyeHeight;
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject arrowSpawnPoint;
    [SerializeField] private float attackCooldown;
    [SerializeField] private EnemyType myEnemyType;
    public int HealthPoints => healthPoints;
    private bool isAttacking;
    public GameObject maxTarget;
    public float chaseRange = 1000;
    public float walkSpeed = 1;
    public float runSpeed = 2;
    private enum EnemyType
    {
        Archer,
        Soldier,
        Bombardier
    }
    public enum EnemyStates
    {
        Idle,
        Roam,
        Chase,
        Dead
    }


    [SerializeField] private bool isDead = false;
    private string enemyTeam = "";

    private void Awake()
    {
        uicontroller = FindObjectOfType<UIController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckMyTeam();

        switch (myEnemyType)
        {
            case EnemyType.Archer:
                attackCooldown = 1.5f;
                break;
            case EnemyType.Soldier:
                attackCooldown = 1.1f;
                break;
            case EnemyType.Bombardier:
                attackCooldown = 3;
                break;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyState = EnemyStates.Idle;
        eyeHeight = transform.Find("EyeHeight").gameObject;
        healthPoints = UnityEngine.Random.Range(100, 200);
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
    private void CheckMyTeam()
    {
        if (gameObject.tag == "Team1")
        {
            var meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mesh in meshes)
            {
                var c = Color.red;
                mesh.material.color = new Color(0.5f, 0.5f, 1, 1);
            }
            targets = GameObject.FindGameObjectsWithTag("Team2");
            enemyTeam = "Team2";
        }
        else
        {
            var meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mesh in meshes)
            {

                mesh.material.color = new Color(1, 0.5f, 0.5f, 1);
            }
            targets = GameObject.FindGameObjectsWithTag("Team1");

            enemyTeam = "Team1";
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

    private void Chase()
    {
        agent.speed = runSpeed;
        agent.SetDestination(currentTarget.transform.position);
        animator.SetInteger("State", (int)EnemyStates.Chase);

        // check if distance to player is less than stopping distance
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= agent.stoppingDistance && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (currentTarget.GetComponent<EnemyMovement>() && currentTarget.GetComponent<EnemyMovement>().isDead)
        {
            enemyState = EnemyStates.Roam;
        }

        if ((int)Time.time % 2 == 0)
            ChooseEnemyInRange();
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
        if (enemyState == EnemyStates.Dead)
        {
            return;
        }
        healthPoints -= hitAmount;
        if (healthPoints <= 0)
        {
            enemyState = EnemyStates.Dead;
            uicontroller.EnemyDied(this);
            return;
        }
        StartCoroutine(Hit());
    }

    internal GameObject GetTarget()
    {
        return currentTarget;
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
        transform.LookAt(currentTarget.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackCooldown);

        if (myEnemyType == EnemyType.Archer)
        {
            Projectile arrow = Instantiate(projectile, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation).GetComponent<Projectile>();
            arrow.SetTarget(currentTarget, gameObject);
            arrow.gameObject.SetActive(true);
        }
        else if (myEnemyType == EnemyType.Soldier)
        {
            if (currentTarget.GetComponent<EnemyMovement>())
                currentTarget.GetComponent<EnemyMovement>().Hit(25);
            if (currentTarget.GetComponent<PlayerMovement>())
                currentTarget.GetComponent<PlayerMovement>().Hit(25);
        }
        else if (myEnemyType == EnemyType.Bombardier)
        {
            Instantiate(projectile, arrowSpawnPoint.transform.position, arrowSpawnPoint.transform.rotation, arrowSpawnPoint.transform);
        }

        if (agent.enabled)
            agent.isStopped = false;
        isAttacking = false;
    }
}
