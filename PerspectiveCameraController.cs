// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script gives control over the perspective camera.
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Camera/Perspective Camera Controller")]
public class PerspectiveCameraController : MonoBehaviour
{

    Camera cam;

    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    public float upDownSpeed = 0.3f;
    public float zoomSpeed = 14f;
    public float rotationSpeed = 2f;

    float horizontal;
    float vertical;
    float scroll;

    float cameraX;
    float cameraY;

    float upDown;

    float desiredFOV;
    bool isRotating = false;

    [Header("Boundaries")]
    public float FOVMin = 5f;
    public float FOVMax = 110f;

    private void Start() {
        cam = GetComponent<Camera>();
        desiredFOV = cam.fieldOfView;
    }

    private void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        scroll = Input.GetAxis("Mouse ScrollWheel");


        // Keep the desired FOV size within bounds
        if(desiredFOV <= FOVMax && desiredFOV >= FOVMin)
            desiredFOV -= scroll * zoomSpeed;

        // Keep the camera view within the bounds
        if(desiredFOV > FOVMax)
            desiredFOV = FOVMax;
        if(desiredFOV < FOVMin)
            desiredFOV = FOVMin;

        if(cam.fieldOfView > FOVMax)
            cam.fieldOfView = FOVMax;
        if(cam.fieldOfView < FOVMin)
            cam.fieldOfView = FOVMin;

        if(Input.GetMouseButton(0))
            isRotating = true;
        else
            isRotating = false;

        if(Input.GetKey(KeyCode.LeftShift)){
            upDown = -1;
        }else if(Input.GetKey(KeyCode.Space)){
            upDown = 1;
        }else{
            upDown = 0;
        }
    }

    private void LateUpdate() {
        if(isRotating){
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cam.fieldOfView * 2 / FOVMax * rotationSpeed, Input.GetAxis("Mouse X") * cam.fieldOfView * 2 / FOVMax  * rotationSpeed, 0));
            cameraX = transform.rotation.eulerAngles.x;
            cameraY = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(cameraX, cameraY, 0);
        }
        if(cam.fieldOfView < FOVMax && cam.fieldOfView > FOVMin){
           cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, 0.1f); // Zooms the camera in and out normally
        }else if(cam.fieldOfView <= FOVMin +1f && scroll < 0){
            cam.fieldOfView -= scroll * zoomSpeed; // Zooms the camera out
        }else if(cam.fieldOfView >= FOVMax -1f && scroll > 0){
            cam.fieldOfView -= scroll * zoomSpeed; // Zooms the camera in
        }

        transform.Translate((Vector3.forward * vertical * verticalSpeed) * cam.fieldOfView / FOVMax * 2); // foward and backwards camera movement
        transform.Translate((Vector3.right * horizontal * horizontalSpeed) * cam.fieldOfView / FOVMax * 2); // Left and Right camera movement
    
        if(upDown == 1)
            transform.position += Vector3.up  * cam.fieldOfView * 2 / FOVMax * upDownSpeed;
        if(upDown == -1)
            transform.position -= Vector3.up *  cam.fieldOfView * 2 / FOVMax * upDownSpeed;

    }
}
