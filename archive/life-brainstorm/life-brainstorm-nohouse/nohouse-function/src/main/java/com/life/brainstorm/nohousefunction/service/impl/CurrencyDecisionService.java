package com.life.brainstorm.nohousefunction.service.impl;

import com.life.brainstorm.nohousefunction.entity.CurrencyDecisionResponse;
import com.life.brainstorm.nohousefunction.entity.DataPoint;
import com.life.brainstorm.nohousefunction.entity.DecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionResponse;
import com.life.brainstorm.nohousefunction.service.DecisionService;

import java.util.List;

public class CurrencyDecisionService implements DecisionService {
    @Override
    public DecisionResponse getResponse(DecisionRequest request, int range) {
        List<DataPoint> points = request.getDataPoints(range);
        CurrencyDecisionResponse response = new CurrencyDecisionResponse();
        response.setDataPoints(points);

        double baseCurrency = points.get(0).getCurrencyPool();
        double latestCurrency = points.get(range - 1).getCurrencyPool();
        response.setLatestCurrencyPool(latestCurrency);
        response.setBaseCurrencyPool(baseCurrency);
        response.setCurrencyIncreaseRate(latestCurrency / baseCurrency);
        response.setTotalBalance(points.get(range - 1).getTotalPool());

        return response;
    }
}
