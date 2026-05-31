using GorillaExtensions;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using PlayFab;
using System;
using UnityEngine;

[HarmonyPatch(typeof(GorillaComputer))]
[HarmonyPatch("GeneralFailureMessage")]
internal class ComputerPatch
{
    private static bool Prefix(string failMessage)
    {
        bool flag = failMessage.Contains("BANNED") || failMessage.Contains("AUTHENTICATE");
        bool result;
        if (flag)
        {
            result = false;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            result = true;
        }
        return result;
    }
}