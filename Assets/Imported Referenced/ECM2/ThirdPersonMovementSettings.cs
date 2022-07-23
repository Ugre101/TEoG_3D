using UnityEngine;

namespace Movement.ECM2.Source
{
    public static class ThirdPersonMovementSettings
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