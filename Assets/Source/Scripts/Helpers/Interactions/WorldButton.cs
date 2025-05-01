using NSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldButton : InteractableBase
{

    [SerializeField]
    UnityEvent<IInteractor> onPressed;

  
    [SerializeField]
    UnityEvent<IInteractor> onReleased;

    [SerializeField]
    UnityEvent<bool> onAction;

    bool pressed;
    // Start is called before the first frame update
    public override void Interact(IInteractor actor, bool pressed)
    {
        this.pressed = pressed;
        if (pressed) { onPressed?.Invoke(actor);}
        else onReleased?.Invoke(actor);
        onAction?.Invoke(pressed);
    }

    public override void Unfocus(IInteractor actor)
    {
        base.Unfocus(actor);
        if(pressed) {onReleased?.Invoke(actor); onAction?.Invoke(false);}
        pressed = false;

    }
}
