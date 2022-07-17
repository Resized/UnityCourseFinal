using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowProjectile : ProjectileBase
{
    public override void OnCollisionEnter(Collision collision)
    {
        print("collision");
        IHittable target;
        if (!collision.gameObject.TryGetComponent<IHittable>(out target))
        {
            return;
        };
        target._healthManager.ProcessHit(damageOnHit);
    }

    public override void OnTriggerEnter(Collider collider)
    {
        print("trigger");
        IHittable target;
        if (!collider.gameObject.TryGetComponent<IHittable>(out target))
        {
            return;
        };
        target.ProcessHit(damageOnHit);
        this.enabled = false;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        transform.SetParent(target.transform);

    }
}
