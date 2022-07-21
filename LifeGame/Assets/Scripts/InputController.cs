using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool isBlocked = true;

    public bool leftMouseButton { get; private set; }
    public bool rightMouseButton { get; private set; }
    public float mouseWheel { get; private set; }
    public Vector3 cameraMovement { get; private set; }

    private void Update()
    {
        if (!isBlocked) 
        { 
            leftMouseButton = Input.GetMouseButton(0) ? true : false;
            rightMouseButton = Input.GetMouseButton(1) ? true : false;
            mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            cameraMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }
    }
}
