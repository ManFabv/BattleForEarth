using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class TimerActionBaseTests
{
    [SetUp]
    public void SetUp()
    {
    }

    [UnityTest]
    public IEnumerator SingleActionTimerTestsWithEnumeratorPasses()
    {
        yield return null;
    }
}