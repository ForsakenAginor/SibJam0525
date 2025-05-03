using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private readonly Dictionary<AudioClip, float> _clips = new Dictionary<AudioClip, float>();

    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip _2ndIntro;
    [SerializeField] private AudioClip _threatment;
    [SerializeField] private AudioClip _complete;

    [SerializeField] private Slider _slider;
    [SerializeField] private SwitchableElement _message;
    [SerializeField] private AudioSource _source;

    [SerializeField] private float _timeBeforeFirstMessage = 5f;
    [SerializeField] private float _timeBeforeSecondMessage = 15f;
    private float _timeSinceStart = 0;
    private bool _isFirstPlayed = false;
    private bool _isSecondPlayed = false;

    private Tween _animation;

    private void Start()
    {
        _clips.Add(_intro, _intro.length);
        _clips.Add(_2ndIntro, _2ndIntro.length);
        _clips.Add(_threatment, _threatment.length);
        _clips.Add(_complete, _complete.length);
    }

    private void Update()
    {
        if (_isSecondPlayed)
            return;

        _timeSinceStart += Time.deltaTime;

        if (_isFirstPlayed == false && _timeSinceStart > _timeBeforeFirstMessage)
        {
            PlayIntro();
            _isFirstPlayed = true;
        }

        if (_isSecondPlayed == false && _timeSinceStart > _timeBeforeSecondMessage)
        {
            Play2ndIntro();
            _isSecondPlayed = true;
        }
    }

    public void PlayIntro()
    {
        Normalize();
        _message.Enable();
        _source.clip = _intro;
        _source.Play();
        _animation = _slider.DOValue(1f, _clips[_intro]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void Play2ndIntro()
    {
        Normalize();
        _message.Enable();
        _source.clip = _2ndIntro;
        _source.Play();
        _animation = _slider.DOValue(1f, _clips[_2ndIntro]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void PlayThreatment()
    {
        Normalize();
        _message.Enable();
        _source.clip = _threatment;
        _source.Play();
        _animation = _slider.DOValue(1f, _clips[_threatment]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void PlayComplete()
    {
        Normalize();
        _message.Enable();
        _source.clip = _complete;
        _source.Play();
        _animation = _slider.DOValue(1f, _clips[_complete]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    private void Normalize()
    {
        _slider.value = 0f;
        _source.Stop();

        if (_animation != null)
            _animation.Kill();

        _message.Disable();
    }
}
