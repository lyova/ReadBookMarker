using System.Collections.Generic;

namespace ReadBookMarker
{
    /// <summary>
    /// Decides whether an item is already used up for this player.
    ///
    /// Books, skill magazines and schematics all carry an "Unlocks" property, but it points at
    /// two different things:
    ///   - books and magazines name a progression entry, which has its own MaxLevel
    ///     (1 for a book, 50/75/100 for a crafting skill)
    ///   - schematics name a recipe, unlocked through a CVar
    /// So the check is: progression at its cap, or recipe already known.
    /// </summary>
    public static class KnownBooks
    {
        const string cPropUnlocks = "Unlocks";

        enum UnlockKind { None, Progression, Recipe }

        struct Unlock
        {
            public UnlockKind Kind;
            public string Name;
        }

        // itemClass id -> what this item unlocks, cached because bindings run for every slot
        static readonly Dictionary<int, Unlock> unlocksByItem = new Dictionary<int, Unlock>();

        public static bool IsKnown(ItemClass _itemClass, EntityPlayer _player, XUi _xui)
        {
            if (_itemClass == null || _player == null) return false;

            var unlock = GetUnlock(_itemClass);

            switch (unlock.Kind)
            {
                case UnlockKind.Progression: return IsProgressionMaxed(unlock.Name, _player);
                case UnlockKind.Recipe:      return _xui != null && XUiM_Recipes.GetRecipeIsUnlocked(_xui, unlock.Name);
                default:                     return false;
            }
        }

        static bool IsProgressionMaxed(string _name, EntityPlayer _player)
        {
            var progression = _player.Progression;
            if (progression == null) return false;

            var value = progression.GetProgressionValue(_name);
            if (value == null) return false;

            var progressionClass = value.ProgressionClass;
            if (progressionClass == null || progressionClass.MaxLevel <= 0) return false;

            return value.Level >= progressionClass.MaxLevel;
        }

        /// <summary>What this item unlocks, and through which system.</summary>
        static Unlock GetUnlock(ItemClass _itemClass)
        {
            if (unlocksByItem.TryGetValue(_itemClass.Id, out var cached)) return cached;

            var unlock = new Unlock { Kind = UnlockKind.None, Name = string.Empty };
            var properties = _itemClass.Properties;

            if (properties != null && properties.Values != null &&
                properties.Values.TryGetValue(cPropUnlocks, out var unlocks) &&
                !string.IsNullOrEmpty(unlocks))
            {
                var isProgression = Progression.ProgressionClasses != null &&
                                    Progression.ProgressionClasses.ContainsKey(unlocks);

                unlock.Kind = isProgression ? UnlockKind.Progression : UnlockKind.Recipe;
                unlock.Name = unlocks;
            }

            unlocksByItem[_itemClass.Id] = unlock;
            return unlock;
        }

        public static void Clear() => unlocksByItem.Clear();
    }
}
