package com.felix.grpc.product;


import org.lognet.springboot.grpc.GRpcService;

import com.felix.grpc.product_proto.Category;
import com.felix.grpc.product_proto.GetProductListRequest;
import com.felix.grpc.product_proto.GetProductRequest;
import com.felix.grpc.product_proto.ProductListResponse;
import com.felix.grpc.product_proto.ProductResponse;
import com.felix.grpc.product_proto.ProductServiceGrpc.ProductServiceImplBase;

import io.grpc.stub.StreamObserver;

@GRpcService
public class ProductServiceImpl extends ProductServiceImplBase{

	@Override
	public void getProduct(GetProductRequest request, StreamObserver<ProductResponse> responseObserver) {
		
		ProductResponse reply = ProductResponse.newBuilder()
				.setId(1)
				.setName("test Product from st")
				.setDescription("description")
				.setCategory(Category.newBuilder().setId(1).setName("category").build())
                .build();
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
	}
	
	@Override
	public void getProductList(GetProductListRequest request, StreamObserver<ProductListResponse> responseObserver) {

		ProductResponse reply = ProductResponse.newBuilder()
				.setId(1)
				.setName("test Product from st")
				.setDescription("description")
				.setCategory(Category.newBuilder().setId(1).setName("category").build())
                .build();
		ProductListResponse response = ProductListResponse.newBuilder().addProducts(reply).setCount(1).build();
        responseObserver.onNext(response);
        responseObserver.onCompleted();
	}
}
