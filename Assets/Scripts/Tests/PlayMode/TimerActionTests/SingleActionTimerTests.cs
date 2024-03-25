using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class SingleActionTimerTests
{
    SingleActionTimer timer;

    [SetUp]
    public void SetUp()
    {
        timer = new SingleActionTimer();
    }

    [UnityTest]
    public IEnumerator SingleActionTimerTestsWithEnumeratorPasses()
    {
        yield return null;
    }
}
