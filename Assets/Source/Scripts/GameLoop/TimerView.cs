using System;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    private const string AlertDisabled = "СИГНАЛИЗАЦИЯ ВКЛЮЧИТСЯ ЧЕРЕЗ:";
    private const string AlertEnabled = "ТРЕВОГА! КИБЕРПОЛИЦИЯ ПРИБУДЕТ ЧЕРЕЗ:";

    [SerializeField] private TMP_Text _timerField;
    [SerializeField] private TMP_Text _labelField;

    private ITimer _timer;

    private void OnDestroy()
    {
        _timer.TimeChanged -= OnTimeChanged;
    }

    public void Init(ITimer timer, GameStage stage)
    {
        if (_timer != null)
            _timer.TimeChanged -= OnTimeChanged;

        _timer = timer;
        _timer.TimeChanged += OnTimeChanged;

        if (stage == GameStage.Collecte)
            _labelField.text = AlertDisabled;
        else if (stage == GameStage.Escape)
            _labelField.text = AlertEnabled;
        else
            throw new NotImplementedException();
    }

    private void OnTimeChanged(float value)
    {
        _timerField.text = value.ToString("0");
    }
}
