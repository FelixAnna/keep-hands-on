package repository

import (
	"errors"
	"fmt"
	"log"
	"math/rand"
	"strconv"
	"time"

	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/aws/credentials"
	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/dynamodb"
	"github.com/aws/aws-sdk-go/service/dynamodb/dynamodbattribute"

	"github.com/web-service-dlw/user-api/users/entity"
)

type UserRepo struct {
	TableName string
	DynamoDB  *dynamodb.DynamoDB
}

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

func GetClient() *dynamodb.DynamoDB {
	// Initialize a session that the SDK will use to load
	// credentials from the shared credentials file ~/.aws/credentials
	// and region from the shared configuration file ~/.aws/config.
	/*sess := session.Must(session.NewSessionWithOptions(session.Options{
		SharedConfigState: session.SharedConfigEnable,
	}))*/

	sess, err := session.NewSession(&aws.Config{
		Region:      aws.String("ap-southeast-1"),
		Credentials: credentials.NewStaticCredentials("", "", ""),
	})

	if err != nil {
		log.Fatalf("Error when connecting to dynamodb: %v", err)
	}

	// Create DynamoDB client
	dynamoDB := dynamodb.New(sess)

	return dynamoDB
}

func (u *UserRepo) GetAllTables() {
	// Build the request with its input parameters
	resp, err := u.DynamoDB.ListTables(&dynamodb.ListTablesInput{
		Limit: aws.Int64(5),
	})
	if err != nil {
		log.Fatalf("failed to list tables, %v", err)
	}

	fmt.Println("Tables:")

	for _, tableName := range resp.TableNames {
		fmt.Println(tableName)
	}
}

func (u *UserRepo) GetUserById(userId string) (*entity.User, error) {
	result, err := u.DynamoDB.GetItem(&dynamodb.GetItemInput{
		TableName: aws.String(u.TableName),
		Key: map[string]*dynamodb.AttributeValue{
			"Id": {S: aws.String(userId)},
		},
	},
	)

	if err != nil {
		log.Fatalf("Got error calling GetItem: %s", err)
	}

	if result.Item == nil {
		msg := "Could not find user with Id: '" + userId + "'"
		return nil, errors.New(msg)
	}

	item := entity.User{}
	err = dynamodbattribute.UnmarshalMap(result.Item, &item)
	if err != nil {
		log.Fatalf("Failed to unmarshal Record, %v", err)
		return nil, err
	}

	return &item, nil
}

func (u *UserRepo) CreateUser(user *entity.User) (*string, error) {
	randId := fmt.Sprintf("%d%03d", time.Now().Unix(), rand.Intn(1000))
	user.Id = randId
	user.CreateTime = strconv.FormatInt(time.Now().UTC().Unix(), 10)

	userJson, err := dynamodbattribute.MarshalMap(user)
	if err != nil {
		log.Fatalf("Got error marshalling new User item: %s", err)
	}

	_, err = u.DynamoDB.PutItem(&dynamodb.PutItemInput{
		TableName: aws.String(u.TableName),
		Item:      userJson,
	})

	if err != nil {
		log.Fatalf("Got error calling PutItem: %s", err)
		return nil, err
	}

	return &user.Id, nil
}

func (u *UserRepo) UpdateUserBirthday(userId, birthday string) error {
	input := &dynamodb.UpdateItemInput{
		ExpressionAttributeValues: map[string]*dynamodb.AttributeValue{
			":birthday": {S: aws.String(birthday)},
		},
		TableName: aws.String(u.TableName),
		Key: map[string]*dynamodb.AttributeValue{
			"Id": {S: aws.String(userId)},
		},
		ReturnValues:     aws.String("ALL_NEW"),
		UpdateExpression: aws.String("set Birthday = :birthday"),
	}

	_, err := u.DynamoDB.UpdateItem(input)
	if err != nil {
		log.Fatalf("Got error calling UpdateItem: %s", err)
		return err
	}

	return nil
}

func (u *UserRepo) UpdateUserAddress(userId string, addresses []entity.Address) error {

	addressJson, err := dynamodbattribute.MarshalList(addresses)
	if err != nil {
		log.Fatalf("Got error calling UpdateItem: %s", err)
		return err
	}

	fmt.Println(addressJson)
	input := &dynamodb.UpdateItemInput{
		ExpressionAttributeValues: map[string]*dynamodb.AttributeValue{
			":addresses": {L: addressJson},
		},
		TableName: aws.String(u.TableName),
		Key: map[string]*dynamodb.AttributeValue{
			"Id": {S: aws.String(userId)},
		},
		ReturnValues:     aws.String("ALL_NEW"),
		UpdateExpression: aws.String("set Address = :addresses"),
	}

	_, err = u.DynamoDB.UpdateItem(input)
	if err != nil {
		log.Fatalf("Got error calling UpdateItem: %s", err)
		return err
	}

	return nil
}

func (u *UserRepo) DeleteUser(userId string) error {
	_, err := u.DynamoDB.DeleteItem(&dynamodb.DeleteItemInput{
		TableName: aws.String(u.TableName),
		Key: map[string]*dynamodb.AttributeValue{
			"Id": {S: aws.String(userId)},
		},
	})

	if err != nil {
		log.Fatalf("Got error calling UpdateItem: %s", err)
		return err
	}

	return nil
}
