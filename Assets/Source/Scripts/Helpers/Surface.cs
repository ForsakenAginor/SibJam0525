using NSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour, IDamageble
{
    public int surfaceType;
    [SerializeField] ParticleSystem hitEffect;
    

    public void TakeDamage(int value)
    {
        
    }

    public void GetHit(HitData hitData)
    {
        if (hitEffect != null)
        {
            ParticleSystem.EmitParams hitParams = new ParticleSystem.EmitParams();
            hitParams.position = hitData.point;
            hitParams.velocity = hitData.normal*0.001f;
            hitEffect.Emit(hitParams, 1);
        }
    }
}
