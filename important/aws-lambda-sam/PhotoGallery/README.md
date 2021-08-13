# S3 image resizer (dotnet)

# Requirements
- [The AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html) v1.17 or newer.
- aws tookit

project_dir='D:\my\epam\photo-board\lambda-functions\PhotoGallery\src\PhotoGallery' 

sourceBinaryBucket='photo-resize-source-bucket'
sourceBinaryBucketFolder='photo-resize-package'

stackName='photo-resize-stack-dotnet'
bucketName='photo-gallery-bucket-dotnet'
tableName='Images-dotnet'
functionName='photo-process-lambda-dotnet'
roleName=lambda_role_s3_dynamo_full_access_dotnet

export AWS_DEFAULT_REGION=ap-east-1

# Combined instructions
### publish lambda project
    dotnet publish "$project_dir\PhotoGallery.csproj" --output "$project_dir\bin\Release\netcoreapp3.1\publish" --configuration "Release" --framework "netcoreapp3.1" --runtime linux-x64 --self-contained false
### zip published artifacts
	rm "$project_dir\bin\Release\netcoreapp3.1\PhotoGallery.zip"

	powershell Compress-Archive -Path "$project_dir\bin\Release\netcoreapp3.1\publish\*" -DestinationPath "$project_dir\bin\Release\netcoreapp3.1\PhotoGallery.zip"
### build and deploy lambda
	## prepare cloudformation stack templete
    aws cloudformation package --template-file template.yml --s3-bucket $sourceBinaryBucket --s3-prefix $sourceBinaryBucketFolder --output-template-file out.yml

	## deploy cloudformation stack
    aws cloudformation deploy --template-file out.yml --stack-name $stackName --capabilities CAPABILITY_NAMED_IAM --parameter-overrides bucketName=$bucketName tableName=$tableName functionName=$functionName roleName=$roleName

	## create source folder in bucket
    aws s3api put-object --bucket $bucketName --key original/

