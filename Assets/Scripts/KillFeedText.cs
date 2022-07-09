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
    [SerializeField] int timeToFade = 3;
    [SerializeField] float fadeTimer = 0;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] Color bgImageEndColor;
    KillFeedManager kfManager;

    [SerializeField] Image bgImage;
    public string messageName;
    public string messageTag;
    [SerializeField] public TextMeshProUGUI feedTMP;
    private void Awake()
    {

        kfManager = GetComponentInParent<KillFeedManager>();
        feedTMP = GetComponentInChildren<TextMeshProUGUI>();
        bgImageEndColor = bgImage.color;
        bgImageEndColor.a = 0;

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
            feedTMP.color = Vector4.Lerp(startColor, endColor, fadeTimer / timeToFade);
            bgImage.color = Vector4.Lerp(bgImage.color, bgImageEndColor, fadeTimer / (timeToFade * 10) * Time.deltaTime);
            fadeTimer += Time.deltaTime;
        }
    }
    public void SetupFeedObject()
    {
        feedTMP.text = $"Bot {messageName} Died";
        if (messageTag == "Team1")
        {
            startColor = Color.blue;
            startColor.r = 0.7f;
            startColor.g = 0.7f;
            endColor = Color.blue;
            endColor.r = 0.7f;
            endColor.g = 0.7f;
            endColor.a = 0;
        }
        else
        {
            startColor = Color.red;
            startColor.b = 0.3f;
            startColor.g = 0.3f;
            endColor = Color.red;
            endColor.b = 0.3f;
            endColor.g = 0.3f;
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
