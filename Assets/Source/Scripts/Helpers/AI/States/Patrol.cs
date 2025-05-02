using UnityEngine;
using NSpace;
using NSpace.AI;

class Patrol : IState
{
    NPCController host;

    int currentWp;

    Vector3 point;
    
    public Patrol(NPCController host)
    {

        this.host = host;

    }
    public void OnEnter()
    {
        Debug.Log($"{host.name} entered patrol mode");
        host.Stop();
        host.SetRun(false); 
        host.currentState = NPCController.States.patrol;
        currentWp = (currentWp) % host.waypoints.Count;
        point = host.waypoints[currentWp].transform.position;
        host.MoveTo(point);


    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if(Vector3.Distance( host.transform.position.With(y:0), point.With(y:0)) < 2)
        {
            currentWp = (currentWp + 1)%host.waypoints.Count;
            point = host.waypoints[currentWp].transform.position;
            host.MoveTo(point);
        }



    }
}


