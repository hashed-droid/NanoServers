using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace NanoServers.Patches
{
    [HarmonyPatch(typeof(PlayFabAuthenticator), "AdvanceLogin")]
    public static class Patch_AdvanceLogin
    {
        private static void Postfix(PlayFabAuthenticator __instance)
        {
            if (CosmeticsController.instance != null)
            {
                CosmeticsController.instance.GetCurrencyBalance();
                if (GorillaTagger.Instance != null && GorillaTagger.Instance.offlineVRRig != null)
                {
                    GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
                }
                CosmeticsController.instance.Initialize();
                CosmeticsController.instance.UpdateWardrobeModelsAndButtons();
            }
        }
    }
}