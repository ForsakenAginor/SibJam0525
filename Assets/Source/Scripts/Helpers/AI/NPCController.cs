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

    [SerializeField] private EnemyAnimationController _enemyAnimationController;
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
    [SerializeField] BehaviourBase brain;
    [HideInInspector]
    public States currentState;
    public List<Transform> waypoints;
    

    public List<Detected> enemies;
    float alarm;
    float lastSensorUpdate;

   
    Automaton automaton;
    
    

    // Start is called before the first frame update
    void Start()
    {


       
        agent = GetComponent<NavMeshAgent>();
        SetRun(false);
        enemies = new List<Detected>();
        automaton = brain.Init(this);



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
    
        if(run)
            _enemyAnimationController.Run();
        else
            _enemyAnimationController.Walk();
    }

    public bool hasPath => agent.hasPath;
    

    

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSensorUpdate > updateSensorRate)
        {
            UpdateSensor();
            lastSensorUpdate = Time.time;
        }
        automaton.Tick();
        
        enemies.RemoveAll(E => Time.time - E.detectedTime > isFreshFor);
        
        if (alarm > 0) alarm -= Time.deltaTime * 0.01f;

        
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


