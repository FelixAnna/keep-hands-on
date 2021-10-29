package greetings

import (
	"errors"
	"fmt"
	"math/rand"
	"rsc.io/quote"
	"time"
)

//function name with Captical leter are known as exported function, can be called by func in other package
func Hello(name string) (string, error) {
	if name == "" {
		return "", errors.New("name cannot be empty")
	}
	message := fmt.Sprintf(getRandFormat(), name, quote.Go())

	return message, nil
}

func HelloAll(names []string) (map[string]string, error) {
	results := make(map[string]string, len(names))

	for _, name := range names {
		message, err := Hello(name)
		if err != nil {
			return nil, err
		}

		results[name] = message
	}

	return results, nil
}

//Go executes init functions automatically at program startup, after global variables have been initialized
func init() {
	rand.Seed(time.Now().UnixNano())
}

func getRandFormat() string {

	formats := []string{
		"Hi, %v. Welcome! %v",
		"Great to see you, %v! %v",
		"Hail, %v! Well met! %v",
	}

	return formats[rand.Intn(len(formats))]
}
