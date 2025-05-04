using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using FMOD.Studio;

public class fmodMixer : MonoBehaviour
{

    [SerializeField] Slider masterVolume;
    [SerializeField] Slider sfxSlider;    
    [SerializeField] Slider musicSlider;

    string musicgrpname = "bus:/Master_Amb";
    string sfxgrpname = "bus:/Master_SFX";
    string masterBusString = "bus:/";

    FMOD.Studio.Bus masterBus;
    FMOD.Studio.Bus sfxgrp;
    FMOD.Studio.Bus muzicgrp;
    private void OnEnable()
    {
        masterBus = RuntimeManager.GetBus(masterBusString);
        sfxgrp = RuntimeManager.GetBus(sfxgrpname);
        muzicgrp = RuntimeManager.GetBus(musicgrpname);

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
