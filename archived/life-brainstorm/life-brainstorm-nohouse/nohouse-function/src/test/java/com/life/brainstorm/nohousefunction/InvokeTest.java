package com.life.brainstorm.nohousefunction;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.xray.AWSXRay;
import com.amazonaws.xray.AWSXRayRecorderBuilder;
import com.amazonaws.xray.strategy.sampling.NoSamplingStrategy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.life.brainstorm.nohousefunction.entity.CurrencyDecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionRequest;
import com.life.brainstorm.nohousefunction.entity.DecisionResponse;
import com.life.brainstorm.nohousefunction.entity.HouseDecisionRequest;
import com.life.brainstorm.nohousefunction.service.impl.CurrencyDecisionService;
import com.life.brainstorm.nohousefunction.service.impl.HouseDecisionService;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.assertTrue;

class InvokeTest {
    private static final Logger logger = LoggerFactory.getLogger(com.life.brainstorm.nohousefunction.InvokeTest.class);
    Gson gson = new GsonBuilder()
            .setPrettyPrinting()
            .create();

    public InvokeTest() {
        AWSXRayRecorderBuilder builder = AWSXRayRecorderBuilder.standard();
        builder.withSamplingStrategy(new NoSamplingStrategy());
        AWSXRay.setGlobalRecorder(builder.build());
    }

    @Test
    void invokeAllTest() {
        String path = "src/test/resources/request.json";
        String requestString = loadJsonFile(path);
        DecisionRequest request = gson.fromJson(requestString, CurrencyDecisionRequest.class);
        DecisionResponse currencyResponse = new CurrencyDecisionService().getResponse(request, 245);

        String path2 = "src/test/resources/requestSZ.json";
        String requestString2 = loadJsonFile(path2);
        DecisionRequest request2 = gson.fromJson(requestString2, HouseDecisionRequest.class);
        DecisionResponse houseResponse = new HouseDecisionService().getResponse(request2, 245);

        assertTrue(houseResponse!=null);
    }

    @Test
    void invokeTest() {
        AWSXRay.beginSegment("nohouse-test");
        String path = "src/test/resources/request.json";
        String requestString = loadJsonFile(path);
        DecisionRequest request = gson.fromJson(requestString, CurrencyDecisionRequest.class);
        Context context = new TestContext();
        String requestId = context.getAwsRequestId();
        Handler handler = new Handler();
        String result = handler.handleRequest(request, context);
        assertTrue(result.contains("dataPoints"));
        AWSXRay.endSegment();
    }

    @Test
    void invokeSZTest() {
        AWSXRay.beginSegment("nohouse-test");
        String path = "src/test/resources/requestSZ.json";
        String requestString = loadJsonFile(path);
        DecisionRequest request = gson.fromJson(requestString, HouseDecisionRequest.class);
        Context context = new TestContext();
        String requestId = context.getAwsRequestId();
        Handler handler = new Handler();
        String result = handler.handleRequest(request, context);
        assertTrue(result.contains("dataPoints"));
        AWSXRay.endSegment();
    }

    private static String loadJsonFile(String path) {
        StringBuilder stringBuilder = new StringBuilder();
        try (Stream<String> stream = Files.lines(Paths.get(path), StandardCharsets.UTF_8)) {
            stream.forEach(s -> stringBuilder.append(s));
        } catch (IOException e) {
            e.printStackTrace();
        }
        return stringBuilder.toString();
    }
}
