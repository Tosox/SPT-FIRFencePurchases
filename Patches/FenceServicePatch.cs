using HarmonyLib;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Services;
using System.Reflection;

namespace Tosox.FIRFencePurchases.Patches
{
    public class FenceServicePatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(FenceService), nameof(FenceService.GetFenceAssorts));
        }

        [PatchPostfix]
        public static void PatchPostfix(ref TraderAssort __result)
        {
            __result.Items = [.. __result.Items
                .Select(item => {
                    item.Upd ??= new Upd();
                    item.Upd.SpawnedInSession = true;
                    return item;
                })];
        }
    }
}
