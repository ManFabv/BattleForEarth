using System;
using UniRx;

public class SingleActionTimer : TimerActionBase
{
    protected override void StartTimer(float intervalInSeconds, Action action)
    {
        timerDisposable = Observable.Timer(TimeSpan.FromSeconds(intervalInSeconds))
            .TakeUntil(pauseSubject.Where(paused => !paused))
            .Subscribe(_ =>
            {
                action?.Invoke();
                Dispose();
            });
    }
}