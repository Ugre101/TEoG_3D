using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class StartSceneEditorTools : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start() => GameTester.GetFirstCall();
    }
}
