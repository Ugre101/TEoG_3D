using System.Linq;
using System.Text;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public sealed class MoreAfterBattleActorInfo : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI bodyInfo;
        [SerializeField] TextMeshProUGUI sexualOrgansInfo;
        BaseCharacter actor;

        void Start()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            if (actor == null)
                return;
            PrintBodyInfo();
            foreach (var body in actor.Body.BodyStats)
                body.Value.StatDirtyEvent += PrintBodyInfo;
            PrintOrganInfo();
            foreach (var org in actor.SexualOrgans.GetAllOrgans())
                org.StatDirtyEvent += PrintOrganInfo;
        }

        void OnDisable()
        {
            if (actor == null)
                return;
            foreach (var body in actor.Body.BodyStats)
                body.Value.StatDirtyEvent -= PrintBodyInfo;
            foreach (var org in actor.SexualOrgans.GetAllOrgans())
                org.StatDirtyEvent -= PrintOrganInfo;
        }

        public void Up() => ToggleAll(false);

        public void Down() => ToggleAll(true);

        void ToggleAll(bool down)
        {
            gameObject.SetActive(down);
        }

        public void Setup(BaseCharacter actor) => this.actor = actor;

        void PrintOrganInfo()
        {
            StringBuilder sb = new();
            foreach (var (key, value) in actor.SexualOrgans.Containers)
            {
                foreach (var org in value.BaseList)
                    sb.Append(org.OrganDesc());
                sb.AppendLine().AppendLine();
            }

            sexualOrgansInfo.text = sb.ToString();
        }


        void PrintBodyInfo() => bodyInfo.text =
            $"{actor.Body.Height.Value.ConvertCm()} tall and weighing {actor.Body.Weight.ConvertKg()}";
    }
}