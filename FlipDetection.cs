// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script detects and counts the number of flips an object has made (for example, in a hill climbing game)
// </summary>

// TODO: Add a 'safe angle' option so it is not totally 360°, but might increase the flow of the mechanic.
// TODO: Allow the script to detect in multiple directions, not just z

using UnityEngine;

[AddComponentMenu("Ben's Script Library/Game Mechanics/Flip Detection")]
public class FlipDetection : MonoBehaviour
{
    public bool isFlipping;

    float angle;
    float lastAngle;
    public int fullRotations;
    
    void FixedUpdate(){
        if(isFlipping){
            angle = transform.rotation.eulerAngles.z;

            if(lastAngle - angle > 270){
                fullRotations ++;
            }else if(angle - lastAngle > 270){
                fullRotations --;
            }
            lastAngle = angle;
        }
    }

    public void StartFlipping(){ // Call this function when the player gets off the ground.
        isFlipping = true;
        lastAngle = 0;
        fullRotations = 0;
    }

    public void OnFlipEnd(){ //Call this function when your flip ends / when the player lands
        isFlipping = false;

        if(fullRotations < 0){
            Debug.Log("Total backflips: " + -fullRotations);
            // Run the code for giving the player points using the variable 'fullRotations' for the amount of flips
        }else{
            Debug.Log("Total flips: " + fullRotations);
            // Run the code for giving the player points, again, using the variable 'fullRotations' for the amount of flips
        }
    }

    public void OnFlipCrash(){
        isFlipping = false;

        // Here you can run some extra code for when the flip crashes
    }
}