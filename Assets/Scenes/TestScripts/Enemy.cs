using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthManager))]
public abstract class Enemy : MonoBehaviour, ITargetable, IHittable
{


    public float healthPoints { get; set; }
    public abstract HealthManager _healthManager { get; set; }
    public float speed;
    public float visionRange;
    public float attackRange;
    public float attackDamage;
    public float timeBetweenAttacks;
    public bool isAlive = true;
    public bool isAttacking;

    public abstract void Attack(IHittable target);
    private void Awake()
    {
        _healthManager = GetComponent<HealthManager>();
    }
    public void ProcessHit(float damage)
    {
        _healthManager.ProcessHit(damage);
        if (_healthManager.IsDead)
        {
            isAlive = false;
        }
    }
}
