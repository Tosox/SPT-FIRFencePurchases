using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;

using Range = SemanticVersioning.Range;
using Version = SemanticVersioning.Version;

namespace Tosox.FIRFencePurchases
{
    public record ModMetadata : AbstractModMetadata
    {
        public override string ModGuid { get; init; } = "de.tosox.firfencepurchases";
        public override string Name { get; init; } = "FIR Fence Purchases";
        public override string Author { get; init; } = "Tosox";
        public override List<string>? Contributors { get; init; }
        public override Version Version { get; init; } = new("1.0.0");
        public override Range SptVersion { get; init; } = new("~4.0.0");
        public override List<string>? Incompatibilities { get; init; }
        public override Dictionary<string, Range>? ModDependencies { get; init; }
        public override string? Url { get; init; } = "https://github.com/Tosox/SPT-FIRFencePurchases";
        public override bool? IsBundleMod { get; init; }
        public override string License { get; init; } = "MIT";
    }

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
