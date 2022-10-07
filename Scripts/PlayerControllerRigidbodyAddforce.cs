// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script moves the player using the rigidbody.AddForce method, while taking account for max speed.
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Game Mechanics/Player/Player Controller AddForce")]
public class PlayerControllerRigidbodyAddforce : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    [SerializeField] float jumpForce = 500f;
    [SerializeField] float maxSpeed = 20f;

    float distToGround;

    #region Methods

    private void Start() { // Get the distance between the ground and the player collider
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, jumpForce, 0f));
    }

    bool isGrounded(){ // Check if the player is on the ground
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);    
    }

    private void FixedUpdate() {
        if(GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            GetComponent<Rigidbody>().AddForce(new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed));
    }

    #endregion
}
