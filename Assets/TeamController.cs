using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField] private List<GameObject> attackers;
    [SerializeField] private List<GameObject> defenders;
    [SerializeField] private List<GameObject> attackerSoldiers;
    [SerializeField] private List<GameObject> defenderSoldiers;
    [SerializeField] private List<GameObject> attackerArchers;
    [SerializeField] private List<GameObject> defenderArchers;

    [SerializeField] private List<GameObject> defenderGrenardiers;
    [SerializeField] private List<GameObject> attackerCatapults;


    [SerializeField] public List<GameObject> Attackers { get => attackers; set => attackers = value; }
    public List<GameObject> Defenders { get => defenders; set => defenders = value; }
    public List<GameObject> AttackerSoldiers { get => attackerSoldiers; set => attackerSoldiers = value; }
    public List<GameObject> AttackerArchers { get => attackerArchers; set => attackerArchers = value; }

    public List<GameObject> AttackerCatapults { get => attackerCatapults; set => attackerCatapults = value; }
    public List<GameObject> DefenderSoldiers { get => defenderSoldiers; set => defenderSoldiers = value; }
    public List<GameObject> DefenderArchers { get => defenderArchers; set => defenderArchers = value; }
    public List<GameObject> DefenderGrenardiers { get => defenderGrenardiers; set => defenderGrenardiers = value; }

    [SerializeField] public Sprite[] skills;

    public enum TargetIconsEnum: int
    {
        Soldier = 0,
        Archer,
        Grenadier,
        Catapult
    }




    private void Awake()
    {
        Attackers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Attackers"));
        Defenders = new List<GameObject>(GameObject.FindGameObjectsWithTag("Defenders"));
        SetupTeams();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetupTeams()
    {
        foreach (GameObject defender in defenders)
        {
            EnemyMovement defenderEnemy = defender.GetComponent<EnemyMovement>();
            switch (defenderEnemy.myEnemyType)
            {
                case EnemyMovement.EnemyType.Soldier:
                    defenderSoldiers.Add(defenderEnemy.gameObject);
                    break;
                case EnemyMovement.EnemyType.Archer:
                    defenderArchers.Add(defenderEnemy.gameObject);
                    break;
                case EnemyMovement.EnemyType.Grenadier:
                    defenderGrenardiers.Add(defenderEnemy.gameObject);
                    break;
            }
        }
        foreach (GameObject attacker in attackers)
        {
            EnemyMovement attackerEnemy = attacker.GetComponent<EnemyMovement>();
            switch (attackerEnemy.myEnemyType)
            {
                case EnemyMovement.EnemyType.Soldier:
                    attackerSoldiers.Add(attackerEnemy.gameObject);
                    break;
                case EnemyMovement.EnemyType.Archer:
                    attackerArchers.Add(attackerEnemy.gameObject);
                    break;

            }
        }
        var k = FindObjectsOfType<CatapultController>();
        foreach (var item in k)
        {
            attackerCatapults.Add(item.gameObject);
        }

    }
    public void AddObjectToAttackers(GameObject obj)
    {
        Attackers.Add(obj);
    }
    public void AddObjectToDefenders(GameObject obj)
    {
        Defenders.Add(obj);
    }
}
