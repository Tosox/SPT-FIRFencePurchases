using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Utils;

namespace Tosox.FIRFencePurchases
{
    [Injectable(TypePriority = OnLoadOrder.PreSptModLoader)]
    public class FIRFencePurchasesPre(
        ISptLogger<FIRFencePurchasesPre> logger
    ) : IOnLoad
    {
        public Task OnLoad()
        {
            new Patches.TradeHelperPatch().Enable();
            logger.Info("[FIRFencePurchases] Patched TradeHelper.BuyItem - Bought Fence items are now FIR");

            return Task.CompletedTask;
        }
    }

    [Injectable(TypePriority = OnLoadOrder.PostSptModLoader)]
    public class FIRFencePurchasesPost(
        ISptLogger<FIRFencePurchasesPost> logger
    ) : IOnLoad
    {
        public Task OnLoad()
        {
            new Patches.FenceServicePatch().Enable();
            logger.Info("[FIRFencePurchases] Patched FenceService.GetFenceAssorts - Fence assort is now FIR");

            return Task.CompletedTask;
        }
    }
}
