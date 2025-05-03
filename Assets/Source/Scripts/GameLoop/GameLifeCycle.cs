using FMODUnity;
using System;
using UnityEngine;

public class GameLifeCycle : MonoBehaviour
{
    [SerializeField] private LevelEscape _escapeCollider;
    [SerializeField] private SwitchableElement _moneyBar;
    [SerializeField] private AlertSoundController _alertSound;

    [Header("Time logic")]
    [SerializeField] private float _escapeTime;
    [SerializeField] private float _lootTime;
    [SerializeField] private TimerView _timerView;
    [SerializeField] private Timer _timer;

    [Header("Alert prefabs for Farkhad")]
    [SerializeField] private SwitchableElement _allertOff;
    [SerializeField] private SwitchableElement _allertOn;

    [Header("Tutorial")]
    [SerializeField] private Tutorial _tutorial;

    private Resource _money;
    private GameStage _stage;

    public event Action PlayerEscaped;
    public event Action PlayerLoose;

    private void OnDestroy()
    {
        _escapeCollider.PlayerEscaped -= OnPlayerEscaped;
        _timer.TimeIsGone -= OnEscapeTimeEnded;
    }

    public void Init(Resource money)
    {
        _money = money != null ? money : throw new ArgumentNullException(nameof(money));
        _stage = GameStage.Collecte;
        _timer.Init(_lootTime);
        _timer.StartTimer();
        _timerView.Init(_timer, _stage);

        _allertOff.Enable();
        _allertOn.Disable();

        _timer.TimeIsGone += ChangeStateToEscape;
        _money.ResourceFulfill += ChangeStateToEscape;
        _escapeCollider.PlayerEscaped += OnPlayerEscaped;
    }

    private void OnPlayerEscaped()
    {
        PlayerEscaped?.Invoke();
    }

    private void ChangeStateToEscape()
    {
        _money.ResourceFulfill -= ChangeStateToEscape;
        _timer.TimeIsGone -= ChangeStateToEscape;

        _allertOff.Disable();
        _allertOn.Enable();
        //AudioManager.Instance.StopAllSounds();
        _alertSound.Play();


        if (_money.Amount != _money.Maximum)
            _tutorial.PlayThreatment();
        else
            _tutorial.PlayComplete();

        _stage = GameStage.Escape;
        //_moneyBar.Disable();
        _escapeCollider.Enable();

        _timer.Init(_escapeTime);
        _timer.StartTimer();
        _timerView.Init(_timer, _stage);

        _timer.TimeIsGone += OnEscapeTimeEnded; 
    }

    private void OnEscapeTimeEnded()
    {
        PlayerLoose?.Invoke();
    }
}

public enum GameStage
{
    Collecte,
    Escape,
}
