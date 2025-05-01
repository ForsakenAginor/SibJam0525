using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelEscape : SwitchableElement
{
    public event Action PlayerEscaped;

    private void OnTriggerEnter(Collider other)
    {
        if(other is CharacterController)
            PlayerEscaped?.Invoke();
    }
}
