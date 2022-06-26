using UnityEngine;

namespace Static
{
    public static class IDGiver
    {
        static int lastId = 1;

        public static int NewID() => lastId++;

        public static int Save() => lastId;

        public static void Load(int value) => lastId = value;
    }
}