using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator anim;
    private bool isAttacking = false;
    private PlayerMovement player;
    public GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // play animation
            Attack();
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Swing") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    internal void Hit(Collider other)
    {
        // set hit animation on collider
        if (isAttacking && other.GetComponent<EnemyMovement>() && other.tag != player.tag)
        {
            other.GetComponent<EnemyMovement>().Hit(25);
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
