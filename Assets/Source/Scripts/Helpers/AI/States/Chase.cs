using UnityEngine;
using NSpace.AI;

class Chase : IState
{

    NPCController host;
    
    public Chase(NPCController host)
    {

        this.host = host;
    }
    public void OnEnter()
    {
        Debug.Log($"{host.name} entered chase mode");
        host.Stop();
        host.SetRun(true);
        host.currentState = NPCController.States.chase;

    }

    public void OnExit()
    {
        host.Stop();
        host.SetRun(false);

    }

    public void OnUpdate()
    {
        if (host.enemies.Count>0)
        {
            host.MoveTo(host.enemies[0].lastKnownPos);
        }
    }
}


