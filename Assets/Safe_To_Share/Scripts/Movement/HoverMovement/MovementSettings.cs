using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public static class MovementSettings
    {
        const string StrafeSave = "ThirdPersonMovementStrafe";
        static bool? strafe;

        public static bool Strafe
        {
            get
            {
                strafe = PlayerPrefs.GetInt(StrafeSave, 0) == 1;
                return strafe.Value;
            }
            set
            {
                PlayerPrefs.SetInt(StrafeSave, value ? 1 : 0);
                strafe = value;
            }
        }
    }
}