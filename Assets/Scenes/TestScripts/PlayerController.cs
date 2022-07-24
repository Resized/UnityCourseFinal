using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    RaycastHit hit;




    // Update is called once per frame
    private void Update()
    {
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed);
    }



    void GetRaycast(TeamController.TargetIconsEnum targetIcon)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            EnemyMovement enemy = hit.transform.GetComponent<EnemyMovement>();
            if (enemy)
            {
                if (enemy.tag == tag)
                {

                    return;
                }


            }
        }
    }



}