using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AlertSoundController : MonoBehaviour
{
    [SerializeField] private EventReference _alertSoundEvent;

    private FMOD.Studio.EventInstance staminaSoundInstance;

    public void Play()
    {
        staminaSoundInstance = RuntimeManager.CreateInstance(_alertSoundEvent);
        staminaSoundInstance.setParameterByName("AllarmActive", 1);
        staminaSoundInstance.start();
    }

    private void OnDestroy()
    {
        staminaSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        staminaSoundInstance.release();
    }
}