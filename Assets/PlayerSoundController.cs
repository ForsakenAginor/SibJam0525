using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSoundController : MonoBehaviour
{
    PlayerControll player;
   
    float lastStep;

    

    private void Update()
    {
        if (player == null)
        {
            player = GetComponent<PlayerControll>();
            player.onJump = () => PlaySound(jumpEvent, transform.position, null); ;
            player.onLand = () => PlaySound(landEvent, transform.position, null);
        }
        float speed = player.speedPar;

        if (speed>0.1f && Time.time - lastStep > 1 / (speed + 1))
        {
            if (player.floor != null)
            {
                Surface surf = player.floor.GetComponentInParent<Surface>();
                List<SoundEventParameter> parameters=new List<SoundEventParameter>();
                if (surf != null)
                {
                    parameters.Add(new SoundEventParameter("surface_type", surf.surfaceType));
                }
                lastStep = Time.time;
                if (player.isRunning)
                    PlaySound(runEvent, transform.position,parameters.ToArray());
                else if (player.isCrouching) PlaySound(crouchEvent, transform.position, parameters.ToArray());
                else PlaySound(walkEvent, transform.position, parameters.ToArray()); ;
            }

        }
    }

    void PlaySound(EventReference reference, Vector3 position, SoundEventParameter[] parameters)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(reference);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
        if (parameters != null)
        {
            foreach (var par in parameters)
            {
                instance.setParameterByName(par.name, par.value);
                //Debug.Log($"{par.name}:{par.value}");
            }
        }
        instance.start();
        instance.release();
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


    [SerializeField] EventReference walkEvent;
    [SerializeField] EventReference runEvent;
    [SerializeField] EventReference crouchEvent;
    [SerializeField] EventReference jumpEvent;
    [SerializeField] EventReference landEvent;



}
