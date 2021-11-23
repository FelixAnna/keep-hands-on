package auth

import (
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/aws"
	"golang.org/x/oauth2"
)

var GitHub oauth2.Endpoint
var confGitHub *oauth2.Config

func init() {
	GitHub = oauth2.Endpoint{
		AuthURL:  "https://github.com/login/oauth/authorize",
		TokenURL: "https://github.com/login/oauth/access_token",
	}

	confGitHub = &oauth2.Config{
		ClientID:     aws.Parameters["/dlf/dev/githubClientId"],
		ClientSecret: aws.Parameters["/dlf/dev/githubClientSecret"],
		Scopes:       []string{"User", "Admin", "Customer"},
		Endpoint:     GitHub,
	}
}

func AuthorizeGithub(c *gin.Context) {
	//ctx := context.Background()

	url := confGitHub.AuthCodeURL("state", oauth2.AccessTypeOffline)

	c.Redirect(http.StatusTemporaryRedirect, url)
}

func GetTokenGithub(c *gin.Context) {
	code := c.Query("code")
	if code == "" {
		c.JSON(http.StatusUnauthorized, "Token not found.")
	}

	tok, err := confGitHub.Exchange(c.Request.Context(), code)
	if err != nil {
		log.Fatal(err)
	}

	c.JSON(http.StatusOK, tok)
}
