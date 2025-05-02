using System;

public interface ITimer
{
    event Action<float> TimeChanged;
}