using Assets.Scripts.Common.Timer;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.TestTools;

public class TimerActionManagerTests
{
    TimerActionManager timerManager;
    private Action nullAction = () => { };
    private float timerInterval = 0.5f;
    private ITimerAction timer;

    [SetUp]
    public void SetUp()
    {
        timerManager = new TimerActionManager();
    }

    [TearDown]
    public void TearDown()
    {
        timer.Dispose();
    }

    [UnityTest]
    public IEnumerator CreateTimer_AfterCallingWithValidParameters_ReturnsAValidRepeatingActionTimer()
    {
        timer = timerManager.CreateTimer<RepeatingActionTimer>(timerInterval, nullAction);
        Assert.NotNull(timer);
        yield return null;
        Assert.IsFalse(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator CreateTimer_AfterCallingWithValidParameters_ReturnsAValidSingleActionTimer()
    {
        timer = timerManager.CreateTimer<SingleActionTimer>(timerInterval, nullAction);
        Assert.NotNull(timer);
        yield return null;
        Assert.IsFalse(timer.IsDisposed);
    }
}
