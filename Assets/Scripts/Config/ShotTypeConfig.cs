using UnityEngine;

[CreateAssetMenu(fileName = "New Shot Type", menuName = "Config/Shot Type")]
public class ShotTypeConfig : ScriptableObject
{
    public enum WeaponShootType
    {
        Manual,
        Automatic,
        Charge,
    }

    [Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite CrosshairSprite;

    [Tooltip("The size of the crosshair image")]
    [Range(0, 100)]
    public int CrosshairSize;

    [Tooltip("The size of the crosshair image at sight")]
    [Range(0, 100)]
    public int CrosshairAtSigthSize;

    [Tooltip("The color of the crosshair image")]
    public Color CrosshairColor;

    [Tooltip("The type of weapon will affect how it shoots")]
    public WeaponShootType ShootType;

    [Tooltip("Velocity at which the crosshair lerp from normal to sight sprite size")]
    [Range(0, 100)]
    public float CrosshairUpdateshrpness;

    [Tooltip("Default damage that we can apply to any target")]
    [Range(0, 100)]
    public float DefaultDamage;

    [Tooltip("Damage that we can apply when the target is vulnerable to the same 'type' of config")]
    [Range(0, 100)]
    public float VulnerableDamage;

    public bool IsChargeWeapon => ShootType == WeaponShootType.Charge;

    public float CalculateDamageByVulnerabilityType(ShotTypeConfig targetObjectType) => this == targetObjectType ? VulnerableDamage : DefaultDamage;
}