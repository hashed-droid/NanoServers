using System;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace NanoServers.HarmonyPatches
{
    [HarmonyPatch(typeof(GorillaComputer), "ProcessRoomState")]
    internal class ComputerPatchRoom
    {
        private static bool Prefix(GorillaComputer __instance, GorillaKeyboardBindings buttonPressed)
        {
            if (buttonPressed == GorillaKeyboardBindings.enter && !string.IsNullOrEmpty(__instance.roomToJoin))
            {
                string codeToJoin = __instance.roomToJoin;
                Debug.Log($"Attempting to join private room: {codeToJoin}");

                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(codeToJoin, JoinType.Solo);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(GorillaComputer), "ProcessNameState")]
    internal class ComputerPatchName
    {
        private static bool Prefix(GorillaComputer __instance, GorillaKeyboardBindings buttonPressed)
        {
            if (buttonPressed == GorillaKeyboardBindings.enter && !string.IsNullOrEmpty(__instance.currentName))
            {
                NetworkSystem.Instance.SetMyNickName(__instance.currentName);
                __instance.savedName = __instance.currentName;

                return false;
            }
            return true;
        }
    }
}