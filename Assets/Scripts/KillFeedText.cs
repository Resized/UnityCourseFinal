using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class KillFeedText : MonoBehaviour
{
    // Start is called before the first frame update
    bool dying = false;
    int timeToFade = 3;
    float fadeTimer = 0;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    KillFeedManager kfManager;

    [SerializeField] Image bgImage;
    public string messageName;
    public string messageTag;
    [SerializeField] public TextMeshProUGUI feedTMP;
    private void Awake()
    {

        kfManager = GetComponentInParent<KillFeedManager>();
        feedTMP = GetComponentInChildren<TextMeshProUGUI>();

    }
    void Start()
    {
        SetupFeedObject();
    }


    // Update is called once per frame
    void Update()
    {
        if (dying)
        {
            feedTMP.color = Vector4.Lerp(startColor, endColor, fadeTimer / (timeToFade * 1.5f));
            bgImage.color = Vector4.Lerp(bgImage.color, Vector4.zero, fadeTimer / (timeToFade * 1.5f));
            fadeTimer += Time.deltaTime;
        }
    }
    public void SetupFeedObject()
    {
        feedTMP.text = $"Bot {messageName} Died";
        if (messageTag == "Team1")
        {
            startColor = Color.blue;
            endColor = Color.blue;
            endColor.a = 0;
        }
        else
        {
            startColor = Color.red;
            endColor = Color.red;
            endColor.a = 0;
        }

        StartFading();
    }
    void StartFading()
    {
        dying = true;
        StartCoroutine(Fade());
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeToFade);
        RemoveFromFeed();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    internal void RemoveFromFeed()
    {
        Destroy(gameObject);
        kfManager.kfMessages.Remove(this);
    }
}
