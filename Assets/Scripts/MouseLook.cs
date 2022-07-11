using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public LayerMask mask;
    public GameObject chest;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Implement Mouse Look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        //transform.Rotate(Vector3.up * mouseX);
        //transform.Rotate(Vector3.right * -mouseY);
        //Camera.main.transform.Rotate(Vector3.up * mouseX);
        //Camera.main.transform.Rotate(Vector3.right * -mouseY);


        if (Physics.Raycast(transform.position, transform.forward, out var hit, 10f))
        {
            if (hit.collider.gameObject.tag == "Chest")
            {
                // Debug.Log("Chest");
            }
        }
    }
}
