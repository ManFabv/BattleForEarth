using Cinemachine;
using KBCore.Refs;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler), typeof(AudioSource))]
    public class PlayerCharacterController : ValidatedMonoBehaviour
    {
        [Header("References")] [Tooltip("Reference to the main camera used for the player")]
        public Camera PlayerCamera;

        [Tooltip("Audio source for footsteps, jump, etc...")]
        public AudioSource AudioSource;

        public UnityAction<bool> OnStanceChanged;

        public bool IsDead { get; private set; }
        
        [HideInInspector, SerializeField, Self] Health m_Health;
        [HideInInspector, SerializeField, Self] CharacterController m_Controller;
        [HideInInspector, SerializeField, Self] PlayerWeaponsManager m_WeaponsManager;
        [HideInInspector, SerializeField, Parent] CinemachineDollyCart DollyCart;

        void Awake()
        {
            ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            if (actorsManager != null)
                actorsManager.SetPlayer(gameObject);
        }

        void Start()
        {
            m_Controller.enableOverlapRecovery = true;

            m_Health.OnDie += OnDie;
        }

        void OnDie()
        {
            IsDead = true;

            // Tell the weapons manager to switch to a non-existing weapon in order to lower the weapon
            m_WeaponsManager.SwitchToWeaponIndex(-1, true);

            EventManager.Broadcast(Events.PlayerDeathEvent);
        }

        private void LateUpdate()
        {
            // we smoothly look at the movement direction (the dolly cart object, which is the one is moving)
            PlayerCamera.transform.rotation = Quaternion.Slerp(PlayerCamera.transform.rotation, DollyCart.transform.rotation, 0.001f);
        }
    }
}