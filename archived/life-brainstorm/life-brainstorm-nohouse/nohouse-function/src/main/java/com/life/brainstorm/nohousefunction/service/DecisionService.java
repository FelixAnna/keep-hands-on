package com.life.brainstorm.nohousefunction.service;

import com.life.brainstorm.nohousefunction.entity.DecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionResponse;

public interface DecisionService {
    DecisionResponse getResponse(DecisionRequest request, int range);
}
