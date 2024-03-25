using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class RepeatingActionTimerTests
{
    RepeatingActionTimer timer;

    [SetUp]
    public void SetUp()
    {
        timer = new RepeatingActionTimer();
    }

    [UnityTest]
    public IEnumerator SingleActionTimerTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
