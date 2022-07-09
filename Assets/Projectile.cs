using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Vector3 target;
    private float speed = 50f;
    private string enemyTeamTag;


    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
        transform.LookAt(target);
        rb = GetComponent<Rigidbody>();
        rb.AddForce((target-transform.position).normalized* speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.tag == enemyTeamTag)
        {
            if (collider.GetComponent<EnemyMovement>())
                collider.GetComponent<EnemyMovement>().Hit(25);
            if (collider.GetComponent<PlayerMovement>())
                collider.GetComponent<PlayerMovement>().Hit(25);
            Destroy(gameObject);
        }
    }


    internal void SetTarget(GameObject currentTarget)
    {
        target = currentTarget.transform.position;
        enemyTeamTag = currentTarget.tag;
    }
}
