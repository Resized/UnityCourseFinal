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
    public ParticleSystem explosion;

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
            rb.AddForce(calcBestThrowVec(transform.position, target, 20f), ForceMode.VelocityChange);
        }
        else if (projectileTag == "arrow")
        {
            rb.AddForce(calcBestThrowVec(transform.position, target, 50f), ForceMode.VelocityChange);
        }
        else if (projectileTag == "bomb")
        {
            speed = 100f;
            rb.AddForce(calcBestThrowVec(transform.position, target, 10f), ForceMode.VelocityChange);
        }

    }

    private Vector3 calcBestThrowVec(Vector3 origin, Vector3 target, float speed)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = xz / speed;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
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
            if (projectileTag == "bomb")
            {
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, 5f);
                ParticleSystem explode = Instantiate(explosion, explosionPos, Quaternion.identity);
                explode.Play();
                foreach (Collider hit in colliders)
                {
                    Debug.Log(hit.gameObject.name);
                    if (hit.GetComponent<EnemyMovement>())
                        hit.GetComponent<EnemyMovement>().Hit(25);
                    if (hit.GetComponent<PlayerMovement>())
                        hit.GetComponent<PlayerMovement>().Hit(25);
                }
                Destroy(gameObject);
                return;

            }
            else if (projectileTag != "bomb")
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
    }

    public void SetTarget(GameObject currentTarget, GameObject shooter)
    {
        this.target = currentTarget.GetComponent<Collider>().bounds.center;
        this.shooter = shooter;
    }
    
}
