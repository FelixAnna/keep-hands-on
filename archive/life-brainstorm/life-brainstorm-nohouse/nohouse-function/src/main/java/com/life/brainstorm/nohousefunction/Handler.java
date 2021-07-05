package com.life.brainstorm.nohousefunction;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.life.brainstorm.nohousefunction.entity.DecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionResponse;
import com.life.brainstorm.nohousefunction.entity.HouseDecisionRequest;
import com.life.brainstorm.nohousefunction.service.DecisionService;
import com.life.brainstorm.nohousefunction.service.impl.CurrencyDecisionService;
import com.life.brainstorm.nohousefunction.service.impl.HouseDecisionService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import software.amazon.awssdk.services.lambda.LambdaAsyncClient;
import software.amazon.awssdk.services.lambda.model.GetAccountSettingsRequest;
import software.amazon.awssdk.services.lambda.model.GetAccountSettingsResponse;

import java.util.concurrent.CompletableFuture;

// Handler value: example.Handler
public class Handler implements RequestHandler<DecisionRequest, String> {
    private static final Logger logger = LoggerFactory.getLogger(Handler.class);
    private static final Gson gson = new GsonBuilder().setPrettyPrinting().create();
    private static final LambdaAsyncClient lambdaClient = LambdaAsyncClient.create();

    public Handler() {
        CompletableFuture<GetAccountSettingsResponse> accountSettings = lambdaClient.getAccountSettings(GetAccountSettingsRequest.builder().build());
        try {
            GetAccountSettingsResponse settings = accountSettings.get();
        } catch (Exception e) {
            e.getStackTrace();
        }
    }

    @Override
    public String handleRequest(DecisionRequest request, Context context) {
        String response = new String();
        // call Lambda API
        logger.info("Getting account settings");
        CompletableFuture<GetAccountSettingsResponse> accountSettings =
                lambdaClient.getAccountSettings(GetAccountSettingsRequest.builder().build());
        // log execution details
        logger.info("ENVIRONMENT VARIABLES: {}", gson.toJson(System.getenv()));
        logger.info("CONTEXT: {}", gson.toJson(context));
        logger.info("EVENT: {}", gson.toJson(request));
        // process event
        DecisionService service = (request instanceof HouseDecisionRequest) ? new HouseDecisionService() : new CurrencyDecisionService();
        DecisionResponse responseEntity = service.getResponse(request, 245);

        // process Lambda API response
        try {
            GetAccountSettingsResponse settings = accountSettings.get();
            responseEntity.setDataPoints(responseEntity.getDataPoints());
            response = gson.toJson(responseEntity);
            logger.info("Account usage: {}", response);
        } catch (Exception e) {
            e.getStackTrace();
        }
        return response;
    }
}