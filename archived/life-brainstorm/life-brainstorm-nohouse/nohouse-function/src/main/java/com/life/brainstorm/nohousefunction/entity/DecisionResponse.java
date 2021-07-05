package com.life.brainstorm.nohousefunction.entity;

import lombok.Data;

import java.util.List;

@Data
public class DecisionResponse {
    private List<DataPoint> dataPoints;
}
