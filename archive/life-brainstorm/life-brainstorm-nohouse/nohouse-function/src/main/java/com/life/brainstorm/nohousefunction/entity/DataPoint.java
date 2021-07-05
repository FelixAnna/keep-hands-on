package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

import java.time.LocalDate;

@Data
public class DataPoint {
    private LocalDate date;
    private double currencyPool;
    private double housePool;
    private double totalPool;

    private double remainingMerchantDebt;
    private double remainingGongjijinDebt;

    private double merchantDebtInterest;
    private double gongjijinDebtInterest;

    private double merchantDebtMonthlyRepay;
    private double gongjijinDebtMonthlyRepay;
}
