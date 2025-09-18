using UnityEngine;

namespace Data
{
    public static class GameResources
    {
        public const string ElementalPath = "Elementals";

        public static ElementalData[] LoadAllElementalData()
            => Resources.LoadAll<ElementalData>(ElementalPath);
    }
}