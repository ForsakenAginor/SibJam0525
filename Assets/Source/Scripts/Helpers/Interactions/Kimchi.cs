using UnityEngine;
using NSpace;

public class Kimchi : InteractableBase
{
    [SerializeField] private int _price;
    [SerializeField] private int _heal;

    private KimchiOverlay _pickapableOverlay;
    private Resource _health;
    private Resource _money;

    public void Init(KimchiOverlay overlay, Resource health, Resource money)
    {
        _health = health;
        _money = money;
        SetOverlay(overlay);
        _pickapableOverlay = overlay;
    }

    public override void Interact(IInteractor _, bool pressed)
    {
        if (pressed)
        {
            if(_money.Amount >= _price)
            {
                _money.Spent(_price);
                _health.Add(_heal);
            }
        }
    }

    public override void Focus(IInteractor actor)
    {
        base.Focus(actor);
        _pickapableOverlay.SetText(_price);
    }
}
