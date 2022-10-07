// Written by Ben Baeyens - https://www.benbaeyens.com/

// <summary>
// This script controls the functionality of the click to move function.
// </summary>

using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("Ben's Script Library/Player/NavMesh Click To Move")]
public class MoveToPoint : MonoBehaviour {

    NavMeshAgent agent;

    bool pressed;
    bool released;

    public bool arrived;
    

    NavMeshPath path;

    float timer = 0f;
    float maxTimer = 0.1f;
    
    void Start() {
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update() {

        Debug.DrawLine(transform.position, agent.destination, Color.red);

        if(Vector3.Distance(agent.destination, transform.position) <= 1.1f){
            agent.ResetPath();
            arrived = true;
        }else{
            arrived = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            timer = 0f;
            pressed = true;
        }
        if(Input.GetMouseButton(0)){
            timer += 1 * Time.deltaTime;
        }
        if(Input.GetMouseButtonUp(0)){
            if(timer <= maxTimer)
                released = true;
            else{
                released = false;
                timer = 0;
                pressed = false;
            }
        }

        if(pressed && released){
            MovePlayerToPoint();
            pressed = false;
            released = false;
        }
    }

    private void MovePlayerToPoint()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            NavMeshHit navMeshHit;
            if(NavMesh.SamplePosition(hit.point, out navMeshHit, 100, -1)){
                agent.destination = navMeshHit.position;
            }
        }
    }
}