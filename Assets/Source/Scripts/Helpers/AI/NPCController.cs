using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NSpace;
using NSpace.AI;

public class NPCController : MonoBehaviour
{
    public enum States
    {
        idle,
        patrol,
        search,
        chase       
    }


    NavMeshAgent agent;
    [SerializeField] Transform sensor;
    [SerializeField] float vievAngle;
    public List<int> enemySides;
    [SerializeField] float perception;
    [SerializeField] float visibleRange;
    [SerializeField] float isFreshFor;
    [SerializeField] float alarmSpeed;
    [SerializeField] float updateSensorRate;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    Automaton automaton;

    public States currentState;
    public List<Transform> waypoints;
    

    public List<Detected> enemies;
    float alarm;
    float lastSensorUpdate;

    
    

    // Start is called before the first frame update
    void Start()
    {

        Search search = new Search(this, 5, 15);
        Chase chase = new Chase(this);
        Idle idle = new Idle(this);
        Patrol patrol = new Patrol(this);

        agent = GetComponent<NavMeshAgent>();
        SetRun(false);
        enemies = new List<Detected>();
        automaton = new Automaton();
        
        System.Func<bool> spoted = () => {  return enemies.Count > 0; };
        System.Func<bool> lost = () => {  return enemies.Count <= 0 && !agent.hasPath; };
        System.Func<bool> timeout = () =>  search.complete; 
        automaton.AddTransition(search, chase, spoted) ;
        automaton.AddTransition(chase, search, lost);
        automaton.AddTransition(idle, chase, spoted);
        automaton.AddTransition(search, idle, timeout);
        automaton.AddTransition(idle, patrol, () => waypoints.Count > 0);
        automaton.AddTransition(patrol,chase, spoted);
        automaton.SetState(idle);
    }

    public void Stop()
    {
       
        agent.destination = transform.position;
    }
    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
    
    public void SetRun(bool run)
    {
        agent.speed = run ? runSpeed : walkSpeed;
    }
    

    

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSensorUpdate > updateSensorRate)
        {
            UpdateSensor();
            lastSensorUpdate = Time.time;
        }
        //Debug.Log(enemies.Count);
        automaton.Tick();
        //if(enemies.Count > 0)
        //{
        //    agent.destination = enemies[0].entity.transform.position;
            enemies.RemoveAll(E => Time.time - E.detectedTime > isFreshFor);
        //}
        //if (alarm > 0 && alarm<1) alarm -= Time.deltaTime * 0.01f;

        
    }

    void UpdateSensor()
    {
        foreach(var entity in Detectible.entities)
        {
            
            if(entity==null || entity.transform==null) continue;            
            if (entity.transform.root == transform.root) continue;
            Debug.DrawLine(transform.position, entity.transform.position, Color.white);
            if (!enemySides.Contains(entity.side)) continue;
            float dist = Vector3.Distance(transform.position, entity.transform.position);

            if (dist > visibleRange) continue;
            foreach(var point in entity.spotPoints)
            {
                Debug.DrawLine(sensor.position, point.position, Color.yellow);
                Vector3 viewLine = point.position - sensor.position;
                float angle = Vector3.Angle(sensor.forward, viewLine);
                
                if (angle > vievAngle) continue;
                if ((dist / visibleRange) + (angle / vievAngle) > perception * (alarm + Random.value)) continue;
                
                Debug.DrawLine(sensor.position, point.position, Color.blue);
                
                if (Physics.Raycast(sensor.position, viewLine, out RaycastHit hit, visibleRange) && hit.collider.transform.root != point.root) continue;
                alarm += alarmSpeed;
                if (alarm < 1) continue;
                Debug.DrawLine(sensor.position, point.position, Color.red);
                Detected inList = enemies.Find(E => E.entity == entity);
                if (inList==null)
                {
                    Detected d = new Detected();
                    d.entity = entity;
                    d.detectedTime = Time.time;
                    d.lastKnownPos = entity.transform.position;
                    enemies.Add(d);
                }
                else
                {                    
                    inList.detectedTime = Time.time;
                    inList.lastKnownPos = entity.transform.position;                    
                }
            }
            
        }
    }
}

class Search: IState
{
    NPCController host;
    Vector3 initialPos;
    float spreadRadius;
    float switchTimeout;
    float timeout;
    float lastSwitch;
    float startedAt;

    public bool complete => Time.time - startedAt > 30;

    public Search(NPCController host, float switchTimeout, float spreadRadius)
    {
        this.switchTimeout = switchTimeout;
        this.spreadRadius = spreadRadius;
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


