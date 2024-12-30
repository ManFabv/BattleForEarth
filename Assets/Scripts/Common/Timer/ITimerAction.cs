using System;

// added some comments
namespace Assets.Scripts.Common.Timer
{
    public interface ITimerAction : IDisposable
    {
        public void Start(float intervalInSeconds, Action action);
        public void Restart();
        public void Pause();
        public void Resume();
        public void Stop();
        public bool IsDisposed { get; }
    }
}