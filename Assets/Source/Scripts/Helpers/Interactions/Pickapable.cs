using UnityEngine;
using NSpace;
using System;

public class Pickapable : InteractableBase
{
    [SerializeField] private int _value;

    public event Action<Pickapable> Pickuped;

    public int Value => _value;

    public override void Interact(IInteractor _, bool pressed)
    {
        if(pressed)
        {
            Pickuped?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
