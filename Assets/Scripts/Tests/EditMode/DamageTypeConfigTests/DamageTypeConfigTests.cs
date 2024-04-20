using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class DamageTypeConfigTests
{
    //this are the default values for the ScriptableObjects
    //Because we are creating an instance, we don't modify these
    //values and we use the defaults
    private const float _normalDamage = 7;
    private const float _comboDamage = 14;

    private DamageTypeConfig _damageTypeConfig;

    [SetUp]
    public void SetUp()
    {
        _damageTypeConfig = ScriptableObject.CreateInstance<DamageTypeConfig>();
    }

    [Test]
    public void When_ConfigIsCreated_IsNotNull()
    {
        Assert.NotNull(_damageTypeConfig);
    }

    [Test]
    public void CalculateDamage_WhenTargetIsNotVulnerable_AppliesNormalDamage()
    {
        DamageTypeConfig targetDamageTypeConfig = ScriptableObject.CreateInstance<DamageTypeConfig>();

        float damage = _damageTypeConfig.CalculateDamage(targetDamageTypeConfig);

        Assert.AreEqual(_normalDamage, damage);
    }

    [Test]
    public void CalculateDamage_WhenTargetIsVulnerable_AppliesMoreDamage()
    {
        float damage = _damageTypeConfig.CalculateDamage(_damageTypeConfig);

        Assert.AreEqual(_comboDamage, damage);
    }

    [Test]
    public void CalculateDamage_WhenTargetVulnerabilityIsNull_AppliesNormalDamage()
    {
        float damage = _damageTypeConfig.CalculateDamage(null);

        Assert.AreEqual(_normalDamage, damage);
    }
}