package auth

import (
	"fmt"
	"io/ioutil"
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

func GetUserGitHub(c *gin.Context) {
	token := c.Query("access_token")
	url := "https://api.github.com/user"

	request, err := http.NewRequest(http.MethodGet, url, nil)
	request.Header.Add("Authorization", fmt.Sprintf("token %v", token))
	response, err := http.DefaultClient.Do(request)

	if err != nil {
		log.Fatal(err.Error())
		c.String(http.StatusInternalServerError, err.Error())
	}

	responseData, err := ioutil.ReadAll(response.Body)
	if err != nil {
		log.Fatal(err)
		c.String(http.StatusInternalServerError, err.Error())
	}

	c.String(http.StatusOK, string(responseData))
	/*var responseObject Response
	json.Unmarshal(responseData, &responseObject)

	fmt.Println(responseObject.Name)
	fmt.Println(len(responseObject.Pokemon))

	c.Redirect(http.StatusTemporaryRedirect, url)*/
}
