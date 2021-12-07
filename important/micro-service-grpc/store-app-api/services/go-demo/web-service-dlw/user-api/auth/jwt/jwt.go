package jwt

import (
	"log"
	"time"

	"github.com/golang-jwt/jwt"
)

type MyCustomClaims struct {
	UserId string `json:"userId"`
	Email  string `json:"email"`
	jwt.StandardClaims
}

type MyToken struct {
	Token string `json:"token"`
}

var mySigningKey = []byte("AllYourBase")

const myIssuer = "dlw"
const myExpireAt = 15000

func NewToken(id, email string) (*MyToken, error) {
	// Create the Claims
	claims := MyCustomClaims{
		id,
		email,
		jwt.StandardClaims{
			ExpiresAt: time.Now().Unix() + myExpireAt,
			Issuer:    myIssuer,
		},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	ss, err := token.SignedString(mySigningKey)

	return &MyToken{Token: ss}, err
}

func ParseToken(tokenString string) (*MyCustomClaims, error) {
	//tokenString = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJmb28iOiJiYXIiLCJleHAiOjE1MDAwLCJpc3MiOiJ0ZXN0In0.HE7fK0xOQwFEr4WDgRWj4teRPZ6i3GLwD5YCm6Pwu_c"

	token, err := jwt.ParseWithClaims(tokenString, &MyCustomClaims{}, func(token *jwt.Token) (interface{}, error) {
		return mySigningKey, nil
	})

	if claims, ok := token.Claims.(*MyCustomClaims); ok && token.Valid {
		log.Println("Valid")
		return claims, nil
	} else {
		log.Println("invalid")
		return nil, err
	}
}
