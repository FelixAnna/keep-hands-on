# Business Requirement
	User Can Register, Login to app
	User Can Search Products
	User Can Add product to order (create a new one, or add to existing active order)
	User can change product count in order
	User can update address for a active order
	User submit order, then the order is locked
		Inventory reduce product count when submit order
		If not enough inventory, then do not let user submit order
		(distributed transaction)
	User can search order history

	Admin can create product
	Admin (inventory manager) can update inventory

# MicroService:
	  AuthService： language C#/java/any, integrate with aws cognito user pool
	  ProductService: language C#/java/any
	  OrderService： language C#/java/any, dependency InventoryService
	  InventoryService： language C#/java/any
  
## AuthService
	Reg
	Login
	Logout
	UserInfo
	
## ProductService
	Product
		AggregateRoot - Prodct
			Request
				CreateProductRequest
				GetOneProductRequest
				SearchProductRequest
			ProductController
			ProductApplicationService
				GetProductList(SearchProductRequest)
				GetProduct(GetOneProductRequest)
				CreateProduct(CreateProductRequest)
			Product  --model
			ProductRepository
			Exception
				ProductNotFoundException
			Response
				ProductResponse
				ProductCollectionResponse
			Factory
				CreateProduct()
			
		
		DomainService
	
## OrderService
	Order
		Request
			CreateOrderRequest
			UpdateOrderRequest
			SearchOrderRequest
			SubmitOrderRequest
		OrderController
		OrderApplicationService
		OrderRepository
		model
			Order
			OrderItem
			Address
		Exception
			OrderNotFoundException
			OrderReadonlyException
		Response
			OrderResponse
			OrderCllectionResponse
		Factory
			OrderFactory
		
	DomainService
		InventoryService
		
		
## InventoryService
	Inventory
		CreateInventoryRequest
		UpdateInventoryRequest
		SearchInventoryRequest
		GetInventoryRequest
	
	InventoryController
	InventoryApplicationService
	InventoryRepository
	model
		Inventory
		Product
	Exception
		InventoryNotFoundException
	Response
		InventoryResponse
		InventoryCollectionResponse
	Factory
		InventoryFactory
		
	
