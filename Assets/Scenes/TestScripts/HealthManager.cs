using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool IsDead = false;
    public GameObject floatingDamageCanvas;
    public GameObject damageText;

    private HealthBarController healthBar;

    public float healthPoints { get; set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        IsDead = false;
        TryGetComponent<HealthBarController>(out healthBar);
    }
    public void ProcessHit(float damage)
    {
        if (healthBar != null)
        {
            healthBar.healthBar.UpdateHealthBar(currentHealth);
        }
        currentHealth -= damage;
        CreateFloatingDamage(damage.ToString());
        if (currentHealth <= 0)
        {
            IsDead = true;
        }
    }

    public void CreateFloatingDamage(string damage)
    {
        FloatingHitPoints floatingHP = Instantiate(damageText, floatingDamageCanvas.transform.position, Quaternion.Euler(0, 180, 0), floatingDamageCanvas.transform).GetComponent<FloatingHitPoints>();
        floatingHP.damageText.text = damage;

    }
}
