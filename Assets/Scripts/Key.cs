using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    internal void PickUp()
    {
        Destroy(gameObject);
    }
}
