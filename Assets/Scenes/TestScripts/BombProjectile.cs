using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class BombProjectile : ProjectileBase
{

    public ParticleSystem ps;
    public float explosionRadius;
    public float areaDamage;
    protected override void Awake()
    {
        base.Awake();
        SetChildObjects(false);

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SetChildObjects(true);
    }
    [SerializeField]
    Collider[] collidersInRange;


    public override void OnCollisionEnter(Collision collision)
    {

        ParticleSystem explode = Instantiate(ps, transform.position, Quaternion.identity);
        explode.Play();
        collidersInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in collidersInRange)
        {
            IHittable target;
            if (collider.TryGetComponent<IHittable>(out target))
            {
                if (target.gameObject != parentObject)
                {
                    target.ProcessHit(areaDamage);
                }
                if (collision.gameObject == target.gameObject)
                {
                    target.ProcessHit(damageOnHit);
                }
            }
        }

        Destroy(gameObject);
    }

    public override void OnTriggerEnter(Collider collider)
    {

    }
    void SetChildObjects(bool active)
    {

        print("Setting child objest to is visible? " + isVisible);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }

    }
}
