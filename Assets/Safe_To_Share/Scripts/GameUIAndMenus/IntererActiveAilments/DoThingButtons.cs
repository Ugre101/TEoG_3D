using Character;
using Safe_To_Share.Scripts.Character.Scat;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IntererActiveAilments
{
    public class DoThingButtons : MonoBehaviour
    {
        [SerializeField] NeedToShitButton shitButton;
        [SerializeField] NeedToPissButton pissButton;
        public void CheckPlayer(BaseCharacter character)
        {
            if (NeedToShit.Has(character))
            {
                shitButton.Show();
            }
        }
    }
}