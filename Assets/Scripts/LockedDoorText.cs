using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorText : MonoBehaviour
{
    public TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            KeyHolder keyHolder = other.gameObject.GetComponent<KeyHolder>();
            if (keyHolder.HasKey())
            {
                text.text = "Press Space to open";
            }
            else
            {
                text.text = "You need a key to open this door";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.text = "";
        }
    }
}
