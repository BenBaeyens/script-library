// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script gives control over the orthographic camera.
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Camera/Orthographic Camera Controller")]
public class OrthographicCameraController : MonoBehaviour
{

    Camera cam;

    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    public float zoomSpeed = 14f;
    public float rotationSpeed = 2f;

    float horizontal;
    float vertical;
    float scroll;

    float cameraX;
    float cameraY;


    float desiredOrthographicSize;
    bool isRotating = false;

    [Header("Boundaries")]
    public float orthographicMin = 5f;
    public float orthographicMax = 70f;

    private void Start() {
        cam = GetComponent<Camera>();
        desiredOrthographicSize = cam.orthographicSize;
    }

    private void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        scroll = Input.GetAxis("Mouse ScrollWheel");

        // Keep the desired orthographic size within bounds
        if(desiredOrthographicSize <= orthographicMax && desiredOrthographicSize >= orthographicMin)
            desiredOrthographicSize -= scroll * zoomSpeed;

        // Keep the camera view within the bounds
        if(desiredOrthographicSize > orthographicMax)
            desiredOrthographicSize = orthographicMax;
        if(desiredOrthographicSize < orthographicMin)
            desiredOrthographicSize = orthographicMin;

        if(cam.orthographicSize > orthographicMax)
            cam.orthographicSize = orthographicMax;
        if(cam.orthographicSize < orthographicMin)
            cam.orthographicSize = orthographicMin;

        if(Input.GetMouseButton(0))
            isRotating = true;
        else
            isRotating = false;
    }

    private void LateUpdate() {
        if(isRotating){
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cam.fieldOfView / orthographicMax * rotationSpeed, Input.GetAxis("Mouse X") * cam.fieldOfView / orthographicMax  * rotationSpeed, 0));
            cameraX = transform.rotation.eulerAngles.x;
            cameraY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(cameraX, cameraY, 0);
        }
        if(cam.orthographicSize < orthographicMax && cam.orthographicSize > orthographicMin){
           cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredOrthographicSize, 0.1f); // Zooms the camera in and out normally
        }else if(cam.orthographicSize <= orthographicMin +1f && scroll < 0){
            cam.orthographicSize -= scroll * zoomSpeed; // Zooms the camera out
        }else if(cam.orthographicSize >= orthographicMax -1f && scroll > 0){
            cam.orthographicSize -= scroll * zoomSpeed; // Zooms the camera in
        }

        transform.Translate((Vector3.up * vertical * verticalSpeed) * cam.orthographicSize / orthographicMax); // Up and down camera movement
        transform.Translate((Vector3.right * horizontal * horizontalSpeed) * cam.orthographicSize / orthographicMax); // Left and Right camera movement
    
    }
}
