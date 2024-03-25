using Assets.Scripts.Common.Timer;
using System;
using UniRx;

public abstract class TimerActionBase : ITimerAction
{
    protected IDisposable timerDisposable = default;
    protected Subject<bool> pauseSubject = new Subject<bool>();
    protected float intervalInSeconds = 0;
    protected Action action = default;

    protected abstract void StartTimer(float intervalInSeconds, Action action);

    public void Dispose()
    {
        timerDisposable?.Dispose();
        pauseSubject.OnNext(false);
    }

    public void Start(float intervalInSeconds, Action action)
    {
        this.intervalInSeconds = intervalInSeconds;
        this.action = action;

        StartTimer(intervalInSeconds, action);
    }

    public void Pause()
    {
        timerDisposable?.Dispose();
        pauseSubject.OnNext(true);
    }

    public void Resume()
    {
        Start(intervalInSeconds, action);
        pauseSubject.OnNext(false);
    }

    public void Stop()
    {
        Dispose();
    }

    public void Restart()
    {
        Stop();
        Start(intervalInSeconds, action);
    }
}