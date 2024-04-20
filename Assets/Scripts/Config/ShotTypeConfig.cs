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

    [field: SerializeField, Tooltip("The image that will be used for this weapon's crosshair")]
    public Sprite CrosshairSprite {  get; private set; }

    [Tooltip("The size of the crosshair image")]
    [field: SerializeField, Range(0, 100)]
    public int CrosshairSize { get; private set; }

    [Tooltip("The size of the crosshair image at sight")]
    [field: SerializeField, Range(0, 100)]
    public int CrosshairAtSigthSize { get; private set; }

    [field: SerializeField, Tooltip("The color of the crosshair image")]
    public Color CrosshairColor { get; private set; }

    [field: SerializeField, Tooltip("The type of weapon will affect how it shoots")]
    public WeaponShootType ShootType { get; private set; }

    [Tooltip("Velocity at which the crosshair lerp from normal to sight sprite size")]
    [field: SerializeField, Range(0, 100)]
    public float CrosshairUpdateshrpness { get; private set; }

    public bool IsChargeWeapon => ShootType == WeaponShootType.Charge;
}