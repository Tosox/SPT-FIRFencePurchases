import type { DependencyContainer } from "tsyringe";

import type { IItemEventRouterResponse } from "@spt/models/eft/itemEvent/IItemEventRouterResponse";
import type { IPmcData } from "@spt/models/eft/common/IPmcData";
import type { IProcessBaseTradeRequestData } from "@spt/models/eft/trade/IProcessBaseTradeRequestData";
import type { ITraderAssort } from "@spt/models/eft/common/tables/ITrader";
import type { IPostSptLoadMod } from "@spt/models/external/IPostSptLoadMod";
import type { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import type { TradeHelper } from "@spt/helpers/TradeHelper";
import type { ILogger } from "@spt/models/spt/utils/ILogger";
import type { FenceService } from "@spt/services/FenceService";

import { Traders } from "@spt/models/enums/Traders";

export class FIRFencePurchases implements IPreSptLoadMod, IPostSptLoadMod {
    public preSptLoad(container: DependencyContainer): void {
        container.afterResolution("TradeHelper", (_t, tradeHelper: TradeHelper) => {
            const logger = container.resolve<ILogger>("WinstonLogger");

            const oTradeHelperBuyItem = tradeHelper.buyItem.bind(tradeHelper);
            tradeHelper.buyItem = (
                pmcData: IPmcData,
                request: IProcessBaseTradeRequestData,
                sessionID: string,
                foundInRaid: boolean,
                output: IItemEventRouterResponse
            ): void => {
                // Mark bought item as FIR
                const shouldMarkFIR = request.type === "buy_from_trader" && request.tid === Traders.FENCE;
                const setFIR = foundInRaid || shouldMarkFIR;

                oTradeHelperBuyItem(pmcData, request, sessionID, setFIR, output);
            };

            logger.info("[FIRFencePurchases] Patched TradeHelper.buyItem - Bought Fence items are now FIR");
        }, { frequency: "Always" });
    }
	
    public postSptLoad(container: DependencyContainer): void {
        container.afterResolution("FenceService", (_t, fenceService: FenceService) => {
            const logger = container.resolve<ILogger>("WinstonLogger");

            const oFenceServiceGetFenceAssorts = fenceService.getFenceAssorts.bind(fenceService);
            fenceService.getFenceAssorts = (pmcProfile: IPmcData): ITraderAssort => {
                const result: ITraderAssort = oFenceServiceGetFenceAssorts(pmcProfile);

                // Mark items as FIR in the trader's assortment only
                // NOTE: Items marked as FIR in the trader's assortment will not have their
                //       FIR status when bought
                result.items = result.items.map(item => {
                    item.upd = item.upd ?? {};
                    item.upd.SpawnedInSession = true;
                    return item;
                });

                return result;
            };

            logger.info("[FIRFencePurchases] Patched FenceService.getFenceAssorts - Fence assort is now FIR");
        }, { frequency: "Always" });
    }
}

module.exports = { mod: new FIRFencePurchases() };
