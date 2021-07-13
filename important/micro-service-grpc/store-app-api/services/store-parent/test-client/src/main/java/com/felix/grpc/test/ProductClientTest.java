package com.felix.grpc.test;

import static java.lang.System.out;

import com.felix.grpc.product_proto.GetProductListRequest;
import com.felix.grpc.product_proto.GetProductRequest;
import com.felix.grpc.product_proto.ProductListResponse;
import com.felix.grpc.product_proto.ProductResponse;
import com.felix.grpc.product_proto.ProductServiceGrpc;

import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;

public class ProductClientTest {
	public void testGetOneProduct() {
		GetProductRequest request = GetProductRequest.newBuilder()
                .setId(1)
                .build();
    	
    	ManagedChannel channel = ManagedChannelBuilder.forAddress("localhost", 5000)
    	        .usePlaintext()
    	        .build();

    	ProductServiceGrpc.ProductServiceBlockingStub stub =
    			ProductServiceGrpc.newBlockingStub(channel);

    	    ProductResponse reply = stub.getProduct(request);

    	    out.println(reply);

    	    channel.shutdown();
	}
	
	public void testGetManyProduct() {
		GetProductListRequest request = GetProductListRequest.newBuilder()
                .setKeywords("java")
                .setPage(1)
                .setRows(10)
                .build();
    	
    	ManagedChannel channel = ManagedChannelBuilder.forAddress("localhost", 5000)
    	        .usePlaintext()
    	        .build();

    	ProductServiceGrpc.ProductServiceBlockingStub stub =
    			ProductServiceGrpc.newBlockingStub(channel);

    	    ProductListResponse reply = stub.getProductList(request);

    	    out.println(reply);

    	    channel.shutdown();
	}
}
