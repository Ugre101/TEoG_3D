using System.IO;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map {
    public sealed class ScreenShootMiniMapBackGround : MonoBehaviour {
        [ContextMenu("Take Picture")]
        void TakePicture() {
            if (!TryGetComponent(out Camera cam))
                return;
            // cam.targetTexture = RenderTexture.active;
            var rt = cam.targetTexture;
            RenderTexture.active = rt;

            cam.Render();

            Texture2D texture2D = new(rt.width, rt.height, TextureFormat.ARGB32, false);
            Rect rect = new(0, 0, rt.width, rt.height);
            texture2D.ReadPixels(rect, 0, 0);

            var bytes = texture2D.EncodeToPNG();
            DestroyImmediate(texture2D);
            var cleanLocation = UgreTools.StringFormatting.CleanFilePath(SceneManager.GetActiveScene().name);
            File.WriteAllBytes(Path.Combine(Application.dataPath, $"MiniMap{cleanLocation}.png"), bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    }
}