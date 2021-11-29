package repository

import (
	"errors"
	"fmt"
	"log"
	"math/rand"
	"strconv"
	"time"

	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/service/dynamodb"
	"github.com/aws/aws-sdk-go/service/dynamodb/dynamodbattribute"
	"github.com/aws/aws-sdk-go/service/dynamodb/expression"

	"github.com/web-service-dlw/user-api/users/entity"
)

type UserRepo struct {
	TableName string
	DynamoDB  *dynamodb.DynamoDB
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

func (u *UserRepo) GetAllUsers() ([]entity.User, error) {
	filt := expression.Name("Email").AttributeExists()
	projection := expression.NamesList(expression.Name("Id"), expression.Name("Name"), expression.Name("Email"), expression.Name("Phone"), expression.Name("Birthday"))

	expr, err := expression.NewBuilder().WithFilter(filt).WithProjection(projection).Build()
	if err != nil {
		log.Fatalf("Got error building expression: %s", err)
		return nil, err
	}

	// Build the query input parameters
	params := &dynamodb.ScanInput{
		ExpressionAttributeNames:  expr.Names(),
		ExpressionAttributeValues: expr.Values(),
		FilterExpression:          expr.Filter(),
		ProjectionExpression:      expr.Projection(),
		TableName:                 aws.String(u.TableName),
	}

	result, err := u.DynamoDB.Scan(params)
	if err != nil {
		log.Fatalf("Query API call failed: %s", err)
		return nil, err
	}

	var users []entity.User = make([]entity.User, 0)
	for _, item := range result.Items {
		user := entity.User{}

		err = dynamodbattribute.UnmarshalMap(item, &user)

		if err != nil {
			log.Fatalf("Got error unmarshalling: %s", err)
			return nil, err
		}

		users = append(users, user)
	}

	return users, nil
}

func (u *UserRepo) GetUserByEmail(email string) (*entity.User, error) {
	result, err := u.DynamoDB.Query(&dynamodb.QueryInput{
		TableName:              aws.String(u.TableName),
		IndexName:              aws.String("Email-index"),
		KeyConditionExpression: aws.String("Email = :email"),
		ExpressionAttributeValues: map[string]*dynamodb.AttributeValue{
			":email": {S: &email},
		},
		Limit: aws.Int64(1),
	})

	if err != nil {
		log.Fatalf("Query API call failed: %s", err)
		return nil, err
	}

	if length := len(result.Items); length > 0 {
		user := entity.User{}

		err = dynamodbattribute.UnmarshalMap(result.Items[0], &user)

		if err != nil {
			log.Fatalf("Got error unmarshalling: %s", err)
			return nil, err
		}

		return &user, nil

	} else {
		return nil, err
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
