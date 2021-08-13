package com.felix.grpc.product.model;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class Product {
	private int id;
	private String name;
	private String description;
    
	private ProductPrice price;
	private ProductCategory  category;
}
