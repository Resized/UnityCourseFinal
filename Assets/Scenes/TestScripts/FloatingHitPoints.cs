using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingHitPoints : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject textContainer;
    public TextMeshProUGUI damageText;
    float fadeOutTime = 2;
    float fadeTimer = 0;
    Color startColor;
    private void Awake()
    {

        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        startColor = damageText.color;
        transform.position = new Vector3(transform.position.x + Random.Range(0.0f, 0.2f), transform.position.y + Random.Range(0.0f, 0.2f), transform.position.z + Random.Range(0.0f, 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        fadeTimer += Time.deltaTime;
        if (fadeTimer <= fadeOutTime)
        {
            startColor.a -= 0.4f * Time.deltaTime;
            damageText.color = startColor;
            // text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), fadeTimer / fadeOutTime);
            transform.position += new Vector3(0, 0.15f, 0) * Time.deltaTime * 2;

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
