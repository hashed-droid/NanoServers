using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Text;
using UnityEngine.UIElements;

namespace NanoServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController), "GetCosmeticsPlayFabCatalogData")]
    internal class CosmeticPatch
    {
        private static readonly MethodInfo AddCosmeticMethod = typeof(CosmeticsController).GetMethod("AddCosmetic", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly FieldInfo CosAgeDict = typeof(CosmeticsController).GetField("_playerOwnedCosmeticsAge", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo BuilderSetsField = typeof(BuilderSetManager).GetField("_starterPieceSets", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo InitField = typeof(CosmeticsController).GetField("initializedCosmetics", BindingFlags.Instance | BindingFlags.NonPublic);

        static bool Prefix(CosmeticsController __instance)
        {
            var addMethod = AddCosmeticMethod ?? HarmonyLib.AccessTools.Method(typeof(CosmeticsController), "AddCosmetic");
            var cosAgeField = CosAgeDict ?? HarmonyLib.AccessTools.Field(typeof(CosmeticsController), "_playerOwnedCosmeticsAge");
            var initField = InitField ?? HarmonyLib.AccessTools.Field(typeof(CosmeticsController), "initializedCosmetics");
            var builderSetsField = BuilderSetsField ?? HarmonyLib.AccessTools.Field(typeof(BuilderSetManager), "_starterPieceSets");

            if (addMethod == null)
            {
                Debug.LogError("Could not find AddCosmetic method on CosmeticsController — aborting patch to avoid crash.");
                return true;
            }

            void AddCosmeticSafe(string id)
            {
                try
                {
                    addMethod.Invoke(__instance, new object[] { id });
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("Failed to AddCosmetic(" + id + "): " + ex.Message);
                }
            }

            var ownedDict = cosAgeField?.GetValue(__instance) as System.Collections.Generic.Dictionary<string, int>;
            AddCosmeticSafe("Slingshot");

            if (BuilderSetManager.instance != null && builderSetsField != null)
            {
                var starterSets = builderSetsField.GetValue(BuilderSetManager.instance) as System.Collections.Generic.List<BuilderPieceSet>;
                if (starterSets != null)
                {
                    foreach (var set in starterSets)
                    {
                        if (set != null && !string.IsNullOrEmpty(set.playfabID))
                        {
                            AddCosmeticSafe(set.playfabID);
                        }
                    }
                }
            }

            foreach (var item in __instance.allCosmetics)
            {
                if (!string.IsNullOrEmpty(item.itemName))
                {
                    AddCosmeticSafe(item.itemName);
                    if (ownedDict != null)
                    {
                        try { ownedDict[item.itemName] = 0; } catch { }
                    }
                }
            }

            try
            {
                initField?.SetValue(__instance, true);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Could not set initialized flag: " + ex.Message);
            }

            try
            {
                if (GorillaTagger.Instance?.offlineVRRig != null)
                {
                    GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
                    CosmeticsController.instance?.UpdateWardrobeModelsAndButtons();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Could not refresh wardrobe models: " + ex.Message);
            }

            Debug.Log("CosmeticPatch: Forced adding of cosmetics and refresh.");
            return false;
        }
    }
}
