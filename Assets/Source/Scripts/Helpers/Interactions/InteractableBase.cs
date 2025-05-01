using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NSpace;
using UnityEngine.Events;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject overlay;

    public virtual void Focus(IInteractor actor)
    {
        if(overlay != null) { overlay.SetActive(true); }
    }

    public abstract void Interact(IInteractor actor, bool pressed);

    public virtual void Unfocus(IInteractor actor)
    {
        if(overlay != null) { overlay.SetActive(false); }
    }
}
