using UnityEngine;
using System;
using System.Collections.Generic;

namespace NSpace.AI
{
    public class Automaton
    {

        Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        List<Transition> curretnTransitions = new List<Transition>();
        List<Transition> anyTransitions = new List<Transition>();
        static List<Transition> empty = new List<Transition>(0);

        IState currentState;

        public void Tick()
        {
            var transition =GetTransition();
            
            if (transition != null)
                SetState(transition.to);
            currentState?.OnUpdate();

        }

        Transition GetTransition()
        {
            foreach(var trans in anyTransitions)
            {
                if (trans.condition())
                    return trans;
            }
            foreach(var trans in curretnTransitions)
            {
                if (trans.condition()) return trans;
            }
            return null;
        }

        public void SetState(IState state)
        {
            if (state == currentState)
                return;

            currentState?.OnExit();
            currentState = state;
            if (currentState == null) return;

            transitions.TryGetValue(currentState.GetType(), out curretnTransitions);
            if (curretnTransitions == null) curretnTransitions = empty;

            currentState.OnEnter();


        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if(transitions.TryGetValue(from.GetType(), out var thistransitions) == false)
            {
                thistransitions = new List<Transition>();
                transitions[from.GetType()] = thistransitions;
            }
            thistransitions.Add(new Transition(to, condition));
            
        }
        
        public void AddAnyTransition(IState state, Func<bool> condition)
        {
            anyTransitions.Add(new Transition(state, condition));
        }


        class Transition
        {
            public Transition( IState to, Func<bool> condition)
            {
                
                this.to = to;
                this.condition = condition;
            }

           
            public IState to { get; private set; }
            public Func<bool> condition { get; private set;}

            
        }




    }
}