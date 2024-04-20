using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ComboBarConfigTests
{
    //this are the default values for the ScriptableObjects
    //Because we are creating an instance, we don't modify these
    //values and we use the defaults
    private const int _maxComboValue = 10;
    private const int _normalIncrement = 1;

    private ComboBarConfig _comboBarConfig;

    [SetUp]
    public void SetUp()
    {
        _comboBarConfig = ScriptableObject.CreateInstance<ComboBarConfig>();
    }

    [Test]
    public void When_ConfigIsCreated_IsNotNull()
    {
        Assert.NotNull(_comboBarConfig);
    }

    [Test]
    public void CalculateComboIncrement_WhenTargetIsNull_AppliesZeroIncrement()
    {
        int increment = _comboBarConfig.CalculateComboIncrement(null);

        Assert.AreEqual(0, increment);
    }

    [Test]
    public void CalculateComboIncrement_WhenTargetIsNotVulnerable_AppliesZeroIncrement()
    {
        DamageTypeConfig targetDamageTypeConfig = ScriptableObject.CreateInstance<DamageTypeConfig>();

        int increment = _comboBarConfig.CalculateComboIncrement(targetDamageTypeConfig);

        Assert.AreEqual(0, increment);
    }

    [Test]
    public void CalculateComboIncrement_WhenTargetIsVulnerable_AppliesNormalIncrement()
    {
        DamageTypeConfig targetDamageTypeConfig = ScriptableObject.CreateInstance<DamageTypeConfig>();
        _comboBarConfig.OverrideDamageTypeConfig(targetDamageTypeConfig);
        
        int increment = _comboBarConfig.CalculateComboIncrement(_comboBarConfig.DamageTypeConfig);

        Assert.AreEqual(_normalIncrement, increment);
    }
}