using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    private Animator anim;
    private bool isOpen = false;
    private AudioSource audioSource;
    public AudioClip doorOpenAudio;
    public AudioClip doorCloseAudio;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        anim = target.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (!isOpen)
            {
                isOpen = true;
                anim.SetBool("Open", true);
                audioSource.PlayOneShot(doorOpenAudio);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (isOpen)
            {
                isOpen = false;
                anim.SetBool("Open", false);
                audioSource.PlayOneShot(doorCloseAudio);
            }
        }
    }



    // Update is called once per frame
    // Script that opens the door when the player is near it
    void Update()
    {

    }
}
