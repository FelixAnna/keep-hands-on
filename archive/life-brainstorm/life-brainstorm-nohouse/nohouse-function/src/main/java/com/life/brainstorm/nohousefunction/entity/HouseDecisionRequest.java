package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 买房：
 * 欠债=30w
 * 商代=110w
 * 公积金贷=90w
 * <p>
 * 月供=（商贷+公积金贷）/ 45 * 2400 = 10000
 * <p>
 * 月度资金变化 = 月收入 - 月供 - 月生活开销 - 月均摊年底开销 + 月均摊年终奖收入 = 4500
 * 资金池 = 欠债 + 月度资金变化 * （date - 2020/09/01）
 * 月剩余房贷减额=2500
 * <p>
 * 净资产=资金池 + 基础房价 * 升值率 * （date - 2020/12/31）- 剩余房贷
 * <p>
 * 租房：房价升值率1.9%跑赢
 * 稳健投资：房价升值2.5%跑赢
 * <p>
 * 房产/资金比 = 100%/0 （5年内）
 */
@Data
public class HouseDecisionRequest extends DecisionRequest {
    private double merchantDebt;
    private double gongjijinDebt;

    private final double merchantBaseRate = 0.0465d;
    private final double gongjijinBaseRate = 0.0325d;

    private double merchantRateTimes;
    private double gongjijinRateTimes;

    private int merchantTerm;
    private int gongjijinTerm;

    private Map<Integer, Double> remainingMerchantDebt = new HashMap<>();
    private Map<Integer, Double> remainingGongjijinDebt = new HashMap<>();
    private Map<Integer, Double> merchantDebtMonthlyInterest = new HashMap<>();
    private Map<Integer, Double> gongjijinDebtgMonthlyInterest = new HashMap<>();
    private Map<Integer, Double> merchantDebtMonthlyRepay = new HashMap<>();
    private Map<Integer, Double> gongjijinDebtgMonthlyRepay = new HashMap<>();

    @Override
    public double getCurrentHousePool(int index) {
        return super.getCurrentHousePool(index)
                - this.remainingMerchantDebt.get(index)
                - this.remainingGongjijinDebt.get(index);
    }

    @Override
    public double getMonthlyDelta(int index) {
        return super.getMonthlySalary()
                + getMonthlyBonus()
                - getMonthlyDebtRepay(index)
                - getMonthlyLifeCharges()
                - getMonthlyOtherCharges();
    }

    private double getMonthlyDebtRepay(int index) {
        return gongjijinDebtgMonthlyRepay.get(index) + merchantDebtMonthlyRepay.get(index);
    }

    @Override
    public List<DataPoint> getDataPoints(int index) {
        ensureRemainingMerchantDebt(index);
        ensureRemainingGongjijinDebt(index);
        ensureUpdateCurrencyHistory(index);

        List<DataPoint> points = new ArrayList<>();
        for (int i = 0; i < index; i++) {

            DataPoint point = new DataPoint();
            point.setCurrencyPool(this.currencyHistory.get(i));
            point.setHousePool(this.getCurrentHousePool(i));
            point.setTotalPool(this.getCurrentPool(i));
            point.setDate(this.getStartDate().plusMonths(i));

            point.setRemainingMerchantDebt(this.remainingMerchantDebt.get(i));
            point.setRemainingGongjijinDebt(this.remainingGongjijinDebt.get(i));

            point.setMerchantDebtMonthlyRepay(this.merchantDebtMonthlyRepay.get(i));
            point.setGongjijinDebtMonthlyRepay(this.gongjijinDebtgMonthlyRepay.get(i));

            point.setMerchantDebtInterest(this.merchantDebtMonthlyInterest.get(i));
            point.setGongjijinDebtInterest(this.gongjijinDebtgMonthlyInterest.get(i));

            points.add(point);
        }

        return points;
    }

    private void ensureRemainingMerchantDebt(int count) {
        for (int i = 0; i < merchantTerm * 12; i++) {
            if (!remainingMerchantDebt.containsKey(i)) {
                if (i == 0) {
                    remainingMerchantDebt.put(i, getMerchantDebt());
                    merchantDebtMonthlyRepay.put(i, 0d);
                    merchantDebtMonthlyInterest.put(i, 0d);
                } else {
                    double preDebt = remainingMerchantDebt.get(i - 1);

                    //简单起见：使用等额本金
                    double payInterest = preDebt * merchantBaseRate / 12;
                    double payTotal = preDebt * merchantBaseRate / 12 + getMerchantDebt() / merchantTerm / 12;
                    merchantDebtMonthlyInterest.put(i, payInterest);
                    merchantDebtMonthlyRepay.put(i, payTotal);
                    remainingMerchantDebt.put(i, preDebt - payInterest);
                }
            }
        }

        if (count > merchantTerm * 12) {
            for (int i = merchantTerm * 12; i < count; i++) {
                merchantDebtMonthlyInterest.put(i, 0d);
                merchantDebtMonthlyRepay.put(i, 0d);
                remainingMerchantDebt.put(i, 0d);
            }
        }
    }

    private void ensureRemainingGongjijinDebt(int count) {
        for (int i = 0; i < gongjijinTerm * 12; i++) {
            if (!remainingGongjijinDebt.containsKey(i)) {
                if (i == 0) {
                    remainingGongjijinDebt.put(i, getGongjijinDebt());
                    gongjijinDebtgMonthlyRepay.put(i, 0d);
                    gongjijinDebtgMonthlyInterest.put(i, 0d);
                } else {
                    double preDebt = remainingGongjijinDebt.get(i - 1);

                    //简单起见：使用等额本金
                    double payInterest = preDebt * gongjijinBaseRate / 12d;
                    double payTotal = payInterest + getGongjijinDebt() / gongjijinTerm / 12;
                    gongjijinDebtgMonthlyInterest.put(i, payInterest);
                    gongjijinDebtgMonthlyRepay.put(i, payTotal);
                    remainingGongjijinDebt.put(i, preDebt - payInterest);
                }
            }
        }

        if (count > gongjijinTerm * 12) {
            for (int i = gongjijinTerm * 12; i < count; i++) {
                gongjijinDebtgMonthlyInterest.put(i, 0d);
                gongjijinDebtgMonthlyRepay.put(i, 0d);
                remainingGongjijinDebt.put(i, 0d);
            }
        }
    }
}
