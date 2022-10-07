// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script moves the player using the transform method.
// </summary>

// TODO : Add jump mechanics

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Game Mechanics/Player/Player Controller Transform")]
public class PlayerControllerTransform : MonoBehaviour
{

    [SerializeField] float speed = 1f;

    #region Methods


    private void FixedUpdate() {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed / 5, 0f, Input.GetAxis("Vertical") * speed / 5));
    }

    #endregion
}
