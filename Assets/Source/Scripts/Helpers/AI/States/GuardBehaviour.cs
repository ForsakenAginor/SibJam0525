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

    public override Automaton Init(NPCController controller)
    {
        Search search = new Search(controller, searchPeriod, searchRadius,searchTimeout);
        Chase chase = new Chase(controller);
        Idle idle = new Idle(controller);
        Patrol patrol = new Patrol(controller);
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
        automaton.SetState(idle);
        return automaton;

    }



}
