using HarmonyLib;
using System.Threading.Tasks;

namespace NanoServers.Patches
{
    public class TOSPatches
    {
        private readonly static bool enabled = true;

        [HarmonyPatch(typeof(LegalAgreements), "Update")]
        public class Update
        {
            private static bool Prefix(LegalAgreements __instance)
            {
                if (enabled)
                {
                    ControllerInputPoller.instance.leftControllerPrimary2DAxis.y = -1f;
                    Traverse.Create(__instance).Field("_currentAge").SetValue(10f);
                    Traverse.Create(__instance).Field("_currentAge").SetValue(10f);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ModIOTermsOfUse_v1), "PostUpdate")]
        public class PostUpdateModIO
        {
            private static bool Prefix(ModIOTermsOfUse_v1 __instance)
            {
                if (enabled)
                {
                    __instance.TurnPage(999);
                    ControllerInputPoller.instance.leftControllerPrimary2DAxis.y = -1f;
                    Traverse.Create(__instance).Field("_currentAge").SetValue(0.1f);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AgeSlider), "PostUpdate")]
        public class PostUpdateAgeSlider
        {
            private static bool Prefix(AgeSlider __instance)
            {
                if (enabled)
                {
                    Traverse.Create(__instance).Field("_currentAge").SetValue(21);
                    Traverse.Create(__instance).Field("_currentAge").SetValue(0.1f);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(PrivateUIRoom), "StartOverlay")]
        public class StartOverlay
        {
            private static bool Prefix() =>
                !enabled;
        }

        [HarmonyPatch(typeof(KIDManager), "UseKID")]
        public class UseKID
        {
            private static bool Prefix(ref Task<bool> __result)
            {
                if (!enabled)
                    return true;

                __result = Task.FromResult(false);
                return false;
            }
        }
    }
}