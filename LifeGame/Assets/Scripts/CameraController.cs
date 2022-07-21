using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private int cameraSpeed = 5; 

    private Camera camera;
    private float maxSize, minSize = 1f;
    private float leftLimit, rightLimit, upperLimit, bottomLimit;

    private void Awake()
    {
        camera = Camera.main;
        maxSize = Camera.main.orthographicSize;

        rightLimit = camera.ViewportToWorldPoint(new Vector3(1, 1, 0)).x;
        upperLimit = camera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
        leftLimit = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        bottomLimit = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
    }

    private void Update()
    {

        CameraScale();
        CameraMove();
    }

    private void CameraScale()
    {
        camera.orthographicSize += inputController.mouseWheel;

        if (camera.orthographicSize > maxSize)
            camera.orthographicSize = maxSize;
        else if (camera.orthographicSize < minSize)
            camera.orthographicSize = minSize;
    }

    private void CameraMove()
    {
        Vector3 cameraDirection = inputController.cameraMovement;
        camera.transform.Translate(cameraDirection * cameraSpeed * Time.deltaTime);

        if (camera.transform.position.y > upperLimit)
            camera.transform.position = new Vector3(camera.transform.position.x, upperLimit, -10);
        if (camera.transform.position.y < bottomLimit)
            camera.transform.position = new Vector3(camera.transform.position.x, bottomLimit, -10);
        if (camera.transform.position.x > rightLimit)
            camera.transform.position = new Vector3(rightLimit, camera.transform.position.y, -10);
        if (camera.transform.position.x < leftLimit)
            camera.transform.position = new Vector3(leftLimit, camera.transform.position.y, -10);
    }
}
