using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool IsDead = false;

    public float healthPoints { get; set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        IsDead = false;
    }
    public void ProcessHit(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            IsDead = true;
        }
    }


}
