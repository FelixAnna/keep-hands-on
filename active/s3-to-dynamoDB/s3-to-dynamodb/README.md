# S3 image resizer (Java)

![Architecture](/sample-apps/s3-java/images/sample-s3-java.png)

The project source includes function code and supporting resources:

- `src/main` - A Java function.
- `src/test` - A unit test and helper classes.
- `template.yml` - An AWS CloudFormation template that creates an application.
- `build.gradle` - A Gradle build file.
- `pom.xml` - A Maven build file.
- `1-create-bucket.sh`, `2-deploy.sh`, etc. - Shell scripts that use the AWS CLI to deploy and manage the application.

Use the following instructions to deploy the sample application.

# Requirements
- [Java 8 runtime environment (SE JRE)](https://www.oracle.com/java/technologies/javase-downloads.html)
- [Gradle 5](https://gradle.org/releases/) or [Maven 3](https://maven.apache.org/docs/history.html)
- The Bash shell. For Linux and macOS, this is included by default. In Windows 10, you can install the [Windows Subsystem for Linux](https://docs.microsoft.com/en-us/windows/wsl/install-win10) to get a Windows-integrated version of Ubuntu and Bash.
- [The AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html) v1.17 or newer.

If you use the AWS CLI v2, add the following to your [configuration file](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html) (`~/.aws/config`):

```
cli_binary_format=raw-in-base64-out
```

This setting enables the AWS CLI v2 to load JSON events from a file, matching the v1 behavior.

# Combined instructions
### From build to deploy
    sh ./2-build-layer.sh

    mvn package

    aws cloudformation package --template-file template-mvn.yml --s3-bucket boconfigbucket --s3-prefix photo-resize-package --output-template-file out.yml

    aws cloudformation deploy --template-file out.yml --stack-name photo-resize-stack-java --capabilities CAPABILITY_NAMED_IAM --parameter-overrides bucketName=photo-board-test-java tableName=photos-java functionName=photo-process-lambda-java roleName=lambda_role_s3_dynamo_full_access_java

    aws s3api put-object --bucket photo-board-test-java --key original/

    manually only at first deployment: open the s3 created, choose Properties -> Event notifications -> The only event -> Edit -> Save (change nothing) to make sure aws console grant lambda required permissions to access s3 event.

# Original instructions
https://github.com/awsdocs/aws-lambda-developer-guide/tree/main/sample-apps/s3-java

