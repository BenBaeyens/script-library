// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script changes the cursor when passing over an object or GUI
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/UI/Change Cursor")]
public class ChangeCursorScript : MonoBehaviour
{

    [SerializeField] Texture2D computerCursor;
    #region Methods

    private void OnMouseEnter() {
        Cursor.SetCursor(computerCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);    
    }

    #endregion
}
