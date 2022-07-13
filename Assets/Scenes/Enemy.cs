using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, ITargetableTarget
{
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float windUpTime;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float healthPoints;
    public Transform target { get => transform; }


    public float WindUpTime { get => windUpTime; set => windUpTime = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    public abstract IEnumerator Attack();
    public abstract void Hit(int hitAmount);
}
