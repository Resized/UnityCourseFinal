using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    private Animator anim;
    private bool isOpen = false;
    public GameObject target;
    public TextMesh text;

    // Start is called before the first frame update
    void Start()
    {
        anim = target.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is in the trigger area and if the player is pressing the interact button
        if (other.gameObject.tag == "Player")
        {
            isOpen = true;
            text.text = "Press Space to open";
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOpen = false;
            anim.SetBool("Open", false);
            text.text = "";
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Check if player is in the trigger area and if the player is pressing the interact button
        if (isOpen && Input.GetKeyDown(KeyCode.Space))
        {
            // Play the chest opening animation
            anim.SetBool("Open", true);
            text.text = "Press Space to grab the Key";
        }
    }
}
