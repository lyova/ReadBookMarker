using HarmonyLib;

namespace ReadBookMarker
{
    /// <summary>
    /// Answers the custom binding used by the check mark sprite added to the item slot template.
    /// </summary>
    [HarmonyPatch(typeof(XUiC_ItemStack), nameof(XUiC_ItemStack.GetBindingValueInternal))]
    public static class Patch_ItemStack_GetBindingValue
    {
        public const string cBinding = "rbmknown";

        static void Postfix(XUiC_ItemStack __instance, ref bool __result, ref string _value, string _bindingName)
        {
            if (_bindingName != cBinding) return;

            try
            {
                var stack = __instance.ItemStack;
                var player = __instance.xui != null && __instance.xui.playerUI != null
                    ? __instance.xui.playerUI.entityPlayer
                    : null;

                var known = stack != null && !stack.IsEmpty() &&
                            KnownBooks.IsKnown(__instance.itemClass, player);

                _value = known ? "true" : "false";
            }
            catch (System.Exception e)
            {
                Log.Error("[ReadBookMarker] binding: " + e);
                _value = "false";
            }

            __result = true;
        }
    }
}
