using KBCore.Refs;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class PlayerInputHandler : ValidatedMonoBehaviour
    {
        [Header("Control")]
        [Tooltip("Horizontal sensitivity multiplier for moving the camera around")]
        public float HorizontalLookSensitivity = 12f;

        [Tooltip("Vertical sensitivity multiplier for moving the camera around")]
        public float VerticalLookSensitivity = 10f;        

        [Tooltip("Horizontal movement speed for the player")]
        public float HorizontalMovementSpeed = 6f;

        [Tooltip("Vertical movement speed for the player")]
        public float VerticalMovementSpeed = 5f;

        [Tooltip("Additional sensitivity multiplier for WebGL")]
        public float WebglLookSensitivityMultiplier = 0.25f;

        [Tooltip("Limit to consider an input when using a trigger on a controller")]
        public float TriggerAxisThreshold = 0.4f;

        [Tooltip("Used to flip the vertical input axis")]
        public bool InvertYAxis = false;

        [Tooltip("Used to flip the horizontal input axis")]
        public bool InvertXAxis = false;

        GameFlowManager m_GameFlowManager;
        [HideInInspector, SerializeField, Self] PlayerCharacterController m_PlayerCharacterController;
        bool[] m_FireInputWasHeld = new bool[Mathf.Min(GameConstants.k_FireButtonName.Length, GameConstants.k_FireGamepadButtonName.Length)];

        private float ValueHorizontalByInvert => InvertXAxis ? -1 : 1;
        private float ValueVerticalByInvert => InvertYAxis ? 1 : -1;

        private bool IsNotValidInputIndex(int index) => !GameConstants.k_FireButtonName.IsIndexValid(index) || !GameConstants.k_FireGamepadButtonName.IsIndexValid(index);

        private bool IsNotValidPowerupInputIndex(int index) => !GameConstants.k_PowerupButtonName.IsIndexValid(index) || !GameConstants.k_PowerupGamepadButtonName.IsIndexValid(index);

        void Start()
        {
            m_GameFlowManager = FindObjectOfType<GameFlowManager>();
            DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, PlayerInputHandler>(m_GameFlowManager, this);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void LateUpdate()
        {
            for(int i=0; i<m_FireInputWasHeld.Length; i++)
            {
                m_FireInputWasHeld[i] = GetFireInputHeld(i);
            }
        }

        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked && !m_GameFlowManager.GameIsEnding;
        }

        public Vector3 GetMoveInputBySpeed()
        {
            if (CanProcessInput())
            {
                Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));

                // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
                move = Vector3.ClampMagnitude(move, 1);
                move.x *= HorizontalMovementSpeed; 
                move.z *= VerticalMovementSpeed;

                return move;
            }

            return Vector3.zero;
        }

        public float GetLookInputsHorizontalBySensitivity()
        {
            return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameHorizontal,
                GameConstants.k_AxisNameJoystickLookHorizontal) * HorizontalLookSensitivity * ValueHorizontalByInvert;
        }

        public float GetLookInputsVerticalBySensitivity()
        {
            return GetMouseOrStickLookAxis(GameConstants.k_MouseAxisNameVertical,
                GameConstants.k_AxisNameJoystickLookVertical) * VerticalLookSensitivity * ValueVerticalByInvert;
        }

        public bool GetFireInputDown(int index)
        {
            if (IsNotValidInputIndex(index))
            {
                return false;
            }
            return GetFireInputHeld(index) && !m_FireInputWasHeld[index];
        }

        public bool GetFireInputReleased(int index)
        {
            if (IsNotValidInputIndex(index))
            {
                return false;
            }
            return !GetFireInputHeld(index) && m_FireInputWasHeld[index];
        }

        public bool GetFireInputHeld(int index)
        {
            if (IsNotValidInputIndex(index))
            {
                return false;
            }
            if (CanProcessInput())
            {
                bool isGamepad = Input.GetAxis(GameConstants.k_FireGamepadButtonName[index]) != 0f;
                if (isGamepad)
                {
                    return Input.GetAxis(GameConstants.k_FireGamepadButtonName[index]) >= TriggerAxisThreshold;
                }
                else
                {
                    return Input.GetButton(GameConstants.k_FireButtonName[index]);
                }
            }

            return false;
        }

        public bool GetSprintInputHeld()
        {
            if (CanProcessInput())
            {
                return Input.GetButton(GameConstants.k_ButtonNameSprint);
            }

            return false;
        }

        public bool GetReloadButtonDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(GameConstants.k_ButtonReload);
            }

            return false;
        }

        public int GetSwitchWeaponInput()
        {
            if (CanProcessInput())
            {
                bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadSwitchWeapon) != 0f;
                string axisName = isGamepad
                    ? GameConstants.k_ButtonNameGamepadSwitchWeapon
                    : GameConstants.k_ButtonNameSwitchWeapon;

                if (Input.GetAxis(axisName) > 0f)
                    return -1;
                else if (Input.GetAxis(axisName) < 0f)
                    return 1;
                else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) > 0f)
                    return -1;
                else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) < 0f)
                    return 1;
            }

            return 0;
        }

        public bool IsFiringWeaponAtIndex(int index)
        {
            if(IsNotValidInputIndex(index))
            {
                return false;
            }

            return Input.GetButtonDown(GameConstants.k_FireButtonName[index]) || Input.GetButtonDown(GameConstants.k_FireGamepadButtonName[index]);
        }

        public int GetSelectWeaponInput()
        {
            if (CanProcessInput())
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    return 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    return 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    return 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    return 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    return 5;
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                    return 6;
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                    return 7;
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                    return 8;
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                    return 9;
                else
                    return 0;
            }

            return 0;
        }

        float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
        {
            if (CanProcessInput())
            {
                // Check if this look input is coming from the mouse
                bool isGamepad = Input.GetAxis(stickInputName) != 0f;
                float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

                // handle inverting vertical input
                if (InvertYAxis)
                    i *= -1f;

                // apply sensitivity multiplier
                i *= VerticalLookSensitivity;

                if (isGamepad)
                {
                    // since mouse input is already deltaTime-dependent, only scale input with frame time if it's coming from sticks
                    i *= Time.deltaTime;
                }
                else
                {
                    // reduce mouse input amount to be equivalent to stick movement
                    i *= 0.01f;
#if UNITY_WEBGL
                    // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                    i *= WebglLookSensitivityMultiplier;
#endif
                }

                return i;
            }

            return 0f;
        }

        public bool IsActivatingPowerupAtIndex(int index)
        {
            if (IsNotValidPowerupInputIndex(index))
            {
                return false;
            }

            return Input.GetButtonDown(GameConstants.k_PowerupButtonName[index]) || Input.GetButtonDown(GameConstants.k_PowerupGamepadButtonName[index]);
        }
    }
}