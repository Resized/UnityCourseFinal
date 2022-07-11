using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject UIContainer;
    [SerializeField] TextMeshProUGUI chooseTeamText;
    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI lossText;
    [SerializeField] GameObject player;
    private TeamController teamController;


    [SerializeField] public List<EnemyMovement> enemies;
    [SerializeField] public List<EnemyMovement> friendlies;
    [SerializeField] public GameObject restartBtn;
    [SerializeField] public GameObject chooseAttackersBtn;
    [SerializeField] public GameObject chooseDefendersBtn;
    public Transform attackersSpawnPoint;
    public Transform defendersSpawnPoint;

    [SerializeField] EnemyMovement[] e;
    KillFeedManager kfManager;
    private bool hasPlayerChosenTeam = false;
    private void Awake()
    {
        kfManager = GetComponentInChildren<KillFeedManager>();
        chooseTeamText = UIContainer.transform.Find("ChooseTeamMessage").GetComponent<TextMeshProUGUI>();
        victoryText = UIContainer.transform.Find("WinMessage").GetComponent<TextMeshProUGUI>();
        lossText = UIContainer.transform.Find("LostMessage").GetComponent<TextMeshProUGUI>();
        restartBtn = UIContainer.transform.Find("RestartBtn").gameObject;
        chooseAttackersBtn = UIContainer.transform.Find("ChooseAttackersBtn").gameObject;
        chooseDefendersBtn = UIContainer.transform.Find("ChooseDefendersBtn").gameObject;
        teamController = FindObjectOfType<TeamController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        var playerTeam = player.tag;
        var enemyGameObjects = GameObject.FindObjectsOfType<EnemyMovement>();
        e = GameObject.FindObjectsOfType<EnemyMovement>();
        foreach (var enemyGO in enemyGameObjects)
        {
            if (enemyGO.tag != playerTeam)
                enemies.Add(enemyGO.GetComponent<EnemyMovement>());
            else
                friendlies.Add(enemyGO.GetComponent<EnemyMovement>());
        }

    }
    public void EnemyDied(EnemyMovement enemy)
    {
        RemoveEnemyMovement(enemy);
        kfManager.UpdateKillFeed(enemy.name, enemy.tag);

    }
    void RemoveEnemyMovement(EnemyMovement enemy)
    {
        if (enemy.tag == player.tag)
        {
            friendlies.Remove(enemy);
        }
        else if (enemy.tag != player.tag)
        {
            enemies.Remove(enemy);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!hasPlayerChosenTeam)
        {
            ChooseTeam();
        }
        CheckLoss();
        CheckWin();
    }

    private void ChooseTeam()
    {
        StopGame();
        Cursor.lockState = CursorLockMode.Confined;
        UIContainer.transform.Find("MessageBackground").gameObject.SetActive(true);
        chooseTeamText.gameObject.SetActive(true);
        chooseAttackersBtn.gameObject.SetActive(true);
        chooseDefendersBtn.gameObject.SetActive(true);
    }

    public void CheckLoss()
    {
        if (player.GetComponent<PlayerMovement>().HealthPoints <= 0)
        {
            Loss();
        }
    }
    
    public void CheckWin()
    {
        if (enemies.Count == 0)
        {
            Victory();
            return;
        }

    }
    void Loss()
    {
        StopGame();
        Cursor.lockState = CursorLockMode.Confined;
        UIContainer.transform.Find("MessageBackground").gameObject.SetActive(true);
        lossText.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
    }

    void Victory()
    {
        StopGame();
        Cursor.lockState = CursorLockMode.Confined;
        UIContainer.transform.Find("MessageBackground").gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
    }

    void StopGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void ChooseAttackers()
    {
        player.tag = "Attackers";
        teamController.AddObjectToAttackers(player);
        hasPlayerChosenTeam = true;
        player.GetComponent<CharacterController>().enabled = false;
        UIContainer.transform.Find("MessageBackground").gameObject.SetActive(false);
        chooseTeamText.gameObject.SetActive(false);
        chooseAttackersBtn.gameObject.SetActive(false);
        chooseDefendersBtn.gameObject.SetActive(false);
        player.transform.position = attackersSpawnPoint.position;
        player.transform.rotation = attackersSpawnPoint.rotation;
        player.GetComponent<CharacterController>().enabled = true;
        ResumeGame();
    }

    public void ChooseDefenders()
    {
        player.tag = "Defenders";
        teamController.AddObjectToDefenders(player);
        hasPlayerChosenTeam = true;
        player.GetComponent<CharacterController>().enabled = false;
        UIContainer.transform.Find("MessageBackground").gameObject.SetActive(false);
        chooseTeamText.gameObject.SetActive(false);
        chooseAttackersBtn.gameObject.SetActive(false);
        chooseDefendersBtn.gameObject.SetActive(false);
        player.transform.position = defendersSpawnPoint.position;
        player.transform.rotation = defendersSpawnPoint.rotation;
        Debug.Log("Moved player to " + defendersSpawnPoint.position);
        player.GetComponent<CharacterController>().enabled = true;
        ResumeGame();
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

    private void LateUpdate()
    {
        
    }
}
