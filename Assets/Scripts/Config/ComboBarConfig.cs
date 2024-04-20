using UnityEngine;

[CreateAssetMenu(fileName = "New Combo Bar Config", menuName = "Combo/Combo Config")]

public class ComboBarConfig : ScriptableObject
{
    [field: SerializeField, Range(0, 100)]
    public int MaxComboValue { get; private set; }

    [field: SerializeField]
    public ShotTypeConfig ShotTypeConfig { get; private set; }
}
