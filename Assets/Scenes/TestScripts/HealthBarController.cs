using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(HealthManager))]
public class HealthBarController : MonoBehaviour
{
    // Start is called before the first frame update
    HealthManager healthManager;
    public HealthBar healthBar;
    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
        healthBar = GetComponentInChildren<HealthBar>();
    }
    void Start()
    {
        healthBar.maxHealth = healthManager.maxHealth;
    }
    public void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar(healthManager.currentHealth);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
