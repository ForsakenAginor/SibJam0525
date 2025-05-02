using UnityEngine;
using NSpace;
using System;

[RequireComponent(typeof(CollectAnimation))]
public class Pickapable : InteractableBase
{
    [SerializeField] private int _value;
    private PickapableOverlay _pickapableOverlay;

    public event Action<Pickapable> Pickuped;

    public int Value => _value;

    public void Init(PickapableOverlay overlay)
    {
        SetOverlay(overlay);
        _pickapableOverlay = overlay;
    }

    public override void Interact(IInteractor _, bool pressed)
    {
        if (pressed)
        {
            Pickuped?.Invoke(this);
            GetComponent<CollectAnimation>().StartAnimation();
            Destroy(this);
        }
    }

    public override void Focus(IInteractor actor)
    {
        base.Focus(actor);
        _pickapableOverlay.SetText(_value);
    }
}
