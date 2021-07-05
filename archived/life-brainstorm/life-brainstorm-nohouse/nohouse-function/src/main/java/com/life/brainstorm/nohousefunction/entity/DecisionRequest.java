package com.life.brainstorm.nohousefunction.entity;

import com.fasterxml.jackson.annotation.JsonFormat;
import lombok.Data;

import java.time.LocalDate;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Data
public abstract class DecisionRequest {
    @JsonFormat(pattern = "yyyy-MM-dd")
    private String startFrom;

    private double baseCurrencyPool;
    private double baseHousePool;

    private double monthlySalary;
    private double annuallyBonus;

    private double monthlyLifeCharges;
    private Map<String, Double> otherAnnuallyCharges;

    private double currencyIncreaseRate;
    private double houseIncreaseRate;

    protected Map<Integer, Double> currencyHistory = new HashMap<>();

    public LocalDate getStartDate() {
        return LocalDate.parse(getStartFrom());
    }

    public abstract double getMonthlyDelta(int index);

    public abstract List<DataPoint> getDataPoints(int index);

    protected double getMonthlyBonus() {
        return getAnnuallyBonus() / 12d;
    }

    protected double getMonthlyOtherCharges() {
        if (getOtherAnnuallyCharges() == null || getOtherAnnuallyCharges().size() == 0) {
            return 0;
        }

        return getOtherAnnuallyCharges().values().stream().reduce(0d, (x, y) -> x + y).doubleValue() / 12d;
    }

    protected void ensureUpdateCurrencyHistory(int count) {
        for (int i = 0; i < count; i++) {
            if (!currencyHistory.containsKey(i)) {
                if (i == 0) {
                    currencyHistory.put(i, getBaseCurrencyPool());
                } else {
                    double preCurrencyPool = currencyHistory.get(i - 1);

                    currencyHistory.put(i, preCurrencyPool * (1 + getCurrencyIncreaseRate() / 12.0d)
                            + getMonthlyDelta(i));
                }
            }
        }
    }

    public double getCurrentHousePool(int index) {
        double range = index / 12d;
        return getBaseHousePool() * Math.pow((1 + getHouseIncreaseRate()), range);
    }

    public double getCurrentPool(int index) {
        return currencyHistory.get(index) + getCurrentHousePool(index);
    }
}
