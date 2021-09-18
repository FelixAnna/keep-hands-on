package product;

import com.felix.grpc.st_proto.GetProductRequest;
import com.felix.grpc.st_proto.ProductResponse;
import com.felix.grpc.st_proto.ProductServiceGrpc;

import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;

public class GrpcClient {

	public static void main(String[] args) {
		GetProductRequest request = GetProductRequest.newBuilder()
                .setId(1)
                .build();
    	
    	ManagedChannel channel = ManagedChannelBuilder.forAddress("localhost", 9090)
    	        .usePlaintext()
    	        .build();

    	ProductServiceGrpc.ProductServiceBlockingStub stub =
    			ProductServiceGrpc.newBlockingStub(channel);

    	    ProductResponse reply = stub.getProduct(request);

    	    System.out.println(reply);

    	    channel.shutdown();

	}

}
