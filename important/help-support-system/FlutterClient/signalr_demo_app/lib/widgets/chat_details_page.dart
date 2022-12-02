import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:signalr_demo_app/models/login_response.dart';
import 'package:signalr_demo_app/services/message_service.dart';
import '../models/chat_message.dart';
import '../services/chart_hub_service.dart';
import '../services/localstorage_service.dart';

class ChatDetailsPage extends StatefulWidget {
  String chatId;
  String name;
  ChatDetailsPage({required this.chatId, required this.name});

  @override
  _ChatDetailsPageState createState() => _ChatDetailsPageState();
}

class _ChatDetailsPageState extends State<ChatDetailsPage> {
  String message = "";
  List<ChatMessage> messages = [];
  late User profile;
  var currentChatId = '';
  final messageController = TextEditingController();
  final HubService hubService = Get.find<HubService>();
  final MessageService messageService = Get.find<MessageService>();
  @override
  void initState() {
    currentChatId = widget.chatId;
    loadProfile();
    super.initState();
    initSignalR();
  }

  loadProfile() async {
    var profileText =
        await LocalStorageService.get(LocalStorageService.PROFILE);

    profile = User.fromJson(jsonDecode(profileText));
  }

  @override
  Widget build(BuildContext context) {
    WidgetsBinding.instance.addPostFrameCallback((_) => _scrollToBottom());
    return Scaffold(
      appBar: AppBar(
        elevation: 0,
        automaticallyImplyLeading: false,
        backgroundColor: Colors.white,
        flexibleSpace: SafeArea(
          child: Container(
            padding: EdgeInsets.only(right: 16),
            child: Row(
              children: <Widget>[
                IconButton(
                  onPressed: () {
                    Get.back();
                  },
                  icon: Icon(
                    Icons.arrow_back,
                    color: Colors.black,
                  ),
                ),
                SizedBox(
                  width: 2,
                ),
                CircleAvatar(
                  backgroundImage: NetworkImage(
                      "https://cdn.pixabay.com/photo/2015/11/16/14/43/cat-1045782__340.jpg"),
                  maxRadius: 20,
                ),
                SizedBox(
                  width: 12,
                ),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      Text(
                        widget.name,
                        style: TextStyle(
                            fontSize: 16, fontWeight: FontWeight.w600),
                      ),
                      SizedBox(
                        height: 6,
                      ),
                      Text(
                        "Online",
                        style: TextStyle(
                            color: Colors.grey.shade600, fontSize: 13),
                      ),
                    ],
                  ),
                ),
                Icon(
                  Icons.settings,
                  color: Colors.black54,
                ),
              ],
            ),
          ),
        ),
      ),
      body: Stack(
        children: <Widget>[
          ListView.builder(
            itemCount: messages.length,
            shrinkWrap: true,
            padding: EdgeInsets.only(top: 10, bottom: 70),
            physics: AlwaysScrollableScrollPhysics(),
            controller: _scrollController,
            itemBuilder: (context, index) {
              return Container(
                padding:
                    EdgeInsets.only(left: 14, right: 14, top: 10, bottom: 10),
                child: Align(
                  alignment: (messages[index].messageType == "receiver"
                      ? Alignment.topLeft
                      : Alignment.topRight),
                  child: Container(
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(20),
                      color: (messages[index].messageType == "receiver"
                          ? Colors.grey.shade200
                          : Colors.blue[200]),
                    ),
                    padding: EdgeInsets.all(16),
                    child: Text(
                      messages[index].messageContent,
                      style: TextStyle(fontSize: 15),
                    ),
                  ),
                ),
              );
            },
          ),
          Align(
            alignment: Alignment.bottomLeft,
            child: Container(
              padding: EdgeInsets.only(left: 10, bottom: 10, top: 10),
              height: 60,
              width: double.infinity,
              color: Colors.white,
              child: Row(
                children: <Widget>[
                  GestureDetector(
                    onTap: () {},
                    child: Container(
                      height: 30,
                      width: 30,
                      decoration: BoxDecoration(
                        color: Colors.lightBlue,
                        borderRadius: BorderRadius.circular(30),
                      ),
                      child: Icon(
                        Icons.add,
                        color: Colors.white,
                        size: 20,
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 15,
                  ),
                  Expanded(
                    child: TextField(
                      controller: messageController,
                      decoration: InputDecoration(
                          hintText: "Write message...",
                          hintStyle: TextStyle(color: Colors.black54),
                          border: InputBorder.none),
                    ),
                  ),
                  SizedBox(
                    width: 15,
                  ),
                  FloatingActionButton(
                    onPressed: () {
                      _sendMessage();
                    },
                    child: Icon(
                      Icons.send,
                      color: Colors.white,
                      size: 18,
                    ),
                    backgroundColor: Colors.blue,
                    elevation: 0,
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  Future<void> _sendMessage() async {
    final msg = messageController.text;
    messageController.text = "";
    setState(() {
      messages.add(ChatMessage(
          sender: profile.UserName,
          messageContent: msg,
          messageType: "sender"));
    });
    await HubService.hubConnection.invoke("SendToUser",
        args: <Object>[currentChatId, msg]).then((value) => {});
  }

  void initSignalR() async {
    await hubService.initHubConnection(
        listeningMessage: _handleNewMessage, listenMethod: "ReceiveMessage");

    hubService.startConnect(
      callback: () => Get.toNamed("/login"),
    );
    final msgList =
        await messageService.getMessages(currentChatId, profile.UserId);
    setState(() {
      messages = msgList;
    });
  }

  void _handleNewMessage(List<Object?>? parameters) {
    setState(() {
      var fromUser = parameters?.elementAt(0).toString();
      var msg = parameters?.elementAt(1).toString();
      var sender = fromUser! == currentChatId ? widget.name : profile.UserName;
      if (fromUser == currentChatId) {
        messages.add(ChatMessage(
          sender: sender,
          messageContent: msg!,
          messageType: "receiver",
        ));
      }
    });
  }

  ScrollController _scrollController = ScrollController();

  _scrollToBottom() {
    _scrollController.jumpTo(_scrollController.position.maxScrollExtent);
  }
}
