﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour
    {
        [Header("Information")] [Tooltip("The name that will be displayed in the UI for this weapon")]
        public string WeaponName;

        [Tooltip("The image that will be displayed in the UI for this weapon")]
        public Sprite WeaponIcon;

        [Tooltip("Default data for the crosshair")]
        public ShotTypeConfig CrosshairDataDefault;

        [Tooltip("Default damage type for the weapon")]
        public DamageTypeConfig DamageTypeConfig;

        [Header("Internal References")]
        [Tooltip("The root object for the weapon, this is what will be deactivated when the weapon isn't active")]
        public GameObject WeaponRoot;

        [Tooltip("Tip of the weapon, where the projectiles are shot")]
        public Transform WeaponMuzzle;

        [Tooltip("The projectile prefab")] public ProjectileBase ProjectilePrefab;

        [Tooltip("Minimum duration between two shots")]
        public float DelayBetweenShots = 0.5f;

        [Tooltip("Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")]
        public float BulletSpreadAngle = 0f;

        [Tooltip("Amount of bullets per shot")]
        public int BulletsPerShot = 1;

        [Tooltip("Force that will push back the weapon after each shot")] [Range(0f, 2f)]
        public float RecoilForce = 1;

        [Tooltip("Ratio of the default FOV that this weapon applies while aiming")] [Range(0f, 1f)]
        public float AimZoomRatio = 1f;

        [Tooltip("How far the weapon can affect enemies")]
        [Range(0f, 10000f)]
        public float ShootRange = 1000f;

        [Tooltip("Translation to apply to weapon arm when aiming with this weapon")]
        public Vector3 AimOffset;

        [Header("Ammo Parameters")]
        [Tooltip("Should the player manually reload")]
        public bool AutomaticReload = true;
        [Tooltip("Has physical clip on the weapon and ammo shells are ejected when firing")]
        public bool HasPhysicalBullets = false;
        [Tooltip("Number of bullets in a clip")]
        public int ClipSize = 30;
        [Tooltip("Bullet Shell Casing")]
        public GameObject ShellCasing;
        [Tooltip("Weapon Ejection Port for physical ammo")]
        public Transform EjectionPort;
        [Tooltip("Force applied on the shell")]
        [Range(0.0f, 5.0f)] public float ShellCasingEjectionForce = 2.0f;
        [Tooltip("Maximum number of shell that can be spawned before reuse")]
        [Range(1, 30)] public int ShellPoolSize = 1;
        [Tooltip("Amount of ammo reloaded per second")]
        public float AmmoReloadRate = 1f;

        [Tooltip("Delay after the last shot before starting to reload")]
        public float AmmoReloadDelay = 2f;

        [Tooltip("Maximum amount of ammo in the gun")]
        public int MaxAmmo = 8;

        [Tooltip("Maximum amount of ammo in the gun")]
        public bool HasInfiniteAmmo = false;

        [Header("Charging parameters (charging weapons only)")]
        [Tooltip("Trigger a shot when maximum charge is reached")]
        public bool AutomaticReleaseOnCharged;

        [Tooltip("Duration to reach maximum charge")]
        public float MaxChargeDuration = 2f;

        [Tooltip("Initial ammo used when starting to charge")]
        public float AmmoUsedOnStartCharge = 1f;

        [Tooltip("Additional ammo used when charge reaches its maximum")]
        public float AmmoUsageRateWhileCharging = 1f;

        [Header("Audio & Visual")] 
        [Tooltip("Optional weapon animator for OnShoot animations")]
        public Animator WeaponAnimator;

        [Tooltip("Prefab of the muzzle flash")]
        public GameObject MuzzleFlashPrefab;

        [Tooltip("Unparent the muzzle flash instance on spawn")]
        public bool UnparentMuzzleFlash;

        [Tooltip("sound played when shooting")]
        public AudioClip ShootSfx;

        [Tooltip("Sound played when changing to this weapon")]
        public AudioClip ChangeWeaponSfx;

        [Tooltip("Continuous Shooting Sound")] public bool UseContinuousShootSound = false;
        public AudioClip ContinuousShootStartSfx;
        public AudioClip ContinuousShootLoopSfx;
        public AudioClip ContinuousShootEndSfx;
        AudioSource m_ContinuousShootAudioSource = null;
        bool m_WantsToShoot = false;

        public UnityAction OnShoot;
        public event Action OnShootProcessed;

        int m_CarriedPhysicalBullets;

        float m_currentAmmo = 0;
        float CurrentAmmo { 
            get 
            {
                return m_currentAmmo;
            } 
            set 
            {
                if (HasInfiniteAmmo)
                {
                    m_currentAmmo = MaxAmmo;
                }
                else
                {
                    m_currentAmmo = value;
                }
            } 
        }
        float m_LastTimeShot = Mathf.NegativeInfinity;
        public float LastChargeTriggerTimestamp { get; private set; }
        Vector3 m_LastMuzzlePosition;

        public GameObject Owner { get; set; }
        public GameObject SourcePrefab { get; set; }
        public bool IsCharging { get; private set; }
        public float CurrentAmmoRatio { get; private set; }
        public bool IsWeaponActive { get; private set; }
        public bool IsCooling { get; private set; }
        public float CurrentCharge { get; private set; }
        public Vector3 MuzzleWorldVelocity { get; private set; }

        public float GetAmmoNeededToShoot() => (CrosshairDataDefault.IsChargeWeapon ? 1f : Mathf.Max(1f, AmmoUsedOnStartCharge)) / (MaxAmmo * BulletsPerShot);

        public int GetCarriedPhysicalBullets() => m_CarriedPhysicalBullets;
        public int GetCurrentAmmo() => Mathf.FloorToInt(CurrentAmmo);

        AudioSource m_ShootAudioSource;

        public bool IsReloading { get; private set; }

        const string k_AnimAttackParameter = "Attack";

        private Queue<Rigidbody> m_PhysicalAmmoPool;

        void Awake()
        {
            CurrentAmmo = MaxAmmo;
            m_CarriedPhysicalBullets = HasPhysicalBullets ? ClipSize : 0;
            m_LastMuzzlePosition = WeaponMuzzle.position;

            m_ShootAudioSource = GetComponent<AudioSource>();
            DebugUtility.HandleErrorIfNullGetComponent<AudioSource, WeaponController>(m_ShootAudioSource, this, gameObject);

            if (UseContinuousShootSound)
            {
                m_ContinuousShootAudioSource = gameObject.AddComponent<AudioSource>();
                m_ContinuousShootAudioSource.playOnAwake = false;
                m_ContinuousShootAudioSource.clip = ContinuousShootLoopSfx;
                m_ContinuousShootAudioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.WeaponShoot);
                m_ContinuousShootAudioSource.loop = true;
            }

            if (HasPhysicalBullets)
            {
                m_PhysicalAmmoPool = new Queue<Rigidbody>(ShellPoolSize);

                for (int i = 0; i < ShellPoolSize; i++)
                {
                    GameObject shell = Instantiate(ShellCasing, transform);
                    shell.SetActive(false);
                    m_PhysicalAmmoPool.Enqueue(shell.GetComponent<Rigidbody>());
                }
            }
        }

        public void AddCarriablePhysicalBullets(int count) => m_CarriedPhysicalBullets = Mathf.Max(m_CarriedPhysicalBullets + count, MaxAmmo);

        void ShootShell()
        {
            Rigidbody nextShell = m_PhysicalAmmoPool.Dequeue();

            nextShell.transform.position = EjectionPort.transform.position;
            nextShell.transform.rotation = EjectionPort.transform.rotation;
            nextShell.gameObject.SetActive(true);
            nextShell.transform.SetParent(null);
            nextShell.collisionDetectionMode = CollisionDetectionMode.Continuous;
            nextShell.AddForce(nextShell.transform.up * ShellCasingEjectionForce, ForceMode.Impulse);

            m_PhysicalAmmoPool.Enqueue(nextShell);
        }

        void PlaySFX(AudioClip sfx) => AudioUtility.CreateSFX(sfx, transform.position, AudioUtility.AudioGroups.WeaponShoot, 0.0f);


        void Reload()
        {
            //we don't reload on infinite ammo
            if (HasInfiniteAmmo)
            {
                return;
            }
            if (m_CarriedPhysicalBullets > 0)
            {
                CurrentAmmo = Mathf.Min(m_CarriedPhysicalBullets, ClipSize);
            }

            IsReloading = false;
        }

        public void StartReloadAnimation()
        {
            //we don't reload on infinite ammo
            if (HasInfiniteAmmo)
            {
                return;
            }
            if (CurrentAmmo < m_CarriedPhysicalBullets)
            {
                GetComponent<Animator>().SetTrigger("Reload");
                IsReloading = true;
            }
        }

        void Update()
        {
            UpdateAmmo();
            UpdateCharge();
            UpdateContinuousShootSound();

            if (Time.deltaTime > 0)
            {
                MuzzleWorldVelocity = (WeaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
                m_LastMuzzlePosition = WeaponMuzzle.position;
            }
        }

        void UpdateAmmo()
        {
            if (AutomaticReload && m_LastTimeShot + AmmoReloadDelay < Time.time && CurrentAmmo < MaxAmmo && !IsCharging)
            {
                // reloads weapon over time
                CurrentAmmo += AmmoReloadRate * Time.deltaTime;

                // limits ammo to max value
                CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, MaxAmmo);

                IsCooling = true;
            }
            else
            {
                IsCooling = false;
            }

            if (MaxAmmo == Mathf.Infinity)
            {
                CurrentAmmoRatio = 1f;
            }
            else
            {
                CurrentAmmoRatio = CurrentAmmo / MaxAmmo;
            }
        }

        void UpdateCharge()
        {
            if (IsCharging)
            {
                if (CurrentCharge < 1f)
                {
                    float chargeLeft = 1f - CurrentCharge;

                    // Calculate how much charge ratio to add this frame
                    float chargeAdded = 0f;
                    if (MaxChargeDuration <= 0f)
                    {
                        chargeAdded = chargeLeft;
                    }
                    else
                    {
                        chargeAdded = (1f / MaxChargeDuration) * Time.deltaTime;
                    }

                    chargeAdded = Mathf.Clamp(chargeAdded, 0f, chargeLeft);

                    // See if we can actually add this charge
                    float ammoThisChargeWouldRequire = chargeAdded * AmmoUsageRateWhileCharging;
                    if (ammoThisChargeWouldRequire <= CurrentAmmo)
                    {
                        // Use ammo based on charge added
                        UseAmmo(ammoThisChargeWouldRequire);

                        // set current charge ratio
                        CurrentCharge = Mathf.Clamp01(CurrentCharge + chargeAdded);
                    }
                }
            }
        }

        void UpdateContinuousShootSound()
        {
            if (UseContinuousShootSound)
            {
                if (m_WantsToShoot && CurrentAmmo >= 1f)
                {
                    if (!m_ContinuousShootAudioSource.isPlaying)
                    {
                        m_ShootAudioSource.PlayOneShot(ShootSfx);
                        m_ShootAudioSource.PlayOneShot(ContinuousShootStartSfx);
                        m_ContinuousShootAudioSource.Play();
                    }
                }
                else if (m_ContinuousShootAudioSource.isPlaying)
                {
                    m_ShootAudioSource.PlayOneShot(ContinuousShootEndSfx);
                    m_ContinuousShootAudioSource.Stop();
                }
            }
        }

        public void ShowWeapon(bool show)
        {
            WeaponRoot.SetActive(show);

            if (show && ChangeWeaponSfx)
            {
                m_ShootAudioSource.PlayOneShot(ChangeWeaponSfx);
            }

            IsWeaponActive = show;
        }

        public void UseAmmo(float amount)
        {
            CurrentAmmo = Mathf.Clamp(CurrentAmmo - amount, 0f, MaxAmmo);
            m_CarriedPhysicalBullets -= Mathf.RoundToInt(amount);
            m_CarriedPhysicalBullets = Mathf.Clamp(m_CarriedPhysicalBullets, 0, MaxAmmo);
            m_LastTimeShot = Time.time;
        }

        public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            m_WantsToShoot = inputDown || inputHeld;
            switch (CrosshairDataDefault.ShootType)
            {
                case ShotTypeConfig.WeaponShootType.Manual:
                    if (inputDown)
                    {
                        return TryShoot();
                    }

                    return false;

                case ShotTypeConfig.WeaponShootType.Automatic:
                    if (inputHeld)
                    {
                        return TryShoot();
                    }

                    return false;

                case ShotTypeConfig.WeaponShootType.Charge:
                    if (inputHeld)
                    {
                        TryBeginCharge();
                    }

                    // Check if we released charge or if the weapon shoot automatically when it's fully charged
                    if (inputUp || (AutomaticReleaseOnCharged && CurrentCharge >= 1f))
                    {
                        return TryReleaseCharge();
                    }

                    return false;

                default:
                    return false;
            }
        }

        bool TryShoot()
        {
            //we don't take ammo into account if we have infinite ammo
            if ( CurrentAmmo >= 1f && m_LastTimeShot + DelayBetweenShots < Time.time)
            {
                HandleShoot();

                return true;
            }

            return false;
        }

        bool TryBeginCharge()
        {
            if (!IsCharging && CurrentAmmo >= AmmoUsedOnStartCharge && Mathf.FloorToInt((CurrentAmmo - AmmoUsedOnStartCharge) * BulletsPerShot) > 0 && m_LastTimeShot + DelayBetweenShots < Time.time)
            {
                UseAmmo(AmmoUsedOnStartCharge);

                LastChargeTriggerTimestamp = Time.time;
                IsCharging = true;

                return true;
            }

            return false;
        }

        bool TryReleaseCharge()
        {
            if (IsCharging)
            {
                HandleShoot();

                CurrentCharge = 0f;
                IsCharging = false;

                return true;
            }

            return false;
        }

        void HandleShoot()
        {
            int bulletsPerShotFinal = CrosshairDataDefault.IsChargeWeapon ? Mathf.CeilToInt(CurrentCharge * BulletsPerShot) : BulletsPerShot;

            // spawn all bullets with random direction
            for (int i = 0; i < bulletsPerShotFinal; i++)
            {
                Vector3 shotDirection = GetShotDirectionWithinSpread(WeaponMuzzle);
                ProjectileBase newProjectile = Instantiate(ProjectilePrefab, WeaponMuzzle.position, Quaternion.LookRotation(shotDirection));
                newProjectile.Shoot(this);
            }

            // muzzle flash
            if (MuzzleFlashPrefab != null)
            {
                GameObject muzzleFlashInstance = Instantiate(MuzzleFlashPrefab, WeaponMuzzle.position, WeaponMuzzle.rotation, WeaponMuzzle.transform);
                // Unparent the muzzleFlashInstance
                if (UnparentMuzzleFlash)
                {
                    muzzleFlashInstance.transform.SetParent(null);
                }

                Destroy(muzzleFlashInstance, 2f);
            }

            if (HasPhysicalBullets)
            {
                ShootShell();
                m_CarriedPhysicalBullets--;
            }

            m_LastTimeShot = Time.time;

            // play shoot SFX
            if (ShootSfx && !UseContinuousShootSound)
            {
                m_ShootAudioSource.PlayOneShot(ShootSfx);
            }

            // Trigger attack animation if there is any
            if (WeaponAnimator)
            {
                WeaponAnimator.SetTrigger(k_AnimAttackParameter);
            }

            OnShoot?.Invoke();
            OnShootProcessed?.Invoke();
        }

        public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
        {
            float spreadAngleRatio = BulletSpreadAngle / 180f;
            Vector3 forwardDirection = shootTransform.forward;
            Vector3 spreadWorldDirection = Vector3.Slerp(forwardDirection, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);

            return spreadWorldDirection;
        }
    }
}