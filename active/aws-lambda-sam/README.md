# functionality
	listen to a s3 directory, when any new jpg/png images uploaded, call a lambda function to resize that, save resized image to another s3 dynamo db, save both images info into dynamodb
	
# build & deployment
	use serverless application mode with cloudformation templete to deploy lambda

# version
	s3-to-dynamodb: java version
	s3-to-dynamodb-node: node version