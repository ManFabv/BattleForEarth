using Assets.Scripts.Common.Timer;
using System;

//TODO: this should be injected using dependency injection or service locator pattern
public class TimerActionManager
{
    public T CreateTimer<T>(float intervalOrDelayInSeconds, Action action) where T : ITimerAction, new()
    {
        T timer = new T();
        
        timer.Start(intervalOrDelayInSeconds, action);

        return timer;
    }
}
