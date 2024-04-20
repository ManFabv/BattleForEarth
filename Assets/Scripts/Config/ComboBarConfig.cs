using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Combo Bar Config", menuName = "Combo/Combo Config")]

public class ComboBarConfig : ScriptableObject
{
    [field: SerializeField, Range(0, 100)]
    public int MaxComboValue { get; private set; } = 10;

    [field: SerializeField, Range(0, 100)]
    public int ValueToIncrementOnCombo { get; private set; } = 1;

    [field: SerializeField]
    public ShotTypeConfig ShotTypeConfig { get; private set; }

    [field: SerializeField]
    public DamageTypeConfig DamageTypeConfig { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }
    [Tooltip("Event to trigger when bar is full")]
    [SerializeField]
    private GameplayEventsConfig OnBarFull;

    public void OverrideDamageTypeConfig(DamageTypeConfig newDamageTypeConfig) => DamageTypeConfig = newDamageTypeConfig;

    public int CalculateComboIncrement(DamageTypeConfig vulnerability) => (vulnerability != null && vulnerability == DamageTypeConfig) ? ValueToIncrementOnCombo : 0;

    public bool IsBarFull(Slider comboBarSlider) => (comboBarSlider.value >= MaxComboValue);

    public void RaiseOnBarFull()
    {
        //if we filled the bar, we trigger an event
        if (OnBarFull != null)
        {
            OnBarFull.RaiseGameplayEvent(DamageTypeConfig);
        }
    }
}
