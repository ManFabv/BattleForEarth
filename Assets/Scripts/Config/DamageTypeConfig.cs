using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Type", menuName = "Config/Damage Type")]
public class DamageTypeConfig :  ScriptableObject
{
    [Header("Damage")]
    [Tooltip("Default damage that we can apply to any target")]
    [SerializeField, Range(0.0f, 100.0f)]
    private float damage = 7;
    [Tooltip("Damage that we can apply when the target is vulnerable to the same 'type' of config")]
    [SerializeField, Range(0.0f, 100.0f)]
    private float comboDamage = 14;
    [Tooltip("Event to trigger when we have a combo")]
    [SerializeField]
    private GameplayEventsConfig OnComboAchieved;

    //if we hit a target that is vulnerable to this type of damage we increase the damage
    public float CalculateDamage(DamageTypeConfig targetVulnerability)
    {
        float calculatedDamage = damage;

        if (targetVulnerability == null)
        {
            return calculatedDamage;
        }

        if (targetVulnerability == this)
        {
            calculatedDamage = comboDamage;

            //if we get a combo, we trigger an event
            if(OnComboAchieved != null)
            {
                OnComboAchieved.RaiseGameplayEvent(targetVulnerability);
            }
        }

        return calculatedDamage;
    }
}