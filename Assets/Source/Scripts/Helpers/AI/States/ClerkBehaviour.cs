using NSpace.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClerkBehaviour : BehaviourBase
{
    [SerializeField] float fleeCooldown;
    [SerializeField] float lookForGuardRadius;


    public override Automaton Init(NPCController controller)
    {
        return null;
    }
}
