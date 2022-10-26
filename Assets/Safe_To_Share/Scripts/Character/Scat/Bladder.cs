using Safe_to_Share.Scripts.CustomClasses;

namespace Safe_To_Share.Scripts.Character.Scat
{
    public class Bladder
    {
        float current;
        BaseConstFloatStat maxPressure = new(100);

        public void Fill(float amount)
        {
            current += amount;
            
        }
    }
}