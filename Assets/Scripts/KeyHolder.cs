using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    private List<Key> keys = new List<Key>();
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    public void AddKey(Key key)
    {
        keys.Add(key);
    }

    public bool HasKey()
    {
        return keys.Count > 0;
    }

    public void RemoveKey(Key key)
    {
        keys.Remove(key);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Shoot raycast and check if it hits a key
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                // Debug.Log("Hit " + hit.collider.name);
                Key key = hit.collider.GetComponent<Key>();
                if (key != null)
                {
                    key.PickUp();
                    AddKey(key);
                }

                LockedDoor lockedDoor = hit.collider.GetComponent<LockedDoor>();
                if (lockedDoor != null && HasKey())
                {
                    lockedDoor.Unlock();
                }
            }
        }
    }
}
