using System;
using System.Text;
using Character.LevelStuff;
using Character.StatsStuff;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUIAndMenus.Menus.Level
{
    public abstract class BaseHoverInfo : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected TextMeshProUGUI cost;
        [SerializeField] protected TextMeshProUGUI desc;
        [SerializeField] protected TextMeshProUGUI needPerk;
        [SerializeField] protected TextMeshProUGUI needCharStat;
        [SerializeField] protected TextMeshProUGUI exclusivePerk;
        [SerializeField] protected Vector2 offset;
        [SerializeField] RectTransform rectTransform;
        [SerializeField, Range(0.5f, 1f),] float flipOffsetThreshold = 0.75f;

        protected virtual void Start() => gameObject.SetActive(false);

        protected void StopShow() => gameObject.SetActive(false);

        protected void ShowPerkInfo(Vector3 pos, BasicPerk perk)
        {
            if (perk == null)
            {
                StopShow();
                return;
            }
            SetPos(pos);
            title.text = perk.Title;
            cost.text = $"Cost {{{perk.Cost}}}";
            desc.text = perk.Desc;
            PrintAltReqStats(perk);
            PrintAltReqPerks(perk);
            PrintAltExclusivePerks(perk);
            gameObject.SetActive(true);
        }

        void PrintAltExclusivePerks(BasicPerk perk)
        {
            if (perk.ExclusiveWithPerkGuids.Count <= 0)
            {
                exclusivePerk.text = string.Empty;
                return;
            }

            exclusivePerk.text = "Exclusive perks";
            foreach (string exclusiveWithPerkGuid in perk.ExclusiveWithPerkGuids)
                Addressables.LoadAssetAsync<BasicPerk>(exclusiveWithPerkGuid).Completed += AddExclusive;
        }

        void PrintAltReqPerks(BasicPerk perk)
        {
            if (perk.NeedPerkGuids.Count <= 0)
            {
                needPerk.text = string.Empty;
                return;
            }

            needPerk.text = "Needed perks";
            foreach (string perkNeedPerkGuid in perk.NeedPerkGuids)
                Addressables.LoadAssetAsync<BasicPerk>(perkNeedPerkGuid).Completed += AddNeed;
        }

        void PrintAltReqStats(BasicPerk perk)
        {
            if (perk.RequireCharStats.Count <= 0)
            {
                needCharStat.text = string.Empty;
                return;
            }

            StringBuilder sb = new("Need stat");
            foreach (RequireCharStat perkRequireCharStat in perk.RequireCharStats)
                sb.Append($" {perkRequireCharStat.StatType} {perkRequireCharStat.Amount}");
            needCharStat.text = sb.ToString();
        }

        protected void SetPos(Vector2 pos)
        {
            // Vector2 pointerPos = pos;
            // pointerPos.x += rectTransform.rect.width / 2;
            // transform.position = pointerPos + offset;
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, pos, null, out Vector3 point))
                return;
            float xPercentOfScreen = point.x / Screen.width;
            Rect rect = rectTransform.rect;
            if (xPercentOfScreen < flipOffsetThreshold)
                point.x += offset.x * (Screen.width / rect.width);
            else
                point.x -= offset.x * (Screen.width / rect.width);
            point.y += offset.y * (Screen.height / rect.height);
            transform.position = point;
        }

        void AddExclusive(AsyncOperationHandle<BasicPerk> obj) =>
            exclusivePerk.text += $"{Environment.NewLine}{obj.Result.Title}";

        void AddNeed(AsyncOperationHandle<BasicPerk> obj) =>
            needPerk.text += $"{Environment.NewLine}{obj.Result.Title}";
    }
}