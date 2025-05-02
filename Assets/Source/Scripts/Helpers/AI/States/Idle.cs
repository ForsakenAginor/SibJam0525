using UnityEngine;
using NSpace.AI;

class Idle : IState
{
    NPCController host;
    float lastTurnTime;
    float interval;
    public int turnsCount;
    Vector3 point;
    public Idle(NPCController host)
    {

        this.host = host;

    }
    public void OnEnter()
    {
        Debug.Log($"{host.name} entered idle mode");
        host.Stop();
        lastTurnTime = Time.time;
        interval = Random.Range(2, 6);
        turnsCount = 0;

        point = host.transform.position;
        host.currentState = NPCController.States.idle;

        //host.FindWayTo(host.pos);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {



    }
}


