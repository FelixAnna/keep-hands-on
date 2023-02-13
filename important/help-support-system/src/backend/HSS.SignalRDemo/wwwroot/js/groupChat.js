"use strict";
const options = {
    accessTokenFactory: getToken,
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
};
const connection = new signalR.HubConnectionBuilder()
                            .withUrl("https://localhost:7133/chat", options)
                            //.withUrl("https://api-prod-hss.metadlw.com/hub/chat", options)
                            .configureLogging(signalR.LogLevel.Information)
                            .withAutomaticReconnect()
                            .build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;


connection.on("ReceiveGroupMessage", function (sender, toGroup, message) {
    if (toGroup != document.getElementById("groupId").value) {
        return;
    }

    const li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${sender}: ${message}`;
});

//auto reconnect
connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);

    document.getElementById("messageInput").disabled = true;

    const li = document.createElement("li");
    li.textContent = `Connection lost due to error "${error}". Reconnecting.`;
    document.getElementById("messageList").appendChild(li);
});

connection.onreconnected(connectionId => {
    console.assert(connection.state === signalR.HubConnectionState.Connected);

    document.getElementById("messageInput").disabled = false;

    const li = document.createElement("li");
    li.textContent = `Connection reestablished. Connected with connectionId "${connectionId}".`;
    document.getElementById("messageList").appendChild(li);
});

connection.onclose(error => {
    console.assert(connection.state === signalR.HubConnectionState.Disconnected);

    document.getElementById("messageInput").disabled = true;

    const li = document.createElement("li");
    li.textContent = `Connection closed due to error "${error}". Try refreshing this page to restart the connection.`;
    document.getElementById("messageList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    const groupId = document.getElementById("groupId").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendToGroup", groupId, message).catch(function (err) {
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
