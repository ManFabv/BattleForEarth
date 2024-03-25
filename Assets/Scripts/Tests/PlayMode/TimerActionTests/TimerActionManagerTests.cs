using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class TimerActionManagerTests
{
    TimerActionManager timerManager;

    [SetUp]
    public void SetUp()
    {
        timerManager = new TimerActionManager();
    }

    [UnityTest]
    public IEnumerator SingleActionTimerTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
