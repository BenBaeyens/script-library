// Script coded by Ben Baeyens - https://www.benbaeyens.com/
using System.Collections;
using UnityEngine;

public class SecurityCameraMovement : MonoBehaviour
{
    #region variables

    // Right rotation
    [SerializeField] float rotateRightSpeed = 2;
    [SerializeField] float rotateRightDegrees = 90;
    [SerializeField] float waitBeforeTurningRight = 2;

    // Left rotation
    [SerializeField] float rotateLeftSpeed = 1;
    [SerializeField] float rotateLeftDegrees = 90;
    [SerializeField] float waitBeforeTurningLeft = 2;

    // Disable this if you want to stop rotation.
    // WARNING: Enabling it at runtime will not cause the rotation to continue!
    public bool isEnabled = true;

    // Private field to determine the current direction
    private bool goingRight = true;


    #endregion
    #region methods

    private void Start()
    {
        StartCoroutine(IRotate(Vector3.up * rotateRightDegrees, rotateRightSpeed, waitBeforeTurningRight));
    }

    IEnumerator IRotate(Vector3 angles, float time, float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + angles);

        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }


        // If the script is enabled, we want to move either left or right. 
        if (isEnabled)
        {
            // Flip the direction for the next time IRotate is called
            goingRight = !goingRight;

            if (goingRight)
                StartCoroutine(IRotate(Vector3.up * rotateRightDegrees, rotateRightSpeed, waitBeforeTurningRight));
            else
                StartCoroutine(IRotate(Vector3.up * rotateLeftDegrees * -1, rotateLeftSpeed, waitBeforeTurningLeft));
        }
    }
    #endregion
}
