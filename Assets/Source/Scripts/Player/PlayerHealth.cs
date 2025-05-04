using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour, IDamageble
{
    private Resource _health;

    public void Init(Resource health)
    {
        _health = health != null ? health : throw new ArgumentNullException(nameof(health));
    }

    public void TakeDamage(int value)
    {
        float chanceToAvoidDamage = 1 - (float)_health.Amount / _health.Maximum;
        float seed = UnityEngine.Random.Range(0f, 1f);

        if (seed > chanceToAvoidDamage)
            _health.Spent(value);
    }
}
