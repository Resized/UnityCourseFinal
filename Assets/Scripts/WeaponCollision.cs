using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    private WeaponController weaponController;
    // Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponentInParent<WeaponController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            weaponController.Hit(other);
        }
    }

}
