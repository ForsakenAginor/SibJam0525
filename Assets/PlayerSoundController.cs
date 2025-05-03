using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] EventReference[] storedEvents;
    [SerializeField] SoundEventParameter[] storedEventparameters;

    public void PlayStoredEvent(int index)
    {
        if (storedEvents.Length <= 0) return;
        index = ((index % storedEvents.Length)+storedEvents.Length)%storedEvents.Length;
        PlaySound(storedEvents[index],transform.position,storedEventparameters);
    }

    public void SetStoredParameter(int index, float value)
    {
        storedEventparameters[index].value = value;
    }
    public void SetStoredParameter(string name, float value)
    {
        for(int i = 0; i<storedEventparameters.Length; i++)
        {
            if (storedEventparameters[i].name==name) storedEventparameters[i].value = value;
        }
        
    }

    public void PlaySound(EventReference reference, Vector3 position, SoundEventParameter[] parameters)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(reference);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
        if (parameters != null)
        {
            foreach (var par in parameters)
            {
                instance.setParameterByName(par.name, par.value);
                Debug.Log(par.name+":"+par.value);
                //Debug.Log($"{par.name}:{par.value}");
            }
        }
        instance.start();
        instance.release();
    }
   
    


}
[System.Serializable]
public struct SoundEventParameter
{
    public string name;
    public float value;

    public SoundEventParameter(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}
