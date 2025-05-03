using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NSpace;
using NSpace.AI;
using FMODUnity;

public class NPCController : MonoBehaviour
{
    [System.Serializable] struct NPCSetup
    {
        public float perception;
        public float vievAngle;
        public float visibleRange;
        public float isFreshFor;
        public float alarmSpeed;
        public float updateSensorRate;
        public float walkSpeed;
        public float runSpeed;
        public float aimSpeed;
    }

    [SerializeField] private EventReference _sound;
    [SerializeField] private EnemyAnimationController _enemyAnimationController;
    [SerializeField] private NPCSetup _setupNormal;
    [SerializeField] private NPCSetup _setupAlarm;
   
    [SerializeField] Transform sensor;
    
    public List<int> enemySides;
    
    [SerializeField] BehaviourBase brain;
    [HideInInspector]
    public States currentState;
    public List<Transform> waypoints;
    NavMeshAgent agent;


    public List<Detected> enemies;
    float alarm;
    float lastSensorUpdate;
    bool isRunning;

   
    Automaton automaton;
    NPCSetup currentSetup;
    
    public void SetAlarm(bool active)
    {
        currentSetup = active ? _setupAlarm : _setupNormal;
    }
    

    // Start is called before the first frame update
    void Start()
    {


       
        agent = GetComponent<NavMeshAgent>();
        SetRun(false);
        enemies = new List<Detected>();
        automaton = brain.Init(this);
        currentSetup = _setupNormal;



    }

    public void Stop()
    {
       
        agent.destination = transform.position;
        _enemyAnimationController.Stop();
    }
    public void MoveTo(Vector3 pos)
    {
        if (isRunning)
            _enemyAnimationController.Run();
        else
            _enemyAnimationController.Walk();
        agent.SetDestination(pos);
    }

    public void AimAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pos.With(y: 0) - transform.position.With(y: 0)), Time.deltaTime*currentSetup.aimSpeed);
        //Debug.Log($"{name} aim at {pos}");
    }

    public void GetWeapon(bool get)
    {
        string word = get ? "get" : "remove";
        Debug.Log($"{name} {word} weapon");
        if (get) _enemyAnimationController.Shoot();
        else _enemyAnimationController.Stop();
    }

    
    public void SetRun(bool run)
    {
        isRunning = run;
        agent.speed = isRunning ? currentSetup.runSpeed : currentSetup.walkSpeed;
        AudioManager.Instance.PlayOneShot(_sound, transform.position);

        if (isRunning)
            _enemyAnimationController.Run();
        else
            _enemyAnimationController.Walk();
    }

    public bool hasPath => agent.hasPath;
    

    

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSensorUpdate > currentSetup.updateSensorRate)
        {
            UpdateSensor();
            lastSensorUpdate = Time.time;
        }
        automaton.Tick();
        
        enemies.RemoveAll(E => Time.time - E.detectedTime >currentSetup.isFreshFor);
        
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

            if (dist > currentSetup.visibleRange) continue;
            foreach(var point in entity.spotPoints)
            {
                Debug.DrawLine(sensor.position, point.position, Color.yellow);
                Vector3 viewLine = point.position - sensor.position;
                float angle = Vector3.Angle(sensor.forward, viewLine);
                
                if (angle > currentSetup.vievAngle) continue;
                if ((dist / currentSetup.visibleRange) + (angle / currentSetup.vievAngle) >currentSetup.perception * (alarm + Random.value)) continue;
                
                Debug.DrawLine(sensor.position, point.position, Color.blue);
                
                if (Physics.Raycast(sensor.position, viewLine, out RaycastHit hit, currentSetup.visibleRange) && hit.collider.transform.root != point.root) continue;
                alarm +=currentSetup.alarmSpeed;
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


