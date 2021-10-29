package main

import (
	"fmt"
	"github.com/FelixAnna/greetings"
	"log"
)

func main() {
	log.SetPrefix("greetings:")
	log.SetFlags(0)

	message, err := greetings.Hello("felix")

	if err != nil {
		//Fatal is equivalent to Print() followed by a call to os.Exit(1).
		log.Fatal(err)
	}

	fmt.Println(message)

	// A slice of names.
	names := []string{"Gladys", "Samantha", "Darrin"}

	// Request greeting messages for the names.
	messages, err := greetings.HelloAll(names)
	if err != nil {
		log.Fatal(err)
	}
	// If no error was returned, print the returned map of
	// messages to the console.
	fmt.Println(messages)
}
