package auth

import (
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/aws"
	"golang.org/x/oauth2"
	"golang.org/x/oauth2/github"
)

var confGitHub *oauth2.Config

func init() {
	confGitHub = &oauth2.Config{
		ClientID:     aws.Parameters["/dlf/dev/githubClientId"],
		ClientSecret: aws.Parameters["/dlf/dev/githubClientSecret"],
		Scopes:       []string{"read:user", "user:email", "read:repo_hook"},
		Endpoint:     github.Endpoint,
	}
}

func AuthorizeGithub(c *gin.Context) {
	//ctx := context.Background()
	//generate state and return to client can stop CSRF
	url := confGitHub.AuthCodeURL("state123", oauth2.AccessTypeOffline)

	c.Redirect(http.StatusTemporaryRedirect, url)
}

func GetTokenGithub(c *gin.Context) {
	code := c.Query("code")
	if code == "" {
		c.JSON(http.StatusUnauthorized, "Token not found.")
	}

	//TODO: how to verify dynamic csrf token
	tok, err := confGitHub.Exchange(c.Request.Context(), code)
	if err != nil {
		log.Fatal(err)
	}

	c.JSON(http.StatusOK, tok)
}
