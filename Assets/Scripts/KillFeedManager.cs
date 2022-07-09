using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillFeedManager : MonoBehaviour
{
    UIController uicontroller;
    [SerializeField] GameObject killTextPrefab;
    List<GameObject> kfMessages = new List<GameObject>();

    int space = 50;
    private void Awake()
    {
        uicontroller = GameObject.Find("UI").GetComponent<UIController>();
    }




    internal void UpdateKillFeed(string name, string tag)
    {
        GameObject killText = Instantiate(killTextPrefab, transform.position, Quaternion.identity, transform);
        TextMeshProUGUI killTMP = killText.GetComponentInChildren<TextMeshProUGUI>();
        killTMP.text = $"bot {name} Died";
        if (tag == "Team1")
        {
            killTMP.color = Color.blue;
        }
        else
        {
            killTMP.color = Color.red;

        }

    }
}
