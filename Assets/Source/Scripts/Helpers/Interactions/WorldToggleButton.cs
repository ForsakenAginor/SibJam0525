using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NSpace;

public class WorldToggleButton : InteractableBase
{
    [SerializeField]
    UnityEvent<IInteractor> onPressIn;


    [SerializeField]
    UnityEvent<IInteractor> OnPressOut;

    

    [SerializeField] bool status;
    // Start is called before the first frame update
    public override void Interact(IInteractor actor, bool pressed)
    {        
        if (pressed) {
            if (status) OnPressOut?.Invoke(actor);
            else onPressIn?.Invoke(actor);
            status = !status;
        }
        
    }
}
