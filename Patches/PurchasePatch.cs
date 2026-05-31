using GorillaNetworking;
using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace NanoServers.Patches
{
    [HarmonyPatch(typeof(CosmeticsController), "PurchaseItem")]
    public static class Patch_PurchaseItem
    {
        private static bool Prefix()
        {
            var controller = CosmeticsController.instance;

            if (controller.itemToBuy.isNullItem || controller.itemToBuy.itemName == controller.nullItem.itemName)
            {
                return false;
            }

            string id = controller.itemToBuy.itemName;

            var request = new ExecuteCloudScriptRequest
            {
                FunctionName = "buyFreeCosmetic",
                FunctionParameter = new { itemID = id }
            };

            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!controller.allCosmeticsDict.ContainsKey(id))
                {
                    controller.allCosmeticsDict.Add(id, controller.itemToBuy);
                }

                if (GorillaTagger.Instance?.offlineVRRig != null)
                {
                    GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
                }

                controller.itemToBuy = controller.nullItem;
                controller.UpdateWardrobeModelsAndButtons();
            }, error => {
                controller.itemToBuy = controller.nullItem;
            });

            return false;
        }
    }
}