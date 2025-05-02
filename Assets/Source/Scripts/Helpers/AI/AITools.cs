using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSpace.AI
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdate();

    }

    public interface ITask
    {
        void Tick();
        object Completed();
    }



    public struct Waypoint
    {
        public Vector3 position { get; private set; }
        public float waitTime { get; private set; }
        public float minDist { get; private set; }
        public bool cyclic { get; private set; }

        public Waypoint(Vector3 position, float waitTime, float minDist, bool cyclic)
        {
            this.position = position;
            this.waitTime = waitTime;
            this.minDist = minDist;
            this.cyclic = cyclic;
        }
    }

    

   
    
}