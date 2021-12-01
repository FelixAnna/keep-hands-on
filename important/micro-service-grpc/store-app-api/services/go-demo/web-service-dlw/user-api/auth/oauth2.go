package auth

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/web-service-dlw/user-api/aws"
	"golang.org/x/oauth2"
	"golang.org/x/oauth2/github"
)

type GitHubUser struct {
	Email     string `json:"email"`
	Login     string `json:"login"`
	Id        int    `json:"id"`
	AvatarUrl string `json:"avatar_url"`
}

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

func AuthorizeGithubUrl(c *gin.Context) {
	//ctx := context.Background()
	//generate state and return to client can stop CSRF
	url := confGitHub.AuthCodeURL("state123", oauth2.AccessTypeOffline)

	c.String(http.StatusOK, url)
}

func GetTokenGithub(c *gin.Context) {
	code := c.Query("code")
	state := c.Query("state")

	if code == "" {
		c.JSON(http.StatusUnauthorized, "Token not found.")
	}

	if state != "state123" {
		c.JSON(http.StatusBadGateway, "Invalid state.")
	}

	//TODO: how to verify dynamic csrf token
	tok, err := confGitHub.Exchange(c.Request.Context(), code)
	if err != nil {
		log.Println(err)
	}

	c.JSON(http.StatusOK, tok)
}

func GetUserGitHub(c *gin.Context) {
	token := c.Query("access_token")
	url := "https://api.github.com/user"

	request, err := http.NewRequest(http.MethodGet, url, nil)
	if err != nil {
		log.Fatal(err.Error())
		c.String(http.StatusBadRequest, err.Error())
	}

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

	var user *GitHubUser = &GitHubUser{}
	json.Unmarshal(responseData, &user)

	c.JSON(http.StatusOK, user)
}
