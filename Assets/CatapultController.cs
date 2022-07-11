using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] private GameObject[] targets;
    [SerializeField] GameObject projectileSpawn;
    [SerializeField] private GameObject currentTarget;

    private bool isAttacking = false;
    private Animator anim;
    private GameObject maxTarget;
    [SerializeField] public bool isControlled = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        targets = GameObject.FindGameObjectsWithTag("Defenders");
        maxTarget = GameObject.FindGameObjectWithTag("MaxTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget)
        {
            transform.LookAt(currentTarget.transform);
        }
        if (isControlled)
        {
            if (currentTarget.GetComponent<EnemyMovement>() && currentTarget.GetComponent<EnemyMovement>().IsDead())
            {
                isControlled = false;
                return;
            }
            Shoot();
        }
        else
        {
            if ((int)Time.time % 2 == 0)
                ChooseEnemyInRange();
        }
    }

    public void SetControlledTarget(GameObject target)
    {
        print("TEST");
        isControlled = true;
        currentTarget = target;
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
