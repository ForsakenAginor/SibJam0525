using NSpace;
using NSpace.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GuardBrain", menuName ="Behaviours/Guard")]
public class GuardBehaviour:BehaviourBase
{
    [SerializeField] float searchRadius;
    [SerializeField] float searchTimeout;
    [SerializeField] float searchPeriod;
    [SerializeField] float minDistance;
    [SerializeField] float fireRate;
    [SerializeField] float fireRange;
    [SerializeField] float keepTime;
    

    public override Automaton Init(NPCController controller)
    {
        IWeapon arms = controller.transform.GetComponent<IWeapon>();
        Search search = new Search(controller, searchPeriod, searchRadius,searchTimeout);
        Chase chase = new Chase(controller, minDistance, keepTime);
        Idle idle = new Idle(controller);
        Patrol patrol = new Patrol(controller);
        Atack atack = new Atack(controller, fireRate, arms);
        Automaton automaton = new Automaton();

        System.Func<bool> spoted = () => { return controller.enemies.Count > 0; };
        System.Func<bool> lost = () => { return controller.enemies.Count <= 0 && !controller.hasPath; };
        System.Func<bool> timeout = () => search.complete;
        
        
        automaton.AddTransition(search, chase, spoted);
        automaton.AddTransition(chase, search, lost);
        automaton.AddTransition(idle, chase, spoted);
        automaton.AddTransition(search, idle, timeout);
        automaton.AddTransition(idle, patrol, () => controller.waypoints.Count > 0);
        automaton.AddTransition(patrol, chase, spoted);
        automaton.AddTransition(chase, atack, () => arms != null
            && controller.enemies.Count > 0 && controller.enemies[0].age < 2
            && Vector3.Distance(controller.transform.position, controller.enemies[0].entity.transform.position) < fireRange * 0.8f);
        automaton.AddTransition(atack, chase,()=> arms==null || controller.enemies[0].age>2
            || Vector3.Distance(controller.transform.position, controller.enemies[0].entity.transform.position)>fireRange*1.2f);
        automaton.SetState(idle);
        return automaton;

    }



}
