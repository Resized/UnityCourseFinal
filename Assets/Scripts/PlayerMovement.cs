using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int healthPoints = 100;
    public int HealthPoints => healthPoints;
    public CharacterController controller;

    TeamController teamController;
    [SerializeField] List<GameObject> myTeam = new List<GameObject>();
    List<GameObject> mySoldiers = new List<GameObject>();
    List<GameObject> myArchers = new List<GameObject>();
    List<GameObject> myGrenardiers = new List<GameObject>();
    List<GameObject> myCatapults = new List<GameObject>();
    public float speed = 12f;
    RaycastHit hit;

    public Vector3 velocity;

    private void Awake()
    {
        teamController = FindObjectOfType<TeamController>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    public void SetupTeammates()
    {

        if (tag == "Attackers")
        {
            myTeam = teamController.Attackers;
            mySoldiers = teamController.AttackerSoldiers;
            myArchers = teamController.AttackerArchers;
        }
        else
        {
            myTeam = teamController.Defenders;
            mySoldiers = teamController.DefenderSoldiers;
            myArchers = teamController.DefenderArchers;
            myGrenardiers = teamController.DefenderGrenardiers;
        }
        myTeam.Remove(
        myTeam.Find(enemy => enemy.GetComponent<PlayerMovement>())
        );
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
    }
    Transform enemyTarget;
    Vector3 attackTargetPosition;
    void GetRaycast()
    {
        enemyTarget = null;
        validTarget = false;
        attackTargetPosition = Vector3.zero;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.red, 10);
            EnemyMovement enemy = hit.transform.GetComponent<EnemyMovement>();
            if (enemy)
            {
                if (enemy.tag == tag)
                {
                    validTarget = false;
                    return;
                }
                enemyTarget = enemy.transform;
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
        GetRaycast();

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
        GetRaycast();

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
        GetRaycast();

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