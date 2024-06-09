namespace Unity.FPS.Game
{
    //TODO: for the future we can use addressables or bundles for these values
    public class GameConstants
    {
        // all the constant string used across the game
        public const string k_AxisNameVertical = "Vertical";
        public const string k_AxisNameHorizontal = "Horizontal";
        public const string k_MouseAxisNameVertical = "Mouse Y";
        public const string k_MouseAxisNameHorizontal = "Mouse X";
        public const string k_AxisNameJoystickLookVertical = "Look Y";
        public const string k_AxisNameJoystickLookHorizontal = "Look X";
        
        public const string k_ButtonNameSprint = "Sprint";
        public const string k_ButtonNameJump = "Jump";
        public const string k_ButtonNameCrouch = "Crouch";

        public const string k_ButtonNameSwitchWeapon = "Mouse ScrollWheel";
        public const string k_ButtonNameGamepadSwitchWeapon = "Gamepad Switch";
        public const string k_ButtonNameNextWeapon = "NextWeapon";
        public const string k_ButtonNamePauseMenu = "Pause Menu";
        public const string k_ButtonNameSubmit = "Submit";
        public const string k_ButtonNameCancel = "Cancel";
        public const string k_ButtonReload = "Reload";

        public const int MAX_AMMOUNT_OF_WEAPONS_ALLOWED = 9;

        //TODO: this can be improved using scriptable objects
        // Fire buttons
        public static readonly string[] k_FireButtonName =
        {
           "Fire",
           "Fire Second",
        };
        //TODO: Fire gamepad buttons
        public static readonly string[] k_FireGamepadButtonName =
        {
           "Gamepad Fire",
           "Gamepad Fire Second",
        };
        //TODO: Power up buttons
        public static readonly string[] k_PowerupButtonName =
        {
           "Activate First Power Up",
           "Activate Second Power Up",
        };
        //TODO: Power up gamepad buttons
        public static readonly string[] k_PowerupGamepadButtonName =
        {
           "Gamepad Activate First Power Up",
           "Gamepad Activate Second Power Up",
        };
    }
}