// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script automatically snaps any component it's attached to to their corresponding values.
// </summary>
// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script automatically snaps all the children of the object to their snapping values. This saves performance and could easy workflow over the single object snap when dealing with large amounts of objects.
// Just attach this script to the parent object, and all the objects within will be automatically snapped.
// </summary>

using UnityEngine;

[ExecuteInEditMode] [AddComponentMenu("Ben's Script Library/Level Building/Group Object Snap")]
public class GroupObjectSnap : MonoBehaviour 
{

    public bool _enabled = true;

    public float snapValueX;
    public float snapValueY;
    public float snapValueZ;


    void Update ()  
    {
        if(!Application.isPlaying && _enabled){

            foreach (Transform child in gameObject.transform)
            {
                
                if (snapValueX != 0)
                    child.transform.position = new Vector3(Mathf.Round(child.transform.position.x * (1 / snapValueX)) / (1 / snapValueX),child.transform.position.y, child.transform.position.z);

                if (snapValueY != 0)
                    child.transform.position = new Vector3(child.transform.position.x, Mathf.Round(child.transform.position.y * (1 / snapValueY)) / (1 / snapValueY), child.transform.position.z);

                if(snapValueZ != 0)
                    child.transform.position = new Vector3(child.transform.position.x, child.transform.position.y, Mathf.Round(child.transform.position.z * (1 / snapValueZ)) / (1 / snapValueZ)); 
            }       
        }  
    }
}