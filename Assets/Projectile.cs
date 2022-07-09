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
    private string projectileTag;
    private GameObject shooter;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        projectileTag = gameObject.tag;
        Destroy(gameObject, 5);
        transform.LookAt(target);
        rb = GetComponent<Rigidbody>();
        if (projectileTag == "catapult")
        {
            speed = 250f;
            rb.AddForce(((target - transform.position + Vector3.up*30).normalized) * speed, ForceMode.Impulse);
        }
        else if (projectileTag == "arrow")
        {
            rb.AddForce((target-transform.position).normalized* speed, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.gameObject != shooter)
        {
            if (collider.GetComponent<EnemyMovement>())
                collider.GetComponent<EnemyMovement>().Hit(25);
            if (collider.GetComponent<PlayerMovement>())
                collider.GetComponent<PlayerMovement>().Hit(25);
            if (collider.tag == "Team1" || collider.tag == "Team2")
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(GameObject currentTarget, GameObject shooter)
    {
        this.target = currentTarget.GetComponent<Collider>().bounds.center;
        this.shooter = shooter;
    }
    
}
