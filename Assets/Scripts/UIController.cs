using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject UIContainer;
    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI lossText;
    [SerializeField] PlayerMovement player;


    [SerializeField] public List<EnemyMovement> enemies;
    [SerializeField] public List<EnemyMovement> friendlies;
    [SerializeField] public GameObject restartBtn;
    [SerializeField] EnemyMovement[] e;
    KillFeedManager kfManager;
    private void Awake()
    {
        kfManager = GetComponentInChildren<KillFeedManager>();
        victoryText = UIContainer.transform.Find("WinMessage").GetComponent<TextMeshProUGUI>();
        lossText = UIContainer.transform.Find("LostMessage").GetComponent<TextMeshProUGUI>();
        restartBtn = UIContainer.transform.Find("RestartBtn").gameObject;
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
        CheckLoss();
        CheckWin();
    }
    public void CheckLoss()
    {
        if (player.HealthPoints <= 0)
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

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
}
