package com.felix.grpc.product.model;

import java.time.Instant;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class ProductPrice {
	private int id;
	private double price;
	private Instant startTime;
}
