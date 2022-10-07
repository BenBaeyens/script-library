// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script moves the player using the rigidbody.velocity method, as an alternative to the addForce method. This causes a more direct movement, instead of a gradual one.
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Game Mechanics/Player/Player Controller Velocity")]
public class PlayerControllerRigidbodyVelocity : MonoBehaviour
{

    [SerializeField] float speed = 4f;
    [SerializeField] float jumpForce = 6f;

    float distToGround;

    #region Methods

    private void Start() { // Get the distance between the ground and the player collider
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
            GetComponent<Rigidbody>().velocity += new Vector3(0f, jumpForce, 0f);
    }

    bool isGrounded(){ // Check if the player is on the ground
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);    
    }

    private void FixedUpdate() {
        GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxis("Horizontal") * speed, GetComponent<Rigidbody>().velocity.y, Input.GetAxis("Vertical") * speed);
    }

    #endregion
}
