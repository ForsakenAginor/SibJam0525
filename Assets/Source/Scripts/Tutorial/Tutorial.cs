using DG.Tweening;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private readonly Dictionary<EventReference, float> _clips = new Dictionary<EventReference, float>();

    [SerializeField] private EventReference _intro;
    [SerializeField] private EventReference _2ndIntro;
    [SerializeField] private EventReference _threatment;
    [SerializeField] private EventReference _complete;

    [SerializeField] private Slider _slider;
    [SerializeField] private SwitchableElement _message;

    [SerializeField] private float _timeBeforeFirstMessage = 2f;
    [SerializeField] private float _timeBeforeSecondMessage = 10f;
    private float _timeSinceStart = 0;
    private bool _isFirstPlayed = false;
    private bool _isSecondPlayed = false;

    private Tween _animation;

    private void Start()
    {
        _clips.Add(_intro, 4.1f);
        _clips.Add(_2ndIntro, 7.1f);
        _clips.Add(_threatment, 6.4f);
        _clips.Add(_complete, 3.6f);
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
        AudioManager.Instance.PlayOneShot(_intro, transform.position);
        _animation = _slider.DOValue(1f, _clips[_intro]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void Play2ndIntro()
    {
        Normalize();
        _message.Enable();
        AudioManager.Instance.PlayOneShot(_2ndIntro, transform.position);
        _animation = _slider.DOValue(1f, _clips[_2ndIntro]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void PlayThreatment()
    {
        Normalize();
        _message.Enable();
        AudioManager.Instance.PlayOneShot(_threatment, transform.position);
        _animation = _slider.DOValue(1f, _clips[_threatment]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    public void PlayComplete()
    {
        Normalize();
        _message.Enable();
        AudioManager.Instance.PlayOneShot(_complete, transform.position);
        _animation = _slider.DOValue(1f, _clips[_complete]).SetEase(Ease.Linear).OnComplete(() => _message.Disable());
    }

    private void Normalize()
    {
        if (_animation != null)
            _animation.Kill();

        _slider.value = 0f;
        _message.Disable();
    }
}
