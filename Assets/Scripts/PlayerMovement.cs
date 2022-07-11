using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float healthPoints = 100;
    [SerializeField] private float maxHP = 100;
    public float HealthPoints => healthPoints;
    public CharacterController controller;

    public Image hpBar;
    [SerializeField] public GameObject catapultTarget;
    TeamController teamController;
    [SerializeField] public List<GameObject> myTeam = new List<GameObject>();
    List<GameObject> mySoldiers = new List<GameObject>();
    List<GameObject> myArchers = new List<GameObject>();
    List<GameObject> myGrenardiers = new List<GameObject>();
    List<GameObject> myCatapults = new List<GameObject>();

    GameObject[] lastTargets;
    [SerializeField] Sprite[] skills;

    EnemyMovement lastTarget;
    public float speed = 12f;
    RaycastHit hit;

    public Vector3 velocity;

    private void Awake()
    {
        teamController = FindObjectOfType<TeamController>();
        lastTargets = new GameObject[skills.Length];
    }

    // Start is called before the first frame update
    private void Start()
    {
        maxHP = healthPoints;
    }

    public void SetupTeammates()
    {

        if (tag == "Attackers")
        {
            myTeam = teamController.Attackers;
            mySoldiers = teamController.AttackerSoldiers;
            myArchers = teamController.AttackerArchers;
            myCatapults = teamController.AttackerCatapults;
        }
        else
        {
            myTeam = teamController.Defenders;
            mySoldiers = teamController.DefenderSoldiers;
            myArchers = teamController.DefenderArchers;
            myGrenardiers = teamController.DefenderGrenardiers;
        }
        myTeam.Remove(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed);

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoldiersAttack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ArchersAttack();
        }
        if (tag == "Attackers")
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CatapultAttack();

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GrenardierAttack();
            }
        }
        updateHPBar();
    }
    Transform enemyTarget;
    Vector3 attackTargetPosition;

    void updateHPBar()
    {
        hpBar.fillAmount = healthPoints / maxHP;
    }
    void GetRaycast(int skillUsed)
    {
        enemyTarget = null;
        validTarget = false;
        attackTargetPosition = Vector3.zero;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            EnemyMovement enemy = hit.transform.GetComponent<EnemyMovement>();
            if (enemy)
            {
                if (enemy.tag == tag)
                {
                    validTarget = false;
                    return;
                }
                enemyTarget = enemy.transform;
                if (lastTargets[skillUsed - 1])
                {

                    if (enemy.gameObject != lastTargets[skillUsed - 1])
                    {
                        enemy.SetTargeted(skills[skillUsed - 1]);
                        lastTargets[skillUsed - 1].GetComponent<EnemyMovement>().RemoveTargeted();
                        lastTargets[skillUsed - 1] = enemy.gameObject;

                    }
                }
                else
                {
                    enemy.SetTargeted(skills[skillUsed - 1]);
                    lastTargets[skillUsed - 1] = enemy.gameObject;
                }
                validTarget = true;
            }
            else
            {
                attackTargetPosition = hit.point;
                validTarget = false;
            };

        }
    }
    bool validTarget = false;
    public void SoldiersAttack()
    {
        GetRaycast(1);

        if (validTarget)
        {
            print("Attacking an enemy: " + enemyTarget.name);
            mySoldiers.ForEach(soldier =>
            {
                EnemyMovement soldierController = soldier.GetComponent<EnemyMovement>();
                soldierController.SetCurrentTarget(enemyTarget.gameObject);
            });
        }
        else
        {
            print("Going to a point " + attackTargetPosition);
            mySoldiers.ForEach(soldier =>
            {
                EnemyMovement soldierController = soldier.GetComponent<EnemyMovement>();
                soldierController.SetControlledTarget(attackTargetPosition);
            });
        }
    }




    void ArchersAttack()
    {
        GetRaycast(2);

        if (validTarget)
        {
            print("Attacking an enemy: " + enemyTarget.name);
            myArchers.ForEach(archer =>
            {
                EnemyMovement archerController = archer.GetComponent<EnemyMovement>();
                archerController.SetCurrentTarget(enemyTarget.gameObject);
            });
        }
        else
        {
            print("Going to a point " + attackTargetPosition);
            myArchers.ForEach(archer =>
            {
                EnemyMovement archerController = archer.GetComponent<EnemyMovement>();
                archerController.SetControlledTarget(attackTargetPosition);
            });
        }
    }

    void GrenardierAttack()
    {
        GetRaycast(3);

        if (validTarget)
        {
            print("Attacking an enemy: " + enemyTarget.name);
            myGrenardiers.ForEach(grenardier =>
            {
                EnemyMovement grenardierController = grenardier.GetComponent<EnemyMovement>();
                grenardierController.SetCurrentTarget(enemyTarget.gameObject);
            });
        }
        else
        {
            print("Going to a point " + attackTargetPosition);
            myGrenardiers.ForEach(grenardier =>
            {
                EnemyMovement grenardierController = grenardier.GetComponent<EnemyMovement>();
                grenardierController.SetControlledTarget(attackTargetPosition);
            });
        }
    }
    void CatapultAttack()
    {

        GetRaycast(4);
        catapultTarget.transform.position = hit.point;
        myCatapults.ForEach(cata =>
        {
            print("TEST2");
            print(cata.name);
            CatapultController cataController = cata.GetComponent<CatapultController>();
            cataController.isControlled = true;
            cataController.SetControlledTarget(catapultTarget);
        });
    }

    internal void Hit(int hitAmount)
    {
        healthPoints -= hitAmount;
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // throw new NotImplementedException();
    }
}