using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyMovement : MonoBehaviour
{

    UIController uicontroller;
    private NavMeshAgent agent;
    [SerializeField] private GameObject currentTarget;
    [SerializeField]
    private List<GameObject> targets;
    [SerializeField] private bool isDead = false;
    private string enemyTeam;
    private Animator animator;
    [SerializeField] private EnemyStates enemyState;
    private RaycastHit hit;
    private GameObject eyeHeight;
    [SerializeField] private int healthPoints;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float windUpTime;
    [SerializeField] public EnemyType myEnemyType;
    [SerializeField] private GameObject maxTarget;
    [SerializeField] public Vector3 controlledTarget;
    private TeamController teamController;
    public int HealthPoints => healthPoints;
    private bool hasCurrentTarget = false;

    [SerializeField] Image[] attackIcons;
    [SerializeField] Sprite currentSprite = null;
    public List<GameObject> Targets { get => targets; set => targets = value; }
    Color spriteColor;
    bool isTargeted = false;
    [SerializeField]private bool isAttacking;
    public float chaseRange = 1000;
    public float walkSpeed = 1;
    public float runSpeed = 2;
    private float attackRange;
    public enum EnemyType
    {
        Archer,
        Soldier,
        Grenadier
    }
    public enum EnemyStates
    {
        Idle,
        Roam,
        Chase,
        Dead,
        Controlled
    }


    private void Awake()
    {
        uicontroller = FindObjectOfType<UIController>();
        maxTarget = GameObject.FindGameObjectWithTag("MaxTarget");
        teamController = FindObjectOfType<TeamController>();
        attackIcons = GetComponentsInChildren<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
        CheckMyTeam();

        switch (myEnemyType)
        {
            case EnemyType.Archer:
                attackCooldown = 0.6139f;
                windUpTime = 1.1191f;
                attackRange = 20f;
                break;
            case EnemyType.Soldier:
                attackCooldown = 1.1f;
                attackRange = 3f;
                break;
            case EnemyType.Grenadier:
                attackCooldown = 2.6503f;
                windUpTime = 2.2167f;
                attackRange = 30f;
                break;
        }

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyState = EnemyStates.Idle;
        eyeHeight = transform.Find("EyeHeight").gameObject;
        healthPoints = UnityEngine.Random.Range(300, 600);
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
            case EnemyStates.Controlled:
                Controlled();
                break;
            default:
                break;
        }
        
    }
    public void SetIconOnTarget(TeamController.TargetIconsEnum icon)
    {
        attackIcons[(int)icon].sprite = teamController.skills[(int)icon];
        attackIcons[(int)icon].color = spriteColor;
        attackIcons[(int)icon].gameObject.SetActive(true);
    }
    public void RemoveTargeted(TeamController.TargetIconsEnum icon)
    {
        attackIcons[(int)icon].sprite = null;
        attackIcons[(int)icon].color = new Color(0, 0, 0, 0);
        attackIcons[(int)icon].gameObject.SetActive(false);
    }
    private void Controlled()
    {
        agent.SetDestination(controlledTarget);
        if (Vector3.Distance(controlledTarget, transform.position) <= 5)
        {
            enemyState = EnemyStates.Roam;
        }
    }
    public void SetControlledTarget(Vector3 target)
    {
        if (enemyState != EnemyStates.Dead)
        {
            target.y = 0;
            controlledTarget = target;
            enemyState = EnemyStates.Controlled;
        }
    }

    public void SetCurrentTarget(GameObject target)
    {
        if (enemyState != EnemyStates.Dead)
        {
            hasCurrentTarget = true;
            currentTarget = target;
            enemyState = EnemyStates.Chase;
        }
    }
    private void CheckMyTeam()
    {
        if (gameObject.tag == "Attackers")
        {
            var meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.material.color = new Color(1, 0.5f, 0.5f, 1);
            }
            spriteColor = new Color(0.5f, 0.5f, 1, 1);
            targets = teamController.Defenders;
            targets.Remove(gameObject);
            enemyTeam = "Defenders";

        }
        else if (gameObject.tag == "Defenders")
        {
            var meshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.material.color = new Color(0.5f, 0.5f, 1, 1);
            }
            spriteColor = new Color(1, 0.5f, 0.5f, 1);
            targets = teamController.Attackers;
            targets.Remove(gameObject);
            enemyTeam = "Attackers";

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
        animator.SetInteger("State", (int)EnemyStates.Chase);
        agent.SetDestination(currentTarget.transform.position);

        // check if distance to player is less than stopping distance
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange)
        {
            if (!isAttacking)
            {
                agent.isStopped = true;
                StartCoroutine(Attack());
            }
        }
        else
        {
            agent.isStopped = false;
        }

        if (currentTarget.GetComponent<EnemyMovement>() && currentTarget.GetComponent<EnemyMovement>().isDead)
        {
            agent.ResetPath();
            hasCurrentTarget = false;
            enemyState = EnemyStates.Roam;
        }
        if (!hasCurrentTarget)
        {
            if ((int)Time.time % 2 == 0)
                ChooseEnemyInRange();
        }

    }
    private void Dead()
    {
        animator.SetInteger("State", (int)EnemyStates.Dead);
        foreach (var icon in attackIcons)
        {
            icon.gameObject.SetActive(false);
        }
        //destroy enemy collision
        StopAllCoroutines();
        isDead = true;
        GetComponent<Collider>().enabled = false;
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

    public bool IsDead()
    {
        return isDead;
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
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1);
        if (agent.enabled)
            agent.isStopped = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        transform.LookAt(currentTarget.transform);

        Vector3 relativePos = currentTarget.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        animator.SetTrigger("Attack");

        if (myEnemyType == EnemyType.Archer)
        {
            yield return new WaitForSeconds(windUpTime);
            Projectile arrow = Instantiate(projectile, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation).GetComponent<Projectile>();
            arrow.SetTarget(currentTarget, gameObject);
            arrow.gameObject.SetActive(true);
            yield return new WaitForSeconds(attackCooldown);
        }
        else if (myEnemyType == EnemyType.Soldier)
        {
            yield return new WaitForSeconds(attackCooldown);
            if (currentTarget.GetComponent<EnemyMovement>())
                currentTarget.GetComponent<EnemyMovement>().Hit(25);
            if (currentTarget.GetComponent<PlayerMovement>())
                currentTarget.GetComponent<PlayerMovement>().Hit(25);
        }
        else if (myEnemyType == EnemyType.Grenadier)
        {
            yield return new WaitForSeconds(windUpTime);
            Projectile bomb = Instantiate(projectile, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation, projectileSpawnPoint.transform).GetComponent<Projectile>();
            bomb.SetTarget(currentTarget, gameObject);
            bomb.gameObject.SetActive(true);
            yield return new WaitForSeconds(attackCooldown);
        }

        if (agent.enabled)
            agent.isStopped = false;
        isAttacking = false;
    }
}
