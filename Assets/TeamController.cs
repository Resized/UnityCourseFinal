using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    private List<GameObject> attackers;
    private List<GameObject> defenders;

    public List<GameObject> Attackers { get => attackers; set => attackers = value; }
    public List<GameObject> Defenders { get => defenders; set => defenders = value; }


    private void Awake()
    {
        Attackers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Attackers"));
        Defenders = new List<GameObject>(GameObject.FindGameObjectsWithTag("Defenders"));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
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
