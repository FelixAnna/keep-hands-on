package com.felix.grpc.product.model;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ProductCategory {
	private int id;
	private String name;
}
