using System.Collections;
using Character.StatsStuff.HealthStuff;
using TMPro;
using UnityEngine;

namespace Battle
{
    public class HealthChangePopupNumbers : MonoBehaviour
    {
        [SerializeField] Color lightDmg = Color.yellow, mediumDmg = new(255, 165, 0), highDmg = Color.red;

        [Range(0.5f, 2f), SerializeField,] float minFontSize = 0.5f;
        [Range(2f, 4f), SerializeField,] float maxFontSize = 2f;

        [SerializeField] float delay = 2f;
        [SerializeField] TextMeshProUGUI text;
        WaitForSeconds delayedSetActiveFalse;

        Health hp, wp;

        void OnDestroy() => UnSub();


        public void Setup(Health health, Health willPower)
        {
            delayedSetActiveFalse = new WaitForSeconds(delay);

            hp = health;
            hp.ValueDecrease += TakeHpDamage;
            hp.ValueIncrease += HealHp;

            wp = willPower;
            wp.ValueDecrease += TakeWillDamage;
            wp.ValueIncrease += HealWp;
        }

        void HealWp(int obj)
        {
            float changePercent = (float) obj / wp.Value;
            text.color = Color.green;
            text.fontSize = CalcFontSize(changePercent);
            BaseChange(obj, "wp");
        }

        void HealHp(int obj)
        {
            float changePercent = (float) obj / hp.Value;
            text.color = Color.blue;
            text.fontSize = CalcFontSize(changePercent);
            BaseChange(obj, "hp");
        }

        void TakeWillDamage(int obj)
        {
            float changePercent = (float) obj / wp.Value;
            text.color = DamageColor(changePercent);
            text.fontSize = CalcFontSize(changePercent);
            BaseChange(obj, "wp");
        }

        Color DamageColor(float changePercent) =>
            changePercent > 0.5f ? highDmg :
            changePercent > 0.3f ? mediumDmg : lightDmg;

        void TakeHpDamage(int obj)
        {
            float changePercent = (float) obj / hp.Value;
            text.color = DamageColor(changePercent);
            text.fontSize = CalcFontSize(changePercent);
            BaseChange(obj, "hp");
        }

        public void UnSub()
        {
            if (hp != null)
            {
                hp.ValueDecrease -= TakeHpDamage;
                hp.ValueIncrease -= HealHp;
            }

            if (wp != null)
            {
                wp.ValueDecrease -= TakeWillDamage;
                wp.ValueIncrease -= HealWp;
            }
        }

        float CalcFontSize(float changePercent) => minFontSize + (maxFontSize - minFontSize) * changePercent;

        void BaseChange(float change, string healthType)
        {
            text.text = $"{change} {healthType}";
            gameObject.SetActive(true);
            StartCoroutine(DelayedSetActiveFalse());
        }


        IEnumerator DelayedSetActiveFalse()
        {
            yield return delayedSetActiveFalse;
            gameObject.SetActive(false);
        }
    }
}