using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSpace.AI;

public class BehaviourBase : ScriptableObject
{
    public virtual Automaton Init(NPCController controller)
    {
        return null;
    }
}
