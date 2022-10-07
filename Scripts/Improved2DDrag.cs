// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script enables you to improve dragging of 2D objects. 
// For example, you can select an element by the corner and drag it from that anchor point.
// The element will not snap to the mouse cursor.
// NOTE: A collider is required for this script to work!
// </summary>
using UnityEngine;

public class Improved2DDrag : MonoBehaviour
{
    private Vector3 offset;

    // Calculate the offset when the player presses down on the element
    private void OnMouseDown() {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // When we start dragging, we use the previously calculated offset to move the element.
    private void OnMouseDrag() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
    }
}
