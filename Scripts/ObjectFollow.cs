// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script moves the camera to follow a given object. 
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Camera/Object Follow")]
public class ObjectFollow : MonoBehaviour
{

    [SerializeField] GameObject Object;
    [SerializeField] Vector3 offset;

    #region Methods

    private void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, Object.transform.position + offset, 0.1f);
    }

    #endregion
}
