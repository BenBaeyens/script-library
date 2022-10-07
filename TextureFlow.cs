// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script scrolls any texture across an object.
// Just apply this script on the object you want the texture to move of and set the variables to your preferences.
// </summary>

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Textures/Texture Flow")]
public class TextureFlow : MonoBehaviour
{

    public float scrollSpeedHorizontal = 0.5f;
    public float scrollSpeedVertical = 0f;

    Renderer rend; 

    void Start()
    {
        // Get the object's renderer component
        rend = GetComponent<Renderer>(); 
    }

    void Update()
    {
        // Calculate the offset based on the time passed
        float offsetX = Time.time * scrollSpeedHorizontal;
        float offsetY = Time.time * scrollSpeedVertical;

        // Apply the offset to the texture
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}