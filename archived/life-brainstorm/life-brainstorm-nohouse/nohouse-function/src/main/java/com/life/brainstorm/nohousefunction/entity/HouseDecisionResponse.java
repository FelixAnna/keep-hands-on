package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

@Data
public class HouseDecisionResponse extends DecisionResponse {
    public double baseHousePool;
    public double latestHousePool;
    public double houseIncreaseRate;

    public double baseCurrencyPool;
    public double latestCurrencyPool;
    public double currencyIncreaseRate;

    public double baseDebt;
    public double totalInterest;
    public double totalRepay;

    public double houseDebt;
    public double totalBalance;
}
