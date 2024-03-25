using System;
using UniRx;

public class RepeatingActionTimer : TimerActionBase
{
    protected override void StartTimer(float intervalInSeconds, Action action)
    {
        timerDisposable = Observable.Interval(TimeSpan.FromSeconds(intervalInSeconds))
            .TakeUntil(pauseSubject.Where(paused => !paused))
            .Subscribe(_ =>
            {
                action?.Invoke();
            });
    }
}