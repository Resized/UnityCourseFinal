using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSpawn;
    private bool isAttacking = false;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (!isAttacking)
        {
            transform.LookAt(target.transform);
            anim.SetTrigger("Shoot");
            StartCoroutine(ShootProjectile());
        }
    }

    IEnumerator ShootProjectile()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        Projectile projectileInstance = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileInstance.SetTarget(target, gameObject);
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;

    }
}
