using CustomClasses;
using Safe_To_Share.Scripts.CameraStuff;
using Safe_To_Share.Scripts.CustomClasses.UI;

namespace Safe_To_Share.Scripts.Movement.Settings.UI
{
    public class ToggleFirstPerson : SavedBoolToggle
    {
        protected override SavedBoolSetting SavedBool => FirstPersonCameraSettings.FirstPersonCameraEnabled;
    }
}