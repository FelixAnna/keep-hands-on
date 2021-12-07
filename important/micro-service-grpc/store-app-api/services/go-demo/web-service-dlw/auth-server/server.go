package main

import (
	"flag"
	"fmt"
	"log"
	"net/http"

	"github.com/felixanna/web-service-dlw/auth-server/auth"
)

var (
	portvar int
)

func init() {
	flag.IntVar(&portvar, "p", 9096, "the base port for the server")
}

func main() {
	flag.Parse()

	log.Println("Dumping requests")

	http.HandleFunc("/login", auth.LoginHandler)
	http.HandleFunc("/auth", auth.AuthHandler)

	http.HandleFunc("/oauth/authorize", auth.OAuthAuthorize)

	http.HandleFunc("/oauth/token", auth.OAuthToken)

	http.HandleFunc("/test", auth.Test)

	log.Printf("Server is running at %d port.\n", portvar)
	log.Printf("Point your OAuth client Auth endpoint to %s:%d%s", "http://localhost", portvar, "/oauth/authorize")
	log.Printf("Point your OAuth client Token endpoint to %s:%d%s", "http://localhost", portvar, "/oauth/token")
	log.Fatal(http.ListenAndServe(fmt.Sprintf(":%d", portvar), nil))
}
