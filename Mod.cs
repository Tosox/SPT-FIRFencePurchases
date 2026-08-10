using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Reflection.Patching;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace Tosox.FIRFencePurchases
{
    [Injectable(TypePriority = OnLoadOrder.Preload + 1)]
    public class FIRFencePurchases(
        ISptLogger<FIRFencePurchases> logger,
        IEnumerable<IRuntimePatch> patches
    ) : IOnLoad
    {
        public Task OnLoadAsync(CancellationToken cancellationToken)
        {
            foreach (var patch in patches)
            {
                patch.Enable();
            }

            logger.Info($"[{ModMetadata.ModName}] Bought items from Fence will now be marked as Found in Raid");
            return Task.CompletedTask;
        }
    }

    public record ModMetadata : IModMetadata
    {
        internal const string ModName = "FIR Fence Purchases";
        internal const string ModVersion = "1.1.0";
        internal const string ModAuthor = "Tosox";
        internal const string ModSource = "https://github.com/Tosox/SPT-FIRFencePurchases";

        public string ModGuid { get; init; } = "de.tosox.firfencepurchases";
        public string Name { get; init; } = ModName;
        public string Author { get; init; } = ModAuthor;
        public List<string>? Contributors { get; init; }
        public SemanticVersioning.Version Version { get; init; } = new(ModVersion);
        public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
        public bool HasPrepatcher { get; init; }
        public List<string>? Incompatibilities { get; init; }
        public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
        public string? Url { get; init; } = ModSource;
        public string License { get; init; } = "MIT";
    }
}
