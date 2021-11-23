package aws

import (
	"log"

	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/dynamodb"
	"github.com/aws/aws-sdk-go/service/ssm"
)

/*
func GetClientByConfig() *dynamodb.Client {
	// Using the SDK's default configuration, loading additional config
	// and credentials values from the environment variables, shared
	// credentials, and shared configuration files
	cfg, err := config.LoadDefaultConfig(context.TODO(), config.WithRegion("ap-southeast-1"))
	if err != nil {
		log.Fatalf("unable to load SDK config, %v", err)
	}

	// Using the Config value, create the DynamoDB client
	client := dynamodb.NewFromConfig(cfg)

	return client
}*/

var sess = session.Must(session.NewSessionWithOptions(session.Options{
	SharedConfigState: session.SharedConfigEnable,
}))

var Parameters map[string]string

func init() {
	ssmClient := ssm.New(sess)

	path := "/dlf/dev"
	withDecryption := true
	out, err := ssmClient.GetParametersByPath(&ssm.GetParametersByPathInput{
		Path:           &path,
		WithDecryption: &withDecryption,
	})

	if err != nil {
		log.Fatalf("Error when geting ssm parameters: %v", err)
		panic(err)
	}

	Parameters = make(map[string]string, len(out.Parameters))
	for _, parameter := range out.Parameters {
		Parameters[*parameter.Name] = *parameter.Value
	}
}

func GetDynamoDBClient() *dynamodb.DynamoDB {
	// Initialize a session that the SDK will use to load
	// credentials from the shared credentials file ~/.aws/credentials
	// and region from the shared configuration file ~/.aws/config.

	//set AWS_REGION=ap-southeast-1
	/*sess, err := session.NewSession(&aws.Config{
		Region: aws.String("ap-southeast-1")},
	)

	sess, err := session.NewSession(&aws.Config{
		Region:      aws.String("ap-southeast-1"),
		Credentials: credentials.NewStaticCredentials(key, value, ""),
	})

	if err != nil {
		log.Fatalf("Error when connecting to dynamodb: %v", err)
	}*/

	// Create DynamoDB client
	dynamoDB := dynamodb.New(sess)

	return dynamoDB
}
