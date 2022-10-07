// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script controls the scroll functionality and drag systems.
// </summary>

using UnityEngine;

public class MovementController : MonoBehaviour
{

    Camera cam;

    public float speed = 15f; // The default scroll to move speed 
    public float rotationSpeed = 2f; // The rotational speed when dragging the camera

    [SerializeField] GameObject playerObject; // The player object
    [SerializeField] GameObject smoothTransitionObject; // This is the object that ensures gradual movement when scrolling


    float scroll;

    float cameraX;
    float cameraY;

    Quaternion lastpos;

    Vector3 newPosition;

    bool isRotating = false;


    private void Start() {
        cam = GetComponent<Camera>();
        newPosition = playerObject.transform.position;
    }

    private void Update() {

        Debug.Log(transform.parent.GetComponent<MoveToPoint>().agent.destination);
        scroll = Input.GetAxis("Mouse ScrollWheel");
        lastpos = transform.rotation;

        if(Input.GetMouseButton(0))
            isRotating = true;
        else
            isRotating = false;

        if(transform.rotation.y != 0){
            RotatePlayer();
        }

            if(transform.parent.GetComponent<MoveToPoint>().arrived){ // Checks if the player is still moving using the NavMesh system
                Vector3 normalizedDirection = (smoothTransitionObject.transform.position).normalized;
                smoothTransitionObject.transform.Translate(normalizedDirection * -Input.GetAxis("Mouse ScrollWheel"));
                playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, smoothTransitionObject.transform.position, 0.1f);
            }
            else{
                smoothTransitionObject.transform.position = playerObject.transform.position;
            }
    }

    private void LateUpdate() {

        smoothTransitionObject.transform.rotation = playerObject.transform.rotation;

        if(isRotating)
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotationSpeed, 0, 0));
            playerObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
            RotatePlayer();
        }




    }

    private void RotatePlayer()
    {
        cameraX = transform.rotation.eulerAngles.x;
        cameraY = playerObject.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Lerp(lastpos, Quaternion.Euler(cameraX, cameraY, 0), 0.1f);
        playerObject.transform.rotation = Quaternion.Euler(0, cameraY, 0);

    }


}
