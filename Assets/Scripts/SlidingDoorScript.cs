using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorScript : MonoBehaviour
{

    private Animator anim;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        anim = target.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            anim.SetBool("Open", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            anim.SetBool("Open", false);
        }
    }



    // Update is called once per frame
    // Script that opens the door when the player is near it
    void Update()
    {

    }
}
