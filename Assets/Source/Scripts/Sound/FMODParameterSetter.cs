using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FMODParameterSetter : MonoBehaviour
{
    [SerializeField] string defaultName;
    FMOD.Studio.PARAMETER_DESCRIPTION p;
    [SerializeField] StudioEventEmitter emitter;

    private void Start()
    {
        var description = FMODUnity.RuntimeManager.GetEventDescription(emitter.EventReference);        
        description.getParameterDescriptionByName(defaultName, out p);
       
    }

    public void SetParameter(string name, float value)
    {
        emitter.EventInstance.setParameterByName(name, value);
        
    }
    public void SetParameter( float value)
    {
        emitter.EventInstance.setParameterByID(p.id, value);

    }
}
