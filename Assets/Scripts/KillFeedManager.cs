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
    [SerializeField] public List<KillFeedText> kfMessages = new List<KillFeedText>();


    private void Awake()
    {
        uicontroller = GameObject.Find("UI").GetComponent<UIController>();
    }
    internal void UpdateKillFeed(string botName, string botTag)
    {
        if (kfMessages.Count >= 5)
        {
            kfMessages[0].RemoveFromFeed();
            kfMessages.RemoveAt(0);
        }
        GameObject feedObject = Instantiate(killTextPrefab, transform);
        KillFeedText feedText = feedObject.GetComponent<KillFeedText>();
        feedText.messageTag = botTag;
        feedText.messageName = botName;
        kfMessages.Add(feedText);
        feedObject.transform.SetSiblingIndex(kfMessages.Count);

    }


}
