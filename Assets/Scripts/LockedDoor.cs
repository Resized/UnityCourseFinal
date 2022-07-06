using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public GameObject lockOnDoor;
    private Animator anim;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        anim = target.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        anim.SetBool("Open", true);
        //lockOnDoor.SetActive(false);
        Destroy(lockOnDoor);

    }
}
