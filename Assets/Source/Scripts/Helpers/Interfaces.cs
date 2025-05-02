using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NSpace
{
    public interface IEntity
    {
        public string name { get; }
        Transform transform { get; }
    }

    public interface IFocusable : IEntity
    {
        void Focus(IInteractor actor);
        void Unfocus(IInteractor actor);
    }

    public interface IInteractor:IEntity
    {

    }

    public interface IWeapon
    {
        void Fire();
        
    }

    public interface IDetectible : IEntity
    {
        List<Transform> spotPoints { get; }
        int side { get; }

    }

    public interface IInteractable:IFocusable
    {
        void Interact(IInteractor actor, bool pressed);
    }

    public interface IHitSurface
    {
        void Hit(HitData hit, float[] damage);
    }

    public struct HitData
    {
        public Vector3 point;
        public Vector3 normal;
        public Vector3 velocity;
        public float distance;
        public Collider collider;
    }
    public class Detected
    {
        public IDetectible entity;
        public float detectedTime;
        public float age => Time.time - detectedTime;
        public Vector3 lastKnownPos;
    }

    public enum States
    {
        idle,
        patrol,
        search,
        chase,
        atack
    }



}
