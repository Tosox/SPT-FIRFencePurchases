using HarmonyLib;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.Helpers.Commerce;
using SPTarkov.Server.Core.Models.Eft.Trade;
using SPTarkov.Server.Core.Models.Enums;
using System.Reflection;

namespace Tosox.FIRFencePurchases.Patches
{
    [Injectable]
    public class TradeHelperPatch : AbstractPatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TradeHelper), nameof(TradeHelper.BuyItem));
        }

        [PatchPrefix]
        public static bool PatchPrefix(ProcessBuyTradeRequestData buyRequestData, ref bool foundInRaid)
        {
            // Mark bought item as FIR
            foundInRaid |= buyRequestData.Type == ItemEventActions.BUY_FROM_TRADER
                && buyRequestData.TransactionId == Traders.FENCE;

            return true;
        }
    }
}
