package com.life.brainstorm.nohousefunction.service.impl;

import com.life.brainstorm.nohousefunction.entity.DataPoint;
import com.life.brainstorm.nohousefunction.entity.DecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionResponse;
import com.life.brainstorm.nohousefunction.entity.HouseDecisionResponse;
import com.life.brainstorm.nohousefunction.service.DecisionService;

import java.util.List;

public class HouseDecisionService implements DecisionService {
    @Override
    public DecisionResponse getResponse(DecisionRequest request, int range) {
        List<DataPoint> points = request.getDataPoints(range);
        HouseDecisionResponse response = new HouseDecisionResponse();
        response.setDataPoints(points);

        double baseCurrency = points.get(0).getCurrencyPool();
        double latestCurrency = points.get(range - 1).getCurrencyPool();
        response.setLatestCurrencyPool(latestCurrency);
        response.setBaseCurrencyPool(baseCurrency);
        response.setCurrencyIncreaseRate(latestCurrency / baseCurrency);
        response.setTotalBalance(points.get(range - 1).getTotalPool());

        double baseDebt = points.get(0).getRemainingGongjijinDebt()
                + points.get(0).getRemainingMerchantDebt();
        double baseHousePool = points.get(0).getHousePool()
                + baseDebt;
        double lastHousePool = points.get(range - 1).getHousePool();
        double remainingDebt = points.get(range - 1).getRemainingMerchantDebt()
                + points.get(range - 1).getRemainingGongjijinDebt();
        response.setBaseHousePool(baseHousePool);
        response.setLatestHousePool(lastHousePool);
        response.setHouseIncreaseRate(lastHousePool / baseHousePool);

        double totalInterest = points.stream()
                .map(x -> x.getMerchantDebtInterest()
                        + x.getGongjijinDebtInterest())
                .reduce(0d, Double::sum);
        double totalRepay = points.stream()
                .map(x -> x.getGongjijinDebtMonthlyRepay()
                        + x.getMerchantDebtMonthlyRepay())
                .reduce(0d, Double::sum);
        response.setBaseDebt(baseDebt);
        response.setTotalInterest(totalInterest);
        response.setTotalRepay(totalRepay);

        response.setHouseDebt(remainingDebt);
        response.setTotalBalance(points.get((range - 1)).getTotalPool());

        return response;
    }
}
