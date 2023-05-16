using UnityEngine;

namespace AvatarStuff
{
    public sealed class AvatarCuller : MonoBehaviour
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
                float cullerRatio = ratio_of_screen();
                if (cullerRatio > threesHold) 
                    SetAllRenders(true);
                culled = false;
                return;
            }

            foreach (var skinnedMeshRenderer in avatar.AllShapes)
                if (skinnedMeshRenderer.isVisible)
                    visible = true;
            if (visible) return;
            
            float ratio = ratio_of_screen();
            if (threesHold < ratio) return;
            SetAllRenders(false);
            culled = true;
        }

        void SetAllRenders(bool setTo)
        {
            foreach (var skinnedMeshRenderer in avatar.AllShapes)
                skinnedMeshRenderer.enabled = setTo;
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
                var bounds = child.bounds;
                var center = bounds.center;
                var radius = bounds.extents.magnitude;
                var sixPoints = new Vector3[6];
                var cX = center.x;
                var cY = center.y;
                var cZ = center.z;
                sixPoints[0] = cam.WorldToScreenPoint(new Vector3(cX - radius, cY, cZ));
                sixPoints[1] = cam.WorldToScreenPoint(new Vector3(cX + radius, cY, cZ));
                sixPoints[2] = cam.WorldToScreenPoint(new Vector3(cX, cY - radius, cZ));
                sixPoints[3] = cam.WorldToScreenPoint(new Vector3(cX, cY + radius, cZ));
                sixPoints[4] = cam.WorldToScreenPoint(new Vector3(cX, cY, cZ - radius));
                sixPoints[5] = cam.WorldToScreenPoint(new Vector3(cX, cY, cZ + radius));
                foreach (var v in sixPoints)
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