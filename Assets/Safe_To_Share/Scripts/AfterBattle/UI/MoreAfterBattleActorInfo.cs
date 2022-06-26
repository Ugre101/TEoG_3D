using System.Linq;
using System.Text;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.UI
{
    public class MoreAfterBattleActorInfo : MonoBehaviour
    {
        [SerializeField] Button downBtn, upBtn;
        [SerializeField] TextMeshProUGUI bodyInfo;
        [SerializeField] TextMeshProUGUI sexualOrgansInfo;
        BaseCharacter actor;

        void Start()
        {
            gameObject.SetActive(false);
            downBtn.onClick.AddListener(Down);
            downBtn.gameObject.SetActive(true);
            upBtn.onClick.AddListener(Up);
            upBtn.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            if (actor == null)
                return;
            PrintBodyInfo();
            foreach (var body in actor.Body.BodyStats)
                body.Value.StatDirtyEvent += PrintBodyInfo;
            PrintOrganInfo();
            foreach (var container in actor.SexualOrgans.Containers.Values)
            foreach (var org in container.List)
                org.StatDirtyEvent += PrintOrganInfo;
        }

        void OnDisable()
        {
            if (actor == null)
                return;
            foreach (var body in actor.Body.BodyStats)
                body.Value.StatDirtyEvent -= PrintBodyInfo;
            foreach (BaseOrgan org in actor.SexualOrgans.Containers.Values.SelectMany(container => container.List))
                org.StatDirtyEvent -= PrintOrganInfo;
        }

        void Up() => ToggleAll(false);

        void Down() => ToggleAll(true);

        void ToggleAll(bool down)
        {
            gameObject.SetActive(down);
            downBtn.gameObject.SetActive(!down);
            upBtn.gameObject.SetActive(down);
        }

        public void Setup(BaseCharacter actor) => this.actor = actor;

        void PrintOrganInfo()
        {
            StringBuilder sb = new();
            foreach ((SexualOrganType key, OrgansContainer value) in actor.SexualOrgans.Containers)
            {
                foreach (var org in value.List)
                    sb.Append(SexualOrgansExtensions.OrganDesc(key, org));
                sb.AppendLine();
                sb.AppendLine();
            }

            sexualOrgansInfo.text = sb.ToString();
            print(sb.ToString());
        }


        void PrintBodyInfo() => bodyInfo.text =
            $"{actor.Body.Height.Value.ConvertCm()} tall and weighing {actor.Body.Weight.ConvertKg()}";
    }
}