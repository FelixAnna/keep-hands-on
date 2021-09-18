package product;

import com.felix.grpc.st_proto.Category;
import com.felix.grpc.st_proto.GetProductRequest;
import com.felix.grpc.st_proto.ProductResponse;
import com.felix.grpc.st_proto.ProductServiceGrpc.ProductServiceImplBase;

import io.grpc.stub.StreamObserver;
import net.devh.boot.grpc.server.service.GrpcService;

@GrpcService
public class ProductServiceImpl extends ProductServiceImplBase {

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
}
