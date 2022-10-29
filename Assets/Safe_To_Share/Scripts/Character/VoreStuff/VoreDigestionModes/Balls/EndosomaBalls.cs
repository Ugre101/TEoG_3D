using Character;
using Character.Organs;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes.Balls
{
    public class EndosomaBalls : DigestionMethod
    {
        public override bool Tick(BaseCharacter pred, BaseOrgan baseOrgan, bool predIsPlayer)
        {
            baseOrgan.Vore.NoDigestTick();
            foreach (int preysId in baseOrgan.Vore.PreysIds)
                if (VoredCharacters.PreyDict.TryGetValue(preysId, out var prey))
                    if (DateSystem.DateSaveHoursAgo(prey.VoredDate) >
                        8) // Takes time before they start finding there way in
                        if (prey.EndosomaTryImpregnate(pred))
                            break; // Only impregnate max one each tick
            // try impregnate
            return false;
        }
    }
}