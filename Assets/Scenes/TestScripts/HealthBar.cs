using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform healthFill;
    public Image barImage;
    public float maxHealth;

    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {

        healthFill = transform.Find("HPBarCanvas").Find("HealthFill");
        barImage = healthFill.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealthBar(float health)
    {
        barImage.fillAmount = health / maxHealth;
        barImage.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
    }
}
