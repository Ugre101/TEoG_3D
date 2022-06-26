using Static;

namespace Character.BodyStuff
{
    public static class BodyLooks
    {
        public static string HeightAndWeight(this Body body) =>
            $"{body.Height.Value.ConvertCm()} tall and {body.Weight.ConvertKg()}";
    }
}