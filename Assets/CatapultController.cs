using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    private GameObject[] targets;
    [SerializeField] GameObject projectileSpawn;
    [SerializeField] private GameObject currentTarget;
    private bool isAttacking = false;
    private Animator anim;
    private GameObject maxTarget;
    
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targets = GameObject.FindGameObjectsWithTag("Team2");
        maxTarget = GameObject.FindGameObjectWithTag("MaxTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget)
        {
            transform.LookAt(currentTarget.transform);
        }
        if ((int)Time.time % 2 == 0)
            ChooseEnemyInRange();
    }

    private void ChooseEnemyInRange()
    {
        currentTarget = maxTarget;
        foreach (GameObject target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position)
                <= Vector3.Distance(transform.position, currentTarget.transform.position)
                && !target.GetComponent<EnemyMovement>().IsDead())
            {
                currentTarget = target;
            }
        }
        Shoot();
    }

    void Shoot()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("Shoot");
            StartCoroutine(ShootProjectile());
        }
    }

    IEnumerator ShootProjectile()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        Projectile projectileInstance = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileInstance.SetTarget(currentTarget, gameObject);
        yield return new WaitForSeconds(2.733f);
        isAttacking = false;

    }
}
