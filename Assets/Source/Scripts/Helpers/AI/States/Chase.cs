using UnityEngine;
using NSpace.AI;
using NSpace;

class Chase : IState
{
    float minDist;
    float keepTime;
    NPCController host;

    public Chase(NPCController host, float minDist, float keepTime)
    {

        this.host = host;
        this.minDist = minDist;
        this.keepTime = keepTime;
    }

    public void OnEnter()
    {
        Debug.Log($"{host.name} entered chase mode");
        host.Stop();
        host.SetRun(true);
        host.currentState = States.chase;

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
            float age = Time.time - host.enemies[0].detectedTime;
            Vector3 enemyPos = age<keepTime? host.enemies[0].entity.transform.position: host.enemies[0].lastKnownPos;
            Vector3 dest = enemyPos + (host.transform.position - enemyPos).normalized*minDist;

            host.MoveTo(dest);
        }
    }
}


