using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class RepeatingActionTimerTests
{
    private RepeatingActionTimer timer;
    private FakeNullActionObject fakeNullActionObject;
    private float timerInterval = 0.5f;
    private WaitForSeconds waitForOneSecondTimerEnd = new WaitForSeconds(0.6f);

#region Setup
    [SetUp]
    public void SetUp()
    {
        timer = new RepeatingActionTimer();
        fakeNullActionObject = Substitute.For<FakeNullActionObject>();
        fakeNullActionObject.nullAction = Substitute.For<Action>();
    }

    [TearDown]
    public void TearDown()
    {
        timer.Dispose();
    }
#endregion

#region Tests
    [UnityTest]
    public IEnumerator Start_AfterCalledAndWaitTwoIntervals_ActionIsExecutedOnePerInterval()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        fakeNullActionObject.nullAction.Received(1).Invoke();

        yield return waitForOneSecondTimerEnd;

        fakeNullActionObject.nullAction.Received(2).Invoke();

        yield return waitForOneSecondTimerEnd;

        fakeNullActionObject.nullAction.Received(3).Invoke();
    }

    [UnityTest]
    public IEnumerator Start_AfterCalledAndWaitTwoIntervals_TimerIsNotDisposed()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Restart_AfterCalledAndWaitTwoIntervals_TimerIsNotDisposed()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);

        yield return waitForOneSecondTimerEnd;

        timer.Restart();

        yield return null;

        Assert.IsFalse(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Pause_AfterCalledAndWaitTwoIntervals_TimerIsDisposed()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);

        yield return waitForOneSecondTimerEnd;

        timer.Pause();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Resume_AfterCalledAndWaitOneInterval_TimerIsNotDisposed()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);

        yield return waitForOneSecondTimerEnd;

        timer.Pause();

        yield return waitForOneSecondTimerEnd;

        Assert.IsTrue(timer.IsDisposed);

        yield return null;

        timer.Resume();

        yield return waitForOneSecondTimerEnd;

        Assert.False(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Stop_AfterCalledAndWaitTwoIntervals_TimerIsDisposed()
    {
        timer.Start(timerInterval, fakeNullActionObject.nullAction);

        yield return waitForOneSecondTimerEnd;

        Assert.IsFalse(timer.IsDisposed);

        yield return waitForOneSecondTimerEnd;

        timer.Stop();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
    }
#endregion

#region Fake Classes
    public class FakeNullActionObject
    {
        public Action nullAction;
    }
#endregion
}