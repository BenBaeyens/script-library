// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script isolates the jump movement of an object, using the rigidbody component's Velocity method
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Game Mechanics/Player/Player Controller Jump Velocity")]
public class PlayerControllerJumpVelocity : MonoBehaviour
{

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

    #endregion
}
