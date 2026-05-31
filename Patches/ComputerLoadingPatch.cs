using System;
using GorillaNetworking;
using HarmonyLib;
using PlayFab;

namespace NanoServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("SwitchToLoadingState")]
    internal class ComputerLoadingPatch
    {
        private static bool Prefix()
        {
            bool flag = !PlayFabClientAPI.IsClientLoggedIn();
            return !flag;
        }
    }
}