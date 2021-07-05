package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

import java.util.ArrayList;
import java.util.List;

@Data
public class CurrencyDecisionRequest extends DecisionRequest {
    /**
     * 收租=2500
     * 月租金=4500
     * <p>
     * 月度资金变化=月收入 - 租金 - 月生活开销 - 月均摊年底开销 + 月均摊年终奖收入 + 收租 = 12500
     * <p>
     * 资金池 = 基础资金池 + 月度资金变化 * （date - 2020/09/01）
     * <p>
     * 净资产 = 资金池 + 基础房价 * 升值率 * （date - 2020/09/01）
     * <p>
     * 房产/资金比=115 / (35 + 15*years)
     */
    private double monthlyRentIncome;
    private double monthlyRentCharges;

    @Override
    public double getMonthlyDelta(int index) {
        return super.getMonthlySalary()
                + getMonthlyBonus()
                + getMonthlyRentIncome()
                - getMonthlyRentCharges()
                - getMonthlyLifeCharges()
                - getMonthlyOtherCharges();
    }

    @Override
    public List<DataPoint> getDataPoints(int index) {
        ensureUpdateCurrencyHistory(index);
        List<DataPoint> points = new ArrayList<>();
        for (int i = 0; i < index; i++) {

            DataPoint point = new DataPoint();
            point.setCurrencyPool(this.currencyHistory.get(i));
            point.setHousePool(this.getCurrentHousePool(i));
            point.setTotalPool(this.getCurrentPool(i));
            point.setDate(this.getStartDate().plusMonths(i));

            points.add(point);
        }

        return points;
    }

}
