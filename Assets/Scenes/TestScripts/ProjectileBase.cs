using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public abstract class ProjectileBase : MonoBehaviour
{
    private ITargetable targetPoint;
    public float speed = 50f;
    public bool isVisible = false;
    public float damageOnHit = 15;
    public Rigidbody rb;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();

    }
    private void OnEnable()
    {

        rb.isKinematic = false;
        Destroy(gameObject, 7);
        Vector3 targetCenter = targetPoint.gameObject.GetComponent<Collider>().bounds.center + (Vector3.up * 0.3f);
        rb.AddForce(calcBestThrowVec(transform.position, targetCenter, speed), ForceMode.VelocityChange);
        Debug.DrawLine(transform.position, targetCenter, Color.red, 10);
    }
    void Start()
    {


    }

    private void Update()
    {
        if (!isVisible)
        {
            GetComponent<MeshRenderer>().enabled = true;
            isVisible = true;
        }
        else if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, transform.up);
        }
    }

    public void SetTarget(ITargetable currentTarget, float projectileSpeed)
    {
        targetPoint = currentTarget;
        speed = projectileSpeed;
        transform.LookAt(targetPoint.transform);

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
    public abstract void OnCollisionEnter(Collision collision);
    // {
    //     Collider collider = collision.collider;
    //     if (collider.gameObject != shooter)
    //     {
    //         if (projectileTag == "bomb")
    //         {
    //             Vector3 explosionPos = transform.position;
    //             Collider[] colliders = Physics.OverlapSphere(explosionPos, 5f);
    //             ParticleSystem explode = Instantiate(explosion, explosionPos, Quaternion.identity);
    //             explode.Play();
    //             foreach (Collider hit in colliders)
    //             {
    //                 // Debug.Log(hit.gameObject.name);
    //                 if (hit.GetComponent<EnemyMovement>())
    //                     hit.GetComponent<EnemyMovement>().Hit(25);
    //                 if (hit.GetComponent<PlayerMovement>())
    //                     hit.GetComponent<PlayerMovement>().Hit(25);
    //             }
    //             Destroy(gameObject);
    //             return;

    //         }
    //         else if (projectileTag != "bomb")
    //         {
    //             if (collider.GetComponent<EnemyMovement>())
    //                 collider.GetComponent<EnemyMovement>().Hit(25);
    //             if (collider.GetComponent<PlayerMovement>())
    //                 collider.GetComponent<PlayerMovement>().Hit(25);
    //             if (collider.tag == "Attackers" || collider.tag == "Defenders")
    //             {
    //                 Destroy(gameObject);
    //             }

    //         }
    //     }
    // }



    public abstract void OnTriggerEnter(Collider collider);

    // Start is called before the first frame update

}
