# S3 image resizer (nodejs)

# Requirements
- [The AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html) v1.17 or newer.

# Combined instructions
### build layers
    1. create folder layers/sharp/nodejs
    2. run: npm init -y
    3. run: npm install --arch=x64 --platform=linux sharp
    4. zip nodejs folder and add layer to lambda
    5. keep the layer arn for use later

### build and deploy lambda
    manualy: zip index.js file and copy to build/s3-to-dynamodb-node.zip

    aws cloudformation package --template-file template.yml --s3-bucket boconfigbucket --s3-prefix photo-resize-package --output-template-file out.yml

    aws cloudformation deploy --template-file out.yml --stack-name photo-resize-stack-node --capabilities CAPABILITY_NAMED_IAM --parameter-overrides bucketName=photo-board-test-node tableName=photos-node functionName=photo-process-lambda-node roleName=lambda_role_s3_dynamo_full_access_node layerVersion=arn:aws:lambda:ap-southeast-1:365358585348:layer:sharp-layer:2

    aws s3api put-object --bucket photo-board-test-node --key original/

    manually only at first deployment: open the s3 created, choose Properties -> Event notifications -> The only event -> Edit -> Save (change nothing) to make sure aws console grant lambda required permissions to access s3 event.

