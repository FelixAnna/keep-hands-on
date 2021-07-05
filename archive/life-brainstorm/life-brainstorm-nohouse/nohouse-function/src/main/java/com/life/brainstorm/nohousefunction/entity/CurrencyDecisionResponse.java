package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

@Data
public class CurrencyDecisionResponse extends DecisionResponse {
    public double baseCurrencyPool;
    public double latestCurrencyPool;
    public double currencyIncreaseRate;

    public double totalBalance;
}
