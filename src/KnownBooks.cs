using System.Collections.Generic;

namespace ReadBookMarker
{
    /// <summary>
    /// Decides whether an item is already used up for this player.
    ///
    /// Books and skill magazines both carry an "Unlocks" property naming a progression entry,
    /// and every progression entry has its own MaxLevel: 1 for a book, 50/75/100 for a crafting
    /// skill. So one check covers both cases - a book that has been read, and a magazine whose
    /// skill sits at its cap.
    /// </summary>
    public static class KnownBooks
    {
        const string cPropUnlocks = "Unlocks";

        // itemClass id -> progression name, empty string when the item is not a book at all
        static readonly Dictionary<int, string> unlocksByItem = new Dictionary<int, string>();

        public static bool IsKnown(ItemClass _itemClass, EntityPlayer _player)
        {
            if (_itemClass == null || _player == null) return false;

            var progressionName = GetProgressionName(_itemClass);
            if (string.IsNullOrEmpty(progressionName)) return false;

            var progression = _player.Progression;
            if (progression == null) return false;

            var value = progression.GetProgressionValue(progressionName);
            if (value == null) return false;

            var progressionClass = value.ProgressionClass;
            if (progressionClass == null || progressionClass.MaxLevel <= 0) return false;

            return value.Level >= progressionClass.MaxLevel;
        }

        /// <summary>Progression this item unlocks, cached because bindings run for every slot.</summary>
        static string GetProgressionName(ItemClass _itemClass)
        {
            if (unlocksByItem.TryGetValue(_itemClass.Id, out var cached)) return cached;

            var name = string.Empty;
            var properties = _itemClass.Properties;

            if (properties != null && properties.Values != null &&
                properties.Values.TryGetValue(cPropUnlocks, out var unlocks) &&
                !string.IsNullOrEmpty(unlocks))
            {
                // Schematics also use Unlocks, but they point at recipes rather than progression,
                // so anything without a progression class of its own is dropped here.
                if (Progression.ProgressionClasses != null && Progression.ProgressionClasses.ContainsKey(unlocks))
                    name = unlocks;
            }

            unlocksByItem[_itemClass.Id] = name;
            return name;
        }

        public static void Clear() => unlocksByItem.Clear();
    }
}
