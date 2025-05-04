using UnityEngine;
using NSpace;
using System;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CollectAnimation))]
public class Pickapable : InteractableBase
{

    [SerializeField] private int _value;
    [SerializeField] private float _randomBrackets = 0.3f;
    private PickapableOverlay _pickapableOverlay;
    private int _result;

    public event Action<Pickapable> Pickuped;

    public int Value => _result;

    public void Init(PickapableOverlay overlay)
    {
        SetOverlay(overlay);
        _pickapableOverlay = overlay;
        
        var multiplier = UnityEngine.Random.Range(1 - _randomBrackets, 1 + _randomBrackets);
        _result = (int)(_value * multiplier);
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
        _pickapableOverlay.SetText(Value);
    }
}
