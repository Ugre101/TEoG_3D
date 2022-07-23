using UnityEngine;

namespace AvatarStuff
{
    public class AvatarCuller : MonoBehaviour
    {
        [SerializeField] AvatarChanger avatarChanger;

        [SerializeField, Range(float.Epsilon, 0.5f),]
        float threesHold = 0.5f;

        CharacterAvatar avatar;
        Camera cam;
        bool culled;

        bool hasAvatar;
        float lastTick;

        void Start()
        {
            avatarChanger.NewAvatar += Add;
            cam = Camera.main;
        }

        void Update()
        {
            if (!hasAvatar) return;
            if (Time.time < lastTick + 1f)
                return;
            lastTick = Time.time;
            bool visible = false;
            if (culled)
            {
                float ratio = ratio_of_screen();
                if (ratio > threesHold)
                    foreach (SkinnedMeshRenderer skinnedMeshRenderer in avatar.AllShapes)
                        skinnedMeshRenderer.enabled = true;
                culled = false;
            }
            else
            {
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in avatar.AllShapes)
                    if (skinnedMeshRenderer.isVisible)
                        visible = true;
                if (visible) return;
                float ratio = ratio_of_screen();
                if (ratio < threesHold)
                {
                    foreach (SkinnedMeshRenderer skinnedMeshRenderer in avatar.AllShapes)
                        skinnedMeshRenderer.enabled = false;
                    culled = true;
                }
            }
        }

        void OnDestroy() => avatarChanger.NewAvatar -= Add;

        void Add(CharacterAvatar obj)
        {
            hasAvatar = true;
            avatar = obj;
        }

        float ratio_of_screen()
        {
            Vector3 min = new(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new(float.MinValue, float.MinValue, float.MinValue);
            foreach (var child in avatar.AllShapes)
            {
                Vector3 center = child.bounds.center;
                float radius = child.bounds.extents.magnitude;
                Vector3[] sixPoints = new Vector3[6];
                sixPoints[0] = cam.WorldToScreenPoint(new Vector3(center.x - radius, center.y, center.z));
                sixPoints[1] = cam.WorldToScreenPoint(new Vector3(center.x + radius, center.y, center.z));
                sixPoints[2] = cam.WorldToScreenPoint(new Vector3(center.x, center.y - radius, center.z));
                sixPoints[3] = cam.WorldToScreenPoint(new Vector3(center.x, center.y + radius, center.z));
                sixPoints[4] = cam.WorldToScreenPoint(new Vector3(center.x, center.y, center.z - radius));
                sixPoints[5] = cam.WorldToScreenPoint(new Vector3(center.x, center.y, center.z + radius));
                foreach (Vector3 v in sixPoints)
                {
                    if (v.x < min.x) min.x = v.x;
                    if (v.y < min.y) min.y = v.y;
                    if (v.x > max.x) max.x = v.x;
                    if (v.y > max.y) max.y = v.y;
                }
            }

            float ratioWidth = (max.x - min.x) / cam.pixelWidth;
            float ratioHeight = (max.y - min.y) / cam.pixelHeight;
            float ratio = ratioWidth > ratioHeight ? ratioWidth : ratioHeight;
            ratio *= QualitySettings.GetQualityLevel() + 1f;
            if (ratio > 1.0f) ratio = 1.0f;

            return ratio;
        }
    }
}