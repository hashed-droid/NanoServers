using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace NanoServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaLevelScreen))]
    [HarmonyPatch("UpdateText")]
    internal class GorillaLevelScreenPatch
    {
        private static bool Prefix(GorillaLevelScreen __instance, ref Text ___myText)
        {
            bool flag = ___myText == null || __instance.GetComponent<MeshRenderer>() == null;
            return !flag;
        }
    }
}