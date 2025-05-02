using UnityEngine;
using NSpace;
using NSpace.AI;

class Search: IState
{
    NPCController host;
    Vector3 initialPos;
    float spreadRadius;
    float switchTimeout;
    float timeout;
    float lastSwitch;
    float startedAt;

    public bool complete => Time.time - startedAt > timeout;

    public Search(NPCController host, float switchTimeout, float spreadRadius, float timeout)
    {
        this.switchTimeout = switchTimeout;
        this.spreadRadius = spreadRadius;
        this.timeout = timeout;
        this.host = host;
    }

    public void OnEnter()
    {
        Debug.Log($"{host.name} entered search mode");
        host.Stop();
        initialPos = host.transform.position;
        lastSwitch = Time.time;
        startedAt = Time.time;
        host.currentState = NPCController.States.search;

    }

    public void OnExit()
    {
        host.Stop();

    }

    public void OnUpdate()
    {
        if (Time.time - lastSwitch > switchTimeout)
        {
            Vector3 newDst = initialPos + Random.onUnitSphere.With(y: 0) * spreadRadius;

            host.MoveTo(newDst);
            lastSwitch = Time.time;
        }
    }

    

}


