using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Transform cam;
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void LateUpdate()
    {

        transform.LookAt(cam.position);
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
