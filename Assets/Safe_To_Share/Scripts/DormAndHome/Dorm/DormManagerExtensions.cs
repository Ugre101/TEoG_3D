using UnityEngine;

namespace DormAndHome.Dorm {
    public static class DormManagerExtensions {
        const string FreeRangeSave = "DormFreeRangeLimit";
        static int? freeRangeLimit;


        public static int FreeRangeLimit {
            get {
                freeRangeLimit ??= PlayerPrefs.GetInt(FreeRangeSave, 5);
                return freeRangeLimit.Value;
            }
            set {
                freeRangeLimit = value;
                PlayerPrefs.SetInt(FreeRangeSave, value);
            }
        }
    }
}