using Static;
using UnityEngine;

namespace Options
{
    public class VoreContentHider : MonoBehaviour
    {
        void OnEnable()
        {
            if(!OptionalContent.Vore.Enabled)
                gameObject.SetActive(false);
        }
    }
}
