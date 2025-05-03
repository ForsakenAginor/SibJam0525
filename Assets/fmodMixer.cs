using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class fmodMixer : MonoBehaviour
{

    [SerializeField] Slider masterVolume;
    [SerializeField] string sfxgrpname;
    [SerializeField] Slider sfxSlider;
    [SerializeField] string musicgrpname;
    [SerializeField] Slider musicSlider;
    string masterBusString = "bus:/"; 
    
    FMOD.Studio.Bus masterBus;
    FMOD.Studio.Bus sfxgrp;
    FMOD.Studio.Bus muzicgrp;
    private void OnEnable()
    {
        masterBus = RuntimeManager.GetBus(masterBusString);
        sfxgrp = RuntimeManager.GetBus(masterBusString+ sfxgrpname);
        muzicgrp = RuntimeManager.GetBus(masterBusString + musicgrpname);
        
        masterBus.getVolume(out float master);
        masterVolume.SetValueWithoutNotify(master);
        sfxgrp.getVolume(out float sfxVolume);
        muzicgrp.getVolume(out float muzicVolume);
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        sfxSlider.SetValueWithoutNotify(sfxVolume);
        musicSlider.SetValueWithoutNotify(muzicVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void OnDisable()
    {
        masterVolume.onValueChanged.RemoveListener(SetMasterVolume);
    }

    public void SetMasterVolume(float volume)
    { 
        masterBus.setVolume(volume);
    }
    public void SetSfxVolume(float volume)
    {
        sfxgrp.setVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        muzicgrp.setVolume(volume);
    }
}
