using UnityEngine;
using PlayFab;
using BepInEx;

namespace NanoServers
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Debug.Log("Old title ID: " + PlayFabSettings.TitleId);
            PlayFabSettings.TitleId = "12D3A3";
            PlayFabAuthenticatorSettings.TitleId = "12D3A3";

            NanoServers.HarmonyPatches.HarmonyPatches.ApplyHarmonyPatches();

            Debug.Log("New title ID: " + PlayFabSettings.TitleId);
            Debug.Log(PlayFabAuthenticatorSettings.AuthApiBaseUrl);
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public class PluginInfo
    {
        public const string GUID = "com.ivy.NanoServers";
        public const string Name = "NanoServers";
        public const string Version = "1.0.0";
    }
}