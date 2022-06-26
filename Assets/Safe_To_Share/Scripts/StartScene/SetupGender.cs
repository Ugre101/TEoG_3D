using System;
using System.Collections;
using Character;
using Character.EssenceStuff;
using Character.GenderStuff;
using Character.PlayerStuff;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.StartScene
{
    [Serializable]
    public class SetupGender
    {
        [SerializeField] TMP_Dropdown genderDropdown;
        [SerializeField] TextMeshProUGUI genderInfo;
        [SerializeField] AssetReference doll;
        [SerializeField] AssetReference male;
        [SerializeField] AssetReference female;
        [SerializeField] AssetReference cuntBoy;
        [SerializeField] AssetReference dickgirl;
        [SerializeField] AssetReference futa;
        [SerializeField] AssetReference maleFuta;
        Gender startGender = Gender.Doll;
        StartGenderPerk loaded;
        AsyncOperationHandle<StartGenderPerk> loadingOp;

        public void SetupGenderDropDown()
        {
            genderDropdown.SetupTmpDropDown(startGender,ChangeStartGender);    
            ChangeStartGender(UgreTools.IndexOfEnum(startGender));
        }
       
        public IEnumerator SetStartGender(Player tempPlayer)
        {
            if (!loadingOp.IsDone)
                yield return loadingOp;
            const int startEssence = 100;
            EssenceSystem essence = tempPlayer.Essence;
            loaded.GainPerk(tempPlayer);
            switch (startGender)
            {
                case Gender.Doll:
                    break;
                case Gender.Male:
                    essence.Masculinity.Amount += startEssence;
                    break;
                case Gender.Female:
                    essence.Femininity.Amount += startEssence;
                    break;
                case Gender.CuntBoy:
                    essence.Femininity.Amount += startEssence;
                    if (tempPlayer.SexualOrgans.Vaginas.TryGrowNew(essence.Femininity))
                    {
                        tempPlayer.SexualOrgans.Vaginas.GrowFirstAsMuchAsPossible(essence.Femininity);
                    }

                    essence.Femininity.Amount = 0;
                    break;
                case Gender.DickGirl:
                    essence.Masculinity.Amount += startEssence / 2;
                    essence.Femininity.Amount += startEssence / 2;
                    if (tempPlayer.SexualOrgans.Boobs.HaveAny() ||
                        tempPlayer.SexualOrgans.Boobs.TryGrowNew(essence.Femininity))
                    {
                        tempPlayer.SexualOrgans.Boobs.GrowFirstAsMuchAsPossible(essence.Femininity);
                    }

                    essence.Femininity.Amount = 0;
                    break;
                case Gender.Futanari:
                    essence.Masculinity.Amount += startEssence / 2;
                    essence.Femininity.Amount += startEssence / 2;
                    break;
                case Gender.MaleFutanari:
                    essence.Masculinity.Amount += startEssence / 2;
                    essence.Femininity.Amount += startEssence / 2;
                    if (tempPlayer.SexualOrgans.Vaginas.HaveAny() ||
                        tempPlayer.SexualOrgans.Vaginas.TryGrowNew(essence.Femininity))
                    {
                        tempPlayer.SexualOrgans.Vaginas.GrowFirstAsMuchAsPossible(essence.Femininity);
                    }
                    essence.Femininity.Amount = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            tempPlayer.GrowOrgans();
            essence.Femininity.Amount = 0;
            essence.Masculinity.Amount = 0;
        }
        void ChangeStartGender(int arg0)
        {
            genderInfo.text = "Loading..";
            startGender = UgreTools.IntToEnum(arg0, Gender.Doll);
            if (loadingOp.IsValid())
                Addressables.Release(loadingOp);
            loadingOp = startGender switch
            {
                Gender.Doll => doll.LoadAssetAsync<StartGenderPerk>(),
                Gender.Male => male.LoadAssetAsync<StartGenderPerk>(),
                Gender.Female => female.LoadAssetAsync<StartGenderPerk>(),
                Gender.CuntBoy => cuntBoy.LoadAssetAsync<StartGenderPerk>(),
                Gender.DickGirl => dickgirl.LoadAssetAsync<StartGenderPerk>(),
                Gender.Futanari => futa.LoadAssetAsync<StartGenderPerk>(),
                Gender.MaleFutanari => maleFuta.LoadAssetAsync<StartGenderPerk>(),
                _ => throw new ArgumentOutOfRangeException(),
            };
            loadingOp.Completed += LoadedGender;
        }

        void LoadedGender(AsyncOperationHandle<StartGenderPerk> obj)
        {
            loaded = obj.Result;
            PrintGenderInfo();
        }

        void PrintGenderInfo()
        {
            genderInfo.text = $"{UgreTools.StringFormatting.AddSpaceAfterCapitalLetter(loaded.ToString(), true)}\n" +
                              $"{Info()}\n\n(In future builds there will more effects of your start gender)";
            string Info() =>
                startGender switch
                {
                    Gender.Doll => "You are an empty canvas making it easier for you to be transformed.",
                    Gender.Male => "Start with an average sized dick & balls.",
                    Gender.Female => "Start with an average sized vagina & boobs.",
                    Gender.CuntBoy =>
                        "Start with a vagina and a moderate amount of stable essence making it harder for you to be transformed involuntary.",
                    Gender.DickGirl =>
                        "Start with dick & boobs and some stable essence making it harder for you to be transformed involuntary.",
                    Gender.Futanari => "Start with smaller male & female equipment.",
                    Gender.MaleFutanari => "Dick, balls and vagina but no boobs.",
                    _ => "",
                };
        }
    }
}