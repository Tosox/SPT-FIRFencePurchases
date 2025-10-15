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
            // Mark items as FIR in the trader's assortment only
            // NOTE: Items marked as FIR in the trader's assortment will not have their
            //       FIR status when bought
            foreach (var item in __result.Items)
            {
                item.Upd ??= new Upd();
                item.Upd.SpawnedInSession = true;
            }
        }
    }
}
