using NUnit.Framework;
using System;
using System.Collections;
using UniRx;
using UnityEngine.TestTools;

public class TimerActionBaseTests
{
    private FakeStartTimerEmptyForTimerActionBase timer;
    private Action nullAction = () => { };

#region Setup
    [SetUp]
    public void SetUp()
    {
        timer = new FakeStartTimerEmptyForTimerActionBase();
    }
#endregion

#region Tests
    [UnityTest]
    public IEnumerator Start_AfterCalled_SavesCorrectIntervalAndAction()
    {
        timer.Start(1, nullAction);

        yield return null;
        
        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Start_AfterCalledWithNullAction_DoesntTriggerException()
    {
        yield return null;
        
        Assert.DoesNotThrow(() => timer.Start(1, null));
    }

    [UnityTest]
    public IEnumerator Start_AfterCalledWithNullActionAndNegativeInterval_DoesntTriggerException()
    {
        yield return null;
        
        Assert.DoesNotThrow(() => timer.Start(-1, null));
    }

    [UnityTest]
    public IEnumerator Start_AfterCalledWithNegativeInterval_ItSetsIntervalInZero()
    {
        timer.Start(-1, nullAction);

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
    }

    [UnityTest]
    public IEnumerator Start_AfterCalledWithNegativeIntervalAndNegativeAction_ItSetsIntervalInZeroAndActionToNull()
    {
        timer.Start(-1, null);

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Restart_AfterCalledWithNegativeIntervalAndNegativeAction_ItSetsIntervalInZeroAndActionToNull()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Restart();

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Restart_AfterCalledWithValidValues_SavesCorrectIntervalAndAction()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Restart();

        yield return null;

        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Pause_AfterCalledWithValidValues_IsDisposed()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Pause();

        yield return null;

        Assert.IsNull(GetTimerDisposableValue());
        Assert.IsTrue(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Pause_AfterCalledWithValidValues_ItSetsIntervalInZeroAndActionToNull()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Pause();

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Pause_AfterCalledWithInvalidValues_ItSetsIntervalInZeroAndActionToNull()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Pause();

        yield return null;

        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Resume_AfterCalledWithValidValues_InternalTimerObserverIsNotNull()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Pause();

        yield return null;

        timer.Resume();

        yield return null;

        Assert.IsNotNull(GetTimerDisposableValue());
    }

    [UnityTest]
    public IEnumerator Resume_AfterCalledWithValidValues_ItSetsCorrectValues()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Pause();

        yield return null;

        timer.Resume();

        yield return null;

        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Resume_CalledAfterCalledStartWithValidValues_ItSetsCorrectValues()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Resume();

        yield return null;

        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Resume_CalledAfterCalledStartWithInvalidValues_ItSetsIntervalToZeroAndActionToNull()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Resume();

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Resume_CalledAfterCalledStartWithInvalidValues_IsNotDisposed()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Resume();

        yield return null;

        Assert.IsFalse(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Resume_CalledWithoutCallingStart_IsNotDisposed()
    {
        timer.Resume();

        yield return null;

        Assert.IsFalse(timer.IsDisposed);
    }

    [UnityTest]
    public IEnumerator Resume_CalledWithoutCallingStart_ItHasIntervalAsZeroAndActionAsNull()
    {
        yield return null;

        timer.Resume();

        yield return null;

        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Stop_CalledAfterCalledStartWithValidValues_IsDisposedAndHasValidValues()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Stop();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }

    [UnityTest]
    public IEnumerator Stop_CalledAfterCalledStartWithInvalidValues_IsDisposedAndItSetsIntervalAsZeroAndActionAsNull()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Stop();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Stop_CalledWithoutCallingStart_DoesntThrowException()
    {
        yield return null;

        Assert.DoesNotThrow(() => timer.Stop());
    }

    [UnityTest]
    public IEnumerator Dispose_CalledWithoutCallingStart_IsDisposedAndItSetsIntervalAsZeroAndActionAsNull()
    {
        yield return null;

        timer.Stop();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Dispose_CalledAfterCalledStartWithInvalidValues_IsDisposedAndItSetsIntervalAsZeroAndActionAsNull()
    {
        timer.Start(-1, null);

        yield return null;

        timer.Stop();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
        Assert.AreEqual(0, GetIntervalInSecondsValue());
        Assert.IsNull(GetActionValue());
    }

    [UnityTest]
    public IEnumerator Dispose_CalledAfterCalledStartWithValidValues_IsDisposedAndItRetainsValidValues()
    {
        timer.Start(1, nullAction);

        yield return null;

        timer.Dispose();

        yield return null;

        Assert.IsTrue(timer.IsDisposed);
        Assert.AreEqual(1, GetIntervalInSecondsValue());
        Assert.AreSame(nullAction, GetActionValue());
    }
#endregion

#region Private methods
    private float GetIntervalInSecondsValue()
    {
        return ReflectionUtils.GetFieldValue<float>("intervalInSeconds", timer);
    }

    private Action GetActionValue()
    {
        return ReflectionUtils.GetFieldValue<Action>("action", timer);
    }

    private IDisposable GetTimerDisposableValue()
    {
        return ReflectionUtils.GetFieldValue<IDisposable>("timerDisposable", timer);
    }
#endregion

#region Fake Classes
    private class FakeStartTimerEmptyForTimerActionBase : TimerActionBase
    {
        protected override void StartTimer(float intervalInSeconds, Action action)
        {
            //we create a test observable which waits for next frame
            timerDisposable = Observable.NextFrame(FrameCountType.EndOfFrame)
                .TakeUntil(pauseSubject.Where(paused => !paused))
                .Subscribe(_ => { });
        }
    }
#endregion
}