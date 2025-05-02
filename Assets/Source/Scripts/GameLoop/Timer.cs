using System;
using UnityEngine;

public class Timer : MonoBehaviour, ITimer
{
    private float _timeToEnd;
    private bool _isWorking;

    public event Action<float> TimeChanged;
    public event Action TimeIsGone;

    private void Update()
    {
        if (_isWorking == false)
            return;

        _timeToEnd -= Time.deltaTime;
        _timeToEnd = Mathf.Clamp(_timeToEnd, 0f, _timeToEnd);
        TimeChanged?.Invoke(_timeToEnd);

        if (_timeToEnd > 0)
            return;

        _isWorking = false;
        TimeIsGone?.Invoke();
    }

    public void Init(float time)
    {
        _timeToEnd = time;
    }

    public void StartTimer()
    {
        _isWorking = true;
    }
}
