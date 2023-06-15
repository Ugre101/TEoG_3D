using Currency;

namespace Character.PlayerStuff.Currency {
    public static class PlayerGold {
        public static GoldBag GoldBag { get; } = new(100);
        public static GoldSave Save() => new(GoldBag.Gold);

        public static void Load(int load) => GoldBag.Load(load);
    }
}