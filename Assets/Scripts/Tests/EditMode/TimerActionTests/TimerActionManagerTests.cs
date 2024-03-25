using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class TimerActionManagerTests
{
    TimerActionManager timerManager;

    [SetUp]
    public void SetUp()
    {
        timerManager = new TimerActionManager();
    }

    [Test]
    public void TimerActionTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }
}
