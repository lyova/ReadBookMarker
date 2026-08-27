using HarmonyLib;

namespace ReadBookMarker
{
    /// <summary>Shared helper: is the item in this slot already spent for the player?</summary>
    static class Marker
    {
        public const string cBinding = "rbmknown";

        public static string Evaluate(XUiController _controller, ItemStack _stack) =>
            Evaluate(_controller, _stack, _stack != null && !_stack.IsEmpty() ? _stack.itemValue.ItemClass : null);

        public static string Evaluate(XUiController _controller, ItemStack _stack, ItemClass _itemClass)
        {
            try
            {
                var player = _controller != null && _controller.xui != null && _controller.xui.playerUI != null
                    ? _controller.xui.playerUI.entityPlayer
                    : null;

                var xui = _controller != null ? _controller.xui : null;
                var known = _stack != null && !_stack.IsEmpty() && KnownBooks.IsKnown(_itemClass, player, xui);
                return known ? "true" : "false";
            }
            catch (System.Exception e)
            {
                Log.Error("[ReadBookMarker] binding: " + e);
                return "false";
            }
        }
    }

    /// <summary>
    /// Answers the custom binding used by the check mark sprite added to the item slot template.
    /// </summary>
    [HarmonyPatch(typeof(XUiC_ItemStack), nameof(XUiC_ItemStack.GetBindingValueInternal))]
    public static class Patch_ItemStack_GetBindingValue
    {
        static void Postfix(XUiC_ItemStack __instance, ref bool __result, ref string _value, string _bindingName)
        {
            if (_bindingName != Marker.cBinding) return;

            _value = Marker.Evaluate(__instance, __instance.ItemStack, __instance.itemClass);
            __result = true;
        }
    }

    /// <summary>And for the big preview in the item info panel.</summary>
    [HarmonyPatch(typeof(XUiC_ItemInfoWindow), nameof(XUiC_ItemInfoWindow.GetBindingValueInternal))]
    public static class Patch_ItemInfoWindow_GetBindingValue
    {
        static void Postfix(XUiC_ItemInfoWindow __instance, ref bool __result, ref string value, string bindingName)
        {
            if (bindingName != Marker.cBinding) return;

            value = Marker.Evaluate(__instance, __instance.itemStack, __instance.itemClass);
            __result = true;
        }
    }

    /// <summary>And for the reward picker shown when a quest is turned in.</summary>
    [HarmonyPatch(typeof(XUiC_QuestTurnInEntry), nameof(XUiC_QuestTurnInEntry.GetBindingValueInternal))]
    public static class Patch_QuestTurnInEntry_GetBindingValue
    {
        static void Postfix(XUiC_QuestTurnInEntry __instance, ref bool __result, ref string value, string bindingName)
        {
            if (bindingName != Marker.cBinding) return;

            value = Marker.Evaluate(__instance, __instance.item);
            __result = true;
        }
    }

    /// <summary>Same binding for the trader's stock list, which uses its own row template.</summary>
    [HarmonyPatch(typeof(XUiC_TraderItemEntry), nameof(XUiC_TraderItemEntry.GetBindingValueInternal))]
    public static class Patch_TraderItemEntry_GetBindingValue
    {
        static void Postfix(XUiC_TraderItemEntry __instance, ref bool __result, ref string value, string bindingName)
        {
            if (bindingName != Marker.cBinding) return;

            value = Marker.Evaluate(__instance, __instance.item, __instance.itemClass);
            __result = true;
        }
    }
}
