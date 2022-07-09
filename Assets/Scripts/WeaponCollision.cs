using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
    private WeaponController weaponController;
    string playerTag;
    // Start is called before the first frame update
    void Start()
    {

        playerTag = GameObject.Find("Player").tag;
        weaponController = GetComponentInParent<WeaponController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != playerTag)
        {
            weaponController.Hit(other);
        }
    }

}
