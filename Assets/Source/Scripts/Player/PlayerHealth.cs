using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour, IDamageble
{
    private Resource _health;
    [SerializeField] UnityEngine.Events.UnityEvent<float> onDamage;

    public void Init(Resource health)
    {
        _health = health != null ? health : throw new ArgumentNullException(nameof(health));
        
    }

    public void TakeDamage(int value)
    {
        _health.Spent(value);
        onDamage?.Invoke(_health.Amount);
    }
}
