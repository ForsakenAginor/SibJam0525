using UnityEngine;
using NSpace;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] SwitchableElement _overlay;

    public void SetOverlay(SwitchableElement overlay)
    {
        _overlay = overlay;
    }

    public virtual void Focus(IInteractor actor)
    {
        if(_overlay != null) { _overlay.Enable(); }
    }

    public abstract void Interact(IInteractor actor, bool pressed);

    public virtual void Unfocus(IInteractor actor)
    {
        if(_overlay != null) { _overlay.Disable(); }
    }
}
