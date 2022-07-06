using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    public Vector3 velocity;
    public GameObject enemy;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed);

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // if pressing q change enemy animation state
        if (Input.GetKeyDown(KeyCode.Q))
        {
            enemy.GetComponent<Animator>().SetInteger("State", 1);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            enemy.GetComponent<Animator>().SetInteger("State", 0);
        }

    }
}