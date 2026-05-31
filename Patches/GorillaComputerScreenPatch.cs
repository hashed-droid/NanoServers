using System;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;

namespace NanoServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer), "UpdateScreen")]
    internal class GorillaComputerScreenPatch
    {
        private static void Postfix(GorillaComputer __instance)
        {
            string str = PhotonNetwork.CountOfPlayers.ToString();

            if (__instance.currentState == GorillaComputer.ComputerState.Startup)
            {
                __instance.screenText.Set("Nano Servers\n\n" + str + " PLAYERS ONLINE\n\n\n\nPRESS ANY KEY TO BEGIN");
            }
        }
    }
}