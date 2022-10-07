// Written by Ben Baeyens - https://www.benbaeyens.com/
// This script is updated and maintained regularly. Last update - 08/02/2021 (DD/MM/YYYY)


// <summary>
// This script loads and unloads terrain dynamically to save performance in large levels


// <How to use>
// 1. Apply to any GameObject, empty or not. 
// 2. Drag in the anchor object (your player).
// 3. Add all the objects you want to the list. You can also select a layer and the script will find the GameObjects for you.
// 4. Tweak the settings to your liking.


// <Remarks>
// 1. Manual inclusion/exclusion take priority over everything else
// 2. Manual EXCLUSION takes priority over manual INCLUSION
// 3. The anchor object must be assigned in the inspector for the script to work


using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Ben's Script Library/Dynamic Level Loading")]
public class DynamicLevelLoading : MonoBehaviour
{

    #region enums

    public enum LayerMode
    {
        INCLUDE,
        EXCLUDE
    }

    #endregion


    #region Variables

    [Header("ANCHOR AND ROOT")]

    [Tooltip("The parent object of the objects that have to be loaded.")]
    public GameObject root; // The parent object of the loaded objects

    [Tooltip("The anchor around which everything is loaded (The player).")]
    public GameObject anchorObject; // The anchor object, around which everything has to be loaded


    [Header("SEARCH SETTINGS")]

    [Tooltip("The layer which has to be loaded and unloaded by the script (case sensitive) - LEAVE EMPTY FOR NONE")]
    public string layer; // The layer the script will look for at start of script. Leave empty for none
    public LayerMode layerMode; // The layer mode - Include / exclude the layer


    [Header("RANGE SETTINGS")]

    [Tooltip("Don't use vertical height, only x / z positions")]
    public bool useHeight;

    [Tooltip("The distance from the anchor at which object start to unload or load back in.")]
    public float unloadDistance = 15f; // The distance at which objects start to unload

    [Tooltip("The distance from the anchor at which animation is ignored and the object is loaded instantly. Prevents objects from animating when player is already in range. 0 to disable.")]
    public float instantLoadDistance = 5f; // The distance that the object will simply snap to it's original size. Prevents fast objects from encountering still-loading terrain


    [Header("ANIMATION SETTINGS")]

    public bool doAnimation = true; // Load and unload using scale

    [Tooltip("The size at which the animation stops and the object is deactivated.")]
    public float deativateMinSize = 0.1f; // The size at which the animation stops and the object is deactivated 

    [Range(0.001f, 1.5f)] public float unloadSpeed = 0.2f;
    [Range(0.001f, 1.5f)] public float loadSpeed = 0.2f;


    [Header("Manual adding")]
    [Tooltip("Add GameObjects you want to INCLUDE here. Useful if you can't alter the layer or child hierarchy of an object.")]
    public GameObject[] manualAdditions;
    [Tooltip("Add GameObjects you want to EXCLUDE from the script. Useful if you can't alter the layer or child hierarchy of an object.")]
    public GameObject[] manualExclusions;


    [HideInInspector] public Dictionary<GameObject, Vector3> objects; // The objects that are included in the loading/unloading.

    #endregion


    #region Methods

    void Start()
    {
        objects = new Dictionary<GameObject, Vector3>();

        if (root != null) // Null check
        {
            foreach (Transform child in root.transform)
            {
                objects.Add(child.gameObject, child.transform.localScale);
            }
        }

        foreach (GameObject obj in manualAdditions)
        {
            if (!objects.ContainsKey(obj)) // Duplication check
            {
                objects.Add(obj.gameObject, obj.transform.localScale);
            }
        }

        if (layer != "") // If the layer find feature is activated, search for layers
        {
            GameObject[] tempList = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in tempList)
            {
                if (layerMode == LayerMode.INCLUDE) // Check for layer inclusion
                {
                    if (obj.layer == LayerMask.NameToLayer(layer) && !objects.ContainsKey(obj)) // Duplication- and layer check
                    {
                        objects.Add(obj.gameObject, obj.transform.localScale);
                    }
                }
                else
                { // Equal to: If (layerMode == LayerMode.EXCLUDE){}
                    if (objects.ContainsKey(obj) && obj.layer == LayerMask.NameToLayer(layer))
                    {
                        objects.Remove(obj);
                    }
                }
            }
        }

        foreach (GameObject obj in manualExclusions)
        {
            if (objects.ContainsKey(obj)) // Remove all the manually excluded objects 
            {
                objects.Remove(obj);
            }
        }
    }


    private void Update()
    {
        foreach (var kvp in objects) // kvp = Key/Value Pair
        {
            if (rangeCheck(unloadDistance, kvp.Key))
            {
                if (doAnimation)
                {
                    if (kvp.Key.transform.localScale.x <= deativateMinSize)
                    {
                        kvp.Key.SetActive(false);
                    }
                    else
                    {
                        kvp.Key.transform.localScale /= unloadSpeed + 1;
                    }
                }
                else
                {
                    kvp.Key.SetActive(false);
                }
            }
            else if (!rangeCheck(instantLoadDistance, kvp.Key) && instantLoadDistance != 0)
            {
                kvp.Key.SetActive(true);
                kvp.Key.transform.localScale = kvp.Value;
            }
            else
            {
                kvp.Key.SetActive(true);
                if (doAnimation)
                {
                    if (kvp.Key.transform.localScale.x < kvp.Value.x)
                    {
                        kvp.Key.transform.localScale *= loadSpeed + 1;
                    }
                    else
                    {
                        kvp.Key.transform.localScale = kvp.Value;
                    }
                }
                else if (kvp.Key.transform.localScale.x != kvp.Value.x) // Safeguard for turning off animation mid-game
                {
                    kvp.Key.transform.localScale = kvp.Value;
                }
            }
        }
    }


    private bool rangeCheck(float distance, GameObject obj)
    {
        Vector3 objLocation;
        Vector3 anchorLocation;

        if (!useHeight)
        {
            objLocation = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
            anchorLocation = new Vector3(anchorObject.transform.position.x, 0, anchorObject.transform.position.z);
        }
        else
        {
            objLocation = obj.transform.position;
            anchorLocation = anchorObject.transform.position;
        }

        if (Vector3.Distance(objLocation, anchorLocation) >= distance)
            return true;
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        if (useHeight)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.gray;

        Gizmos.DrawWireSphere(anchorObject.transform.position, unloadDistance);
    }

    #endregion
}
