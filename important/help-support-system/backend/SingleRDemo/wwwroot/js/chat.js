"use strict";

const options = {
    accessTokenFactory: getToken,
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
};

let connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7133/chat", options /*, options => {
    options.UseDefaultCredentials = true;
}*/).configureLogging(signalR.LogLevel.Information).withAutomaticReconnect().build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// broadcast way
//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let user = document.getElementById("userInput").value;
    let message = document.getElementById("messageInput").value;
    connection.invoke("SendToUser", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function getToken() {
    const xhr = new XMLHttpRequest();
    return new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {
            if (this.readyState !== 4) return;
            if (this.status == 200) {
                resolve(this.responseText);
            } else {
                reject(this.statusText);
            }
        };
        xhr.open("GET", "/api/Token/get");
        xhr.send();
    });
}