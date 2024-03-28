using Assets.Scripts.Common.Timer;
using System;
using UniRx;

public abstract class TimerActionBase : ITimerAction
{
#region Protected properties
    protected IDisposable timerDisposable = default;
    protected Subject<bool> pauseSubject = new Subject<bool>();
    protected float intervalInSeconds = 0;
    protected Action action = default;
#endregion

#region Public properties
    public bool IsDisposed => !pauseSubject.HasObservers;
#endregion

#region Abstract Methods
    protected abstract void StartTimer(float intervalInSeconds, Action action);
#endregion

#region ITimerAction Interface methods
    public void Start(float intervalInSeconds, Action action)
    {
        this.intervalInSeconds = Math.Max(intervalInSeconds, 0); //we don't allow negative values
        this.action = action;
        
        pauseSubject.OnNext(false);

        StartTimer(intervalInSeconds, action);
    }

    public void Restart()
    {
        Resume();
    }

    public void Pause()
    {
        Stop();
    }

    public void Resume()
    {
        Stop();
        Start(intervalInSeconds, action);
    }

    public void Stop()
    {
        Dispose();
    }
#endregion

#region IDisposable methods
    public void Dispose()
    {
        timerDisposable?.Dispose();
        timerDisposable = null;
        pauseSubject.OnNext(true);
    }
#endregion
}