using FMODUnity;
using UnityEngine;

public class ResourceSoundController : MonoBehaviour
{
    [SerializeField] private EventReference staminaSoundEvent;
    [SerializeField] private Transform _player;
    [SerializeField] private string _parameterName = "Stamina";

    private FMOD.Studio.EventInstance staminaSoundInstance;
    private IResource _resource;

    private void Start()
    {
        staminaSoundInstance = RuntimeManager.CreateInstance(staminaSoundEvent);
        RuntimeManager.AttachInstanceToGameObject(staminaSoundInstance, _player);
        staminaSoundInstance.setParameterByName(_parameterName, 100);
        staminaSoundInstance.start(); 
    }

    public void Init(IResource resource)
    {
        _resource = resource;
        _resource.ResourcesAmountChanged += SetStamina;
    }

    private void SetStamina()
    {
        int value = _resource.Amount * 100 / _resource.Maximum;
        staminaSoundInstance.setParameterByName(_parameterName, value);
    }

    private void OnDestroy()
    {
        _resource.ResourcesAmountChanged -= SetStamina;
        // Плавно останавливаем и освобождаем ресурсы
        staminaSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        staminaSoundInstance.release();
    }
}
