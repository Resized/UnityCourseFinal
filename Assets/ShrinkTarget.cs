using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrinkTarget : MonoBehaviour
{
    // Start is called before the first frame updat
    Image targetImg;
    Vector3 startingScale;
    float timer = 3f;
    float timerTime = 0;
    void Start()
    {
        targetImg = GetComponentInChildren<Image>();
        startingScale = targetImg.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetImg.transform.localScale == Vector3.zero)
        {
            timerTime = 0;
            targetImg.transform.localScale = startingScale;
        }
        timerTime += Time.deltaTime;
        targetImg.transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, timerTime);
    }
}
