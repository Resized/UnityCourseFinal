using UnityEngine;
using System.Collections;
public abstract class RangeEnemy : Enemy
{

    public float loadTime;
    bool isLoading;
    [Range(18, 50)]
    public float projectileSpeed;
    public ProjectileBase projectile;
    public Transform projectileSpawnPoint;
    public GameObject weapon;

}